using Lagrange.Core.Common;
using Lagrange.Core.Utility.Generator;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Lagrange.Core.Utils.Tencent;

/// <summary>
/// Provides methods to query QQ IMEI.
/// </summary>
public class QImei
{
    public string Q16 { get; set; }
    public string Q36 { get; set; }
}

internal static class QImeiProvider
{

    private const string Url = "https://snowflake.qq.com/ola/android";

    private static readonly HttpClient Client = new();

    private const string Secret = "ZdJqM15EeO2zWc08";
    private const string RsaKey = @"
<RSAKeyValue>
  <Exponent>AQAB</Exponent>
  <Modulus>xCMYMKLrX8KCcXBkHnnYD+xRvamiLktKs30fIFpK5E2SjNolh59mo0KQUWYzEqEn+viiRr2q9jkYQX6Q18lbWQiqai0PhS5KZ3ApSlSKwcL+jx8lL7gm9KyGq5oA585H0AKlbnxLUeuImsxgymrbyfcugfTTGx3XRkgFJkUwqx0=</Modulus>
</RSAKeyValue>
";

    public static async Task<QImei?> RequestQImei(BotDeviceInfo device, BotAppInfo appInfo)
    {
        var payload = GenRandomPayloadByDevice(device, appInfo);
        string cryptKey = StringGen.GenerateHex(16);
        long timeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        string nonce = StringGen.GenerateHex(16);

        using var rsa = RSA.Create();
        rsa.FromXmlString(RsaKey);

        string key = Convert.ToBase64String(rsa.Encrypt(Encoding.ASCII.GetBytes(cryptKey), RSAEncryptionPadding.Pkcs1));
        string param = AesEncrypt(payload.ToJsonString(), cryptKey);
        var body = new JsonObject()
        {
            {"key", key},
            {"params", param},
            {"time", timeStamp},
            {"nonce", nonce},
            {"sign", Md5($"{key}{param}{timeStamp}{nonce}{Secret}")},
            {"extra", ""}
        };

        try
        {
            // content-type: application/json
            var request = new HttpRequestMessage(HttpMethod.Post, Url)
            {
                Content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json")
            };
            var response = await Client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(result);
            if (json?["code"]?.ToJsonString() != "0") throw new Exception($"QImei request failed: {json?["code"]}");

            string data = json["data"]?.ToString();
            if (data == null) throw new Exception("QImei request failed: data is null.");
            string raw = AesDecrypt(Convert.FromBase64String(data), cryptKey);

            var rawJson = JsonNode.Parse(raw);
            if (rawJson == null) throw new Exception($"QImei request failed: parse raw json failed. {raw}");
            return new QImei() { Q16 = rawJson["q16"]?.ToString(), Q36 = rawJson["q36"]?.ToString() };
        }
        catch
        {
            return null;
        }
    }

    private static string AesEncrypt(string src, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.ASCII.GetBytes(key);
        aes.IV = Encoding.ASCII.GetBytes(key[..16]);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        return Convert.ToBase64String(encryptor.TransformFinalBlock(Encoding.ASCII.GetBytes(src), 0, src.Length));
    }

    private static string AesDecrypt(byte[] encryptedData, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.ASCII.GetBytes(key);
        aes.IV = Encoding.ASCII.GetBytes(key[..16]);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length));
    }

    private static string Md5(string src)
    {
        using var md5 = MD5.Create();
        return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(src))).Replace("-", "").ToLower();
    }

    private static JsonObject GenRandomPayloadByDevice(BotDeviceInfo device, BotAppInfo appInfo)
    {
        var reserved = new JsonObject()
        {
            { "harmony", "0" },
            { "clone", "0" },
            { "containe", "" },
            { "oz", "UhYmelwouA+V2nPWbOvLTgN2/m8jwGB+yUB5v9tysQg=" },
            { "oo", "Xecjt+9S1+f8Pz2VLSxgpw==" },
            { "kelong", "0" },
            { "uptimes", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
            { "multiUser", "0" },
            { "bod", device.Model.CodeName },
            { "brd", device.Model.BaseBand },
            { "dv", device.Model.CodeName },
            { "firstLevel", "" },
            { "manufact", device.Model.Brand },
            { "name", device.Model.DeviceName },
            { "host", "se.infra" },
            { "kernel", device.System.FingerPrint },
        };

        var random = new Random();
        string timeMonth = DateTime.Now.ToString("yyyy-MM-") + "01";
        int rand1 = random.Next(100000, 999999);
        int rand2 = random.Next(100000000, 999999999);
        var beaconId = new StringBuilder();

        for (int i = 1; i <= 40; i++)
        {
            switch (i)
            {
                case 1:
                case 2:
                case 13:
                case 14:
                case 17:
                case 18:
                case 21:
                case 22:
                case 25:
                case 26:
                case 29:
                case 30:
                case 33:
                case 34:
                case 37:
                case 38:
                    beaconId.Append($"k{i}:{timeMonth}{rand1}.{rand2}");
                    break;
                case 3:
                    beaconId.Append("k3:0000000000000000");
                    break;
                case 4:
                    beaconId.Append($"k4:{StringGen.GenerateHex(16)}");
                    break;
                default:
                    beaconId.Append($"k{i}:{random.Next(10000)}");
                    break;
            }
            beaconId.Append(';');
        }

        return new JsonObject()
        {
            { "androidId", device.System.AndroidId },
            { "platformId", 1 },
            { "appKey", appInfo.AppKey },
            { "appVersion", appInfo.BaseVersion },
            { "beaconIdSrc", beaconId.ToString() },
            { "brand", device.Model.Brand },
            { "channelId", "2017" },
            { "cid", "" },
            { "imei", device.Model.Imei },
            { "imsi", device.Model.Imsi },
            { "mac", "" },
            { "model", device.Model.DeviceName },
            { "networkType", "unknown" },
            { "oaid", "" },
            { "osVersion", $"Android {device.System.OsVersion},level {device.System.InnerVer}" },
            { "qimei", "" },
            { "qimei36", "" },
            { "sdkVersion", "1.2.13.6" },
            { "audit", "" },
            { "userId", "{}" },
            { "packageId", appInfo.PackageName },
            { "deviceType", "Phone" },
            { "sdkName", "" },
            { "reserved", reserved.ToJsonString() },
        };
    }
}
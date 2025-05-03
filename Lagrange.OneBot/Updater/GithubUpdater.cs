using System.Diagnostics;
using System.Formats.Tar;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Lagrange.OneBot.Updater.Model;
using Timer = System.Timers.Timer;

namespace Lagrange.OneBot.Updater
{
    public class GithubUpdater : IDisposable
    {
        public GithubUpdater()
        {
            LastUpdateTime = DateTime.MinValue;
            LastUpdateResources = [];
            Config = new UpdaterConfig();
            _httpClient = new HttpClient(
                new HttpClientHandler()
                {
                    Proxy = string.IsNullOrEmpty(Config.ProxyUrl)
                        ? null
                        : new WebProxy()
                        {
                            Address = new Uri(Config.ProxyUrl),
                            BypassProxyOnLocal = false,
                            UseDefaultCredentials = false,
                        },
                },
                true
            );
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Lagrange.OneBot.Updater");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

            _executablePath =
                Process.GetCurrentProcess().MainModule?.FileName
                ?? throw new Exception("Could not get executable path.");
            _executableDirectory =
                Path.GetDirectoryName(_executablePath)
                ?? throw new Exception("Could not get executable directory.");

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        private const string GithubApiUrl =
            "https://api.github.com/repos/LagrangeDev/Lagrange.Core/releases";

        public DateTime LastUpdateTime { get; set; }

        public List<GithubReleaseResource> LastUpdateResources { get; set; }

        public UpdaterConfig Config { get; set; }

        public Timer? Timer { get; set; }

        private readonly HttpClient _httpClient;

        private readonly string _executablePath;
        private readonly string _executableDirectory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public async Task GetConfig()
        {
            if (Environment.GetEnvironmentVariable("RUNNING_IN_DOCKER") == "true")
            {
                Config = new UpdaterConfig();
                return;
            }

            string configPath = "AutoUpdaterConfig.json";

            if (File.Exists(configPath))
            {
                string config = await File.ReadAllTextAsync(configPath);
                var cfg = JsonSerializer.Deserialize<UpdaterConfig>(config);

                if (cfg != null)
                {
                    Config = cfg;
                }
                else
                {
                    Console.WriteLine("Failed to parse config file, fallback to default config.");
                    var defaultConfig = new UpdaterConfig();
                    Config = defaultConfig;
                }
            }
            else
            {
                var defaultConfig = new UpdaterConfig();
                Config = defaultConfig;
            }
        }

        private async Task SetLastUpdateTime()
        {
            await File.WriteAllTextAsync(
                "AutoUpdaterConfig.json",
                JsonSerializer.Serialize(Config, _jsonSerializerOptions)
            );
        }

        public async Task<bool> CheckUpdate()
        {
            DateTime currentProgramTime;

            if (!string.IsNullOrEmpty(Config.LastUpdateTime))
            {
                currentProgramTime = DateTime.Parse(Config.LastUpdateTime);
            }
            else
            {
                var lastWriteTime = File.GetLastWriteTime(_executablePath);
                Config.LastUpdateTime = lastWriteTime.ToString(
                    "yyyy-MM-ddTHH:mm:ssZ",
                    CultureInfo.InvariantCulture
                );
                await SetLastUpdateTime();
                currentProgramTime = lastWriteTime;
            }

            await GetResponse();

            return LastUpdateTime > currentProgramTime;
        }

        public async Task Update()
        {
            Cleanup();
            var dotNetVersion = Environment.Version.Major switch
            {
                8 => DotNetVersion.DotNet8,
                9 => DotNetVersion.DotNet9,
                _ => throw new Exception("Unknown .NET version.")
            };

            var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier switch
            {
                "win-x64" => RuntimeIdentifier.WinX64,
                "win-x86" => RuntimeIdentifier.WinX86,
                "linux-x64" => RuntimeIdentifier.LinuxX64,
                "linux-arm64" => RuntimeIdentifier.LinuxArm64,
                "linux-arm" => RuntimeIdentifier.LinuxArm32,
                "osx-x64" => RuntimeIdentifier.MacX64,
                "osx-arm64" => RuntimeIdentifier.MacArm64,
                _ => throw new Exception("Unknown target platform.")
            };

            string tempPath = Path.GetTempPath();
            string tempDirectory = Path.Combine(tempPath, "Lagrange.OneBot.Updater");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            var resource = LastUpdateResources.FirstOrDefault(r =>
                r.DotNetVersion == dotNetVersion && r.RuntimeIdentifier == runtimeIdentifier
            );

            if (resource == null)
            {
                throw new Exception("No matching resource found.");
            }

            string fileName = Path.GetFileName(resource.Url);
            string filePath = Path.Combine(tempDirectory, fileName);
            var response = _httpClient.GetAsync(resource.Url).Result;
            if (response.IsSuccessStatusCode)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }
            else
            {
                throw new Exception("Failed to download the file.");
            }

            string extractPath = Path.Combine(tempDirectory, "extracted");
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            if (resource.CompressedFormat == CompressedFormat.Zip)
            {
                await Task.Run(() =>
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(filePath, extractPath, true);
                });
            }
            else if (resource.CompressedFormat == CompressedFormat.TarGz)
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Open))
                await using (
                    var gzipStream = new System.IO.Compression.GZipStream(
                        fileStream,
                        System.IO.Compression.CompressionMode.Decompress
                    )
                )
                    await TarFile.ExtractToDirectoryAsync(gzipStream, extractPath, true);
            }

            string sourceDirectory = Path.Combine(extractPath, GetSourceDirectory(resource));

            int pid = Environment.ProcessId;
            if (runtimeIdentifier.IsWindows())
            {
                string batScript = $"""
                    @echo off
                    setlocal

                    echo Waiting for process to exit...
                    :waitloop
                    tasklist /fi "pid eq {pid}" | findstr "{pid}" >nul
                    if errorlevel 1 (
                        goto end
                    )
                    timeout /t 1 >nul
                    goto waitloop
                    :end

                    set "sourcePath={Path.Combine(sourceDirectory, "Lagrange.OneBot.exe")}"
                    set "targetPath={_executablePath}"

                    xcopy /y /q ""{sourceDirectory}\\*"" ""{_executableDirectory}\\*"" /s /i

                    echo Update completed, starting new process...
                    cd "{Environment.CurrentDirectory}" && start "" "%targetPath%"
                    
                    """;
                string batFilePath = Path.Combine(tempDirectory, "update.bat");
                await File.WriteAllTextAsync(batFilePath, batScript);
                Process.Start(
                    new ProcessStartInfo("cmd.exe", $"/c {batFilePath}")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = true
                    }
                );
            }
            else if (runtimeIdentifier.IsLinux())
            {
                string shellScript = $"""
                    #!/bin/bash

                    echo "Waiting for process to exit..."
                    while ps -p {pid} > /dev/null; do
                        sleep 1
                    done

                    sourcePath="{Path.Combine(sourceDirectory, "Lagrange.OneBot")}"
                    targetPath="{_executablePath}"

                    cp  "$sourcePath" "$targetPath"

                    echo Update completed, starting new process...
                    cd {Environment.CurrentDirectory} exec "$targetPath"
                    
                    """;
                string shellFilePath = Path.Combine(tempDirectory, "update.sh");
                await File.WriteAllTextAsync(shellFilePath, shellScript);
                Process.Start(
                    new ProcessStartInfo("bash", shellFilePath)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = true,
                    }
                );
            }
            else if (runtimeIdentifier.IsMac())
            {
                //沟槽的realm+mac不能把包塞进app里
                string shellScript = $"""
                                      #!/bin/bash

                                      echo "Waiting for process to exit..."
                                      while ps -p {pid} > /dev/null; do
                                          sleep 1
                                      done

                                      sourcePath="{Path.Combine(sourceDirectory, "Lagrange.OneBot")}"
                                      targetPath="{_executablePath}"

                                      dylibSourcePath="{Path.Combine(sourceDirectory, "librealm-wrappers.dylib")}"
                                      targetDylibPath="{Path.Combine(
                                          _executableDirectory,
                                          "librealm-wrappers.dylib"
                                      )}"

                                      cp "$sourcePath" "$targetPath"
                                      cp "$dylibSourcePath" "$targetDylibPath"

                                      echo Update completed, starting new process...
                                      cd "{Environment.CurrentDirectory}" && exec "$targetPath"

                                      """;
                string shellFilePath = Path.Combine(tempDirectory, "update.sh");
                await File.WriteAllTextAsync(shellFilePath, shellScript);
                Process.Start(
                    new ProcessStartInfo("bash", shellFilePath)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = true,
                    }
                );
            }

            Console.WriteLine("Download completed. Please wait for the program to restart.");

            Config.LastUpdateTime = LastUpdateTime.ToString(
                "yyyy-MM-ddTHH:mm:ssZ",
                CultureInfo.InvariantCulture
            );
            await SetLastUpdateTime();
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        public void StartIntervalCheck()
        {
            if (Config.CheckInterval <= 0)
            {
                throw new Exception("Check interval must be greater than 0.");
            }

            Timer = new Timer(Config.CheckInterval * 1000);
            Timer.Elapsed += async (_, _) =>
            {
                try
                {
                    if (await CheckUpdate())
                    {
                        Console.WriteLine("Update available, downloading...");
                        await Update();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        $"Error checking for updates: {e.Message}, please check your network connection or config file, use proxy if needed."
                    );
                }
            };
            Timer.Start();
        }

        public void StopIntervalCheck()
        {
            Timer?.Stop();
        }

        public void Cleanup()
        {
            string tempPath = Path.GetTempPath();
            string tempDirectory = Path.Combine(tempPath, "Lagrange.OneBot.Updater");
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        public void Dispose()
        {
            Cleanup();
            _httpClient.Dispose();
            StopIntervalCheck();
            Timer?.Dispose();
        }

        private async Task GetResponse()
        {
            var response = await _httpClient.GetAsync(GithubApiUrl);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var root = JsonNode.Parse(content);
                var node0 = root?[0]?.AsObject();

                if (node0 == null)
                {
                    throw new Exception("Failed to parse JSON response.");
                }

                string? date = node0["published_at"]?.ToString();
                if (date != null)
                {
                    var dateTime = DateTime.Parse(date);
                    LastUpdateTime = dateTime;
                }
                else
                {
                    throw new Exception("Failed to parse JSON response.");
                }

                var resources = new List<GithubReleaseResource>();
                _ = node0.TryGetPropertyValue("assets", out var assetsNode);
                if (assetsNode == null)
                {
                    throw new Exception("Failed to parse JSON response.");
                }

                var assets = assetsNode.AsArray();
                foreach (var asset in assets)
                {
                    string? name = asset?["name"]?.ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    string? url = asset?["browser_download_url"]?.ToString();
                    if (string.IsNullOrEmpty(url))
                    {
                        continue;
                    }

                    string pattern =
                        @"Lagrange\.OneBot_(?<TargetPlatform>[^_]+)_net(?<DotNetVersion>[\d\.]+)_SelfContained(?<CompressedFormat>[^_]+)";

                    var match = Regex.Match(name, pattern);
                    if (!match.Success)
                    {
                        continue;
                    }

                    var targetPlatform = match.Groups["TargetPlatform"].Value.ToLower() switch
                    {
                        "win-x64" => RuntimeIdentifier.WinX64,
                        "win-x86" => RuntimeIdentifier.WinX86,
                        "linux-x64" => RuntimeIdentifier.LinuxX64,
                        "linux-arm64" => RuntimeIdentifier.LinuxArm64,
                        "linux-arm" => RuntimeIdentifier.LinuxArm32,
                        "osx-x64" => RuntimeIdentifier.MacX64,
                        "osx-arm64" => RuntimeIdentifier.MacArm64,
                        _ => throw new Exception("Unknown target platform.")
                    };
                    var dotNetVersion = match.Groups["DotNetVersion"].Value switch
                    {
                        "8.0" => DotNetVersion.DotNet8,
                        "9.0" => DotNetVersion.DotNet9,
                        _ => throw new Exception("Unknown .NET version.")
                    };
                    var compressedFormat = match.Groups["CompressedFormat"].Value.ToLower() switch
                    {
                        ".zip" => CompressedFormat.Zip,
                        ".tar.gz" => CompressedFormat.TarGz,
                        _ => throw new Exception("Unknown compressed format.")
                    };
                    resources.Add(
                        new GithubReleaseResource
                        {
                            Url = url,
                            DotNetVersion = dotNetVersion,
                            RuntimeIdentifier = targetPlatform,
                            CompressedFormat = compressedFormat
                        }
                    );
                }

                LastUpdateResources = resources;
                return;
            }

            throw new Exception("Failed to fetch release resources.");
        }

        private string GetSourceDirectory(GithubReleaseResource resource)
        {
            string path = Path.Combine(
                "Lagrange.OneBot",
                "bin",
                "Release"
            );
            switch (resource.DotNetVersion)
            {
                case DotNetVersion.DotNet8:
                    path = Path.Combine(path, "net8.0");
                    break;
                case DotNetVersion.DotNet9:
                    path = Path.Combine(path, "net9.0");
                    break;
                default:
                    throw new Exception("Unknown .NET version.");
            }

            switch (resource.RuntimeIdentifier)
            {
                case RuntimeIdentifier.WinX64:
                    path = Path.Combine(path, "win-x64");
                    break;
                case RuntimeIdentifier.WinX86:
                    path = Path.Combine(path, "win-x86");
                    break;
                case RuntimeIdentifier.LinuxX64:
                    path = Path.Combine(path, "linux-x64");
                    break;
                case RuntimeIdentifier.LinuxArm64:
                    path = Path.Combine(path, "linux-arm64");
                    break;
                case RuntimeIdentifier.LinuxArm32:
                    path = Path.Combine(path, "linux-arm");
                    break;
                case RuntimeIdentifier.MacX64:
                    path = Path.Combine(path, "osx-x64");
                    break;
                case RuntimeIdentifier.MacArm64:
                    path = Path.Combine(path, "osx-arm64");
                    break;
                default:
                    throw new Exception("Unknown target platform.");
            }

            path = Path.Combine(path, "publish");
            return path;
        }
    }
}

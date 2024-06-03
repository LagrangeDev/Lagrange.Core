using Lagrange.Core.Utility.Network;

namespace Lagrange.OneBot.Utility;

public class SongHelper
{
    public async Task<dynamic> GetQMusicInfo(string id)
    {
        const string template = """https://u.y.qq.com/cgi-bin/musicu.fcg?format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0&data={"comm":{"ct":24,"cv":0},"songinfo":{"method":"get_song_detail_yqq","param":{"song_type":0,"song_mid":"","song_id":*},"module":"music.pf_song_detail_svr"}}""";
        string url = template.Replace("*", id);
        string response = await Http.GetAsync(url);

        return response;
    }

    public async Task<dynamic> GetNetEaseMusicInfo(string id)
    {
        string url = $"http://music.163.com/api/song/detail/?id={id}&ids=[{id}]";
        string response = await Http.GetAsync(url);

        return response;
    }
}
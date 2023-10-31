using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nonno.Text.Communication;
public class LineNotifyForum : IForum
{
    public static readonly string LINE_URL = "https://notify-api.line.me/api/notify";

    public string? Token { get; set; }

    public void AddText(Text text)
    {

    }

    public async Task Send(string message)
    {
        if (Token is null) throw new Exception();

        using (var client = new HttpClient())
        {
            // 通知するメッセージ
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]{ new("message", message) });
            // ヘッダーにアクセストークンを追加
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            // 実行
            var result = await client.PostAsync(LINE_URL, content);
        }
    }
}

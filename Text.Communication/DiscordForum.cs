using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace Nonno.Text.Communication;
public class DiscordForum<TText> : IForum<TText> where TText : Text, IPersistent<Text>
{
    private readonly DiscordSocketClient _client;

    public event TextAddedEventHandler<TText>? TextAdded;

    public string Token { get; }

    public DiscordForum(string token)
    {
        _client = new DiscordSocketClient();
        _client.Log += LogAsync;
        _client.Ready += onReady;
        _client.MessageReceived += Receive;

        Token = token;
    }

    public async Task MainAsync()
    {
        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    private Task onReady()
    {
        Console.WriteLine($"{_client.CurrentUser} is Running!!");
        return Task.CompletedTask;
    }

    protected virtual async Task Receive(SocketMessage message)
    {
        if (message.Author.Id == _client.CurrentUser.Id) return;

        
        TextAdded?.Invoke(this, );
        if (message.Content == "こんにちは")
        {
            await message.Channel.SendMessageAsync("こんにちは、" + message.Author.Username + "さん！");
        }
    }

    public void AddText(TText text)
    {
        if (text.Latest is Text)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Polumna007.Logic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler : IDisposable
{
    private readonly TelegramBotClient _bot;

    public OnMessageHandler(TelegramBotClient bot)
    {
        _bot = bot;
        _bot.OnMessage += OnMessage;
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text is null) 
            return;
        Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
        await _bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");

        var generator = new ReactionsGenerator();
        await _bot.SendMessage(msg.Chat, $"rating ascii: {generator.GetAsciiEmoji(0.9f)}");
        var analyst = new FileAnalyst();
        await _bot.SendMessage(msg.Chat, $"txt.file length: {analyst.FileLength("D:\\hackaton\\5acceptor\\.gitignore")}");
    }

    public void Dispose()
    {
        _bot.OnMessage -= OnMessage;
    }
}

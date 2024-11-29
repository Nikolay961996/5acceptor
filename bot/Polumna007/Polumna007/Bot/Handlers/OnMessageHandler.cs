using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler
{
    private readonly TelegramBotClient _bot;

    public OnMessageHandler(TelegramBotClient bot)
    {
        _bot = bot;
        _bot.OnMessage += OnMessage;
    }

    // method that handle messages received by the bot:
    public async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text is null) 
            return;
        Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
        await _bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
    }
}

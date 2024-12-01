using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Polumna007.Logic;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler : IDisposable
{
    private readonly string _token;
    private User? _me;
    private readonly TelegramBotClient _bot;
    private readonly UsersContext _usersContext;
    private readonly EmailSender _emailSender;
    private readonly DocumentHandler _documentHandler;
    private readonly BotCommandsHandler _botCommandsHandler;

    public OnMessageHandler(string token, TelegramBotClient bot, BotKeyboard botKeyboard, UsersContext usersContext)
    {
        _emailSender = new EmailSender();
        _documentHandler = new DocumentHandler(bot, botKeyboard);
        _botCommandsHandler = new BotCommandsHandler(bot, usersContext);
        _token = token;
        _bot = bot;
        _usersContext = usersContext;
        _bot.OnMessage += OnMessage;
        _bot.OnUpdate += OnUpdate;
    }

    private async Task OnUpdate(Update update)
    {
        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
                await OnCallbackQuery(update.CallbackQuery);
                break;
            default:
                break;
        }
    }

    private async Task OnCallbackQuery(CallbackQuery? callbackQuery)
    {
        if (callbackQuery is null)
            return;

        var buttonId = callbackQuery.Data;
        if (buttonId == "email")
        {
            await _emailSender.Send(callbackQuery, _bot, _usersContext, _token);
        }
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Document is not null)
        {
            await _documentHandler.Handle(msg.Document, msg.Chat);
            return;
        }
        if (msg.Text is null) 
            return;
        if (!msg.Text.StartsWith('/'))
        {
            await TextHandler(msg);
            return;
        }

        _me ??= await _bot.GetMe();
        var space = msg.Text.IndexOf(' ');
        if (space < 0) 
            space = msg.Text.Length;
        var command = msg.Text[..space].ToLower();
        if (command.LastIndexOf('@') is > 0 and int at)
        {
            if (!command[(at + 1)..].Equals(_me.Username, StringComparison.OrdinalIgnoreCase))
                return; // command was not targeted at bot

            command = command[..at];
        }
        await CommandHandler(command, msg.Text[space..].TrimStart(), msg);
    }

    public void Dispose()
    {
        _bot.OnMessage -= OnMessage;
        _bot.OnUpdate -= OnUpdate;
        _documentHandler.Dispose();
    }

    private async Task TextHandler(Message msg)
    {
        Console.WriteLine($"Received text '{msg.Text}' in {msg.Chat}");
        //await CommandHandler("/start", "", msg);

        var generator = new ReactionsGenerator();
        await _bot.SendMessage(msg.Chat, $"rating ascii: {generator.GetAsciiEmoji(0.9f)}");
        var analyst = new FileAnalyst();
        await _bot.SendMessage(msg.Chat, $"txt.file length: {analyst.FileLength("D:\\hackaton\\5acceptor\\.gitignore")}");
    }

    private async Task CommandHandler(string command, string args, Message msg)
    {
        Console.WriteLine($"Received command: {command} {args}");

        switch (command)
        {
            case "/start":
                await _botCommandsHandler.OnStart(msg);
                break;
            case "/help":
                await _botCommandsHandler.OnHelp(msg);
                break;
            case "/set_email":
                await _botCommandsHandler.OnSetEmail(args, msg);
                break;
            default:
                break;
        }
    }
}

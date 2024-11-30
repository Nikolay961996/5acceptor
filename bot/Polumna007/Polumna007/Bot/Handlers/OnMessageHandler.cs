using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;
using static Telegram.Bot.TelegramBotClient;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Net.Mail;
using Polumna007.Logic;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler : IDisposable
{
    private readonly string _token;
    private readonly TelegramBotClient _bot;
    private readonly BotKeyboard _botKeyboard;
    private readonly UsersContext _usersContext;
    private User _me;
    private const string _fileStoragePath = "FileStorage";

    public IEnumerable<BotCommand> BotCommands { get; } =
    [
        new() { Command = "/start", Description = "show an introductory message" },
        new() { Command = "/help", Description = "show all available commands" },
        new() { Command = "/set_email", Description = "set user email" }
    ];

    public OnMessageHandler(string token, TelegramBotClient bot, BotKeyboard botKeyboard, UsersContext usersContext)
    {
        Directory.CreateDirectory(_fileStoragePath);

        _token = token;

        _bot = bot;
        _bot.OnMessage += OnMessage;
        _bot.OnUpdate += _bot_OnUpdate;

        _botKeyboard = botKeyboard;

        _usersContext = usersContext;

        _me = _bot.GetMe().Result;

        TelegramBotClientExtensions.SetMyCommands(_bot, BotCommands);
    }

    public void Dispose()
    {
        _bot.OnMessage -= OnMessage;
        _bot.OnUpdate -= _bot_OnUpdate;
    }

    private async Task _bot_OnUpdate(Update update)
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
            if ((callbackQuery.Message is null) || (callbackQuery.Message.Document is null))
                return;

            var userInfo = _usersContext.GetUserInfo(callbackQuery.From.Id);
            if (userInfo is null)
                return;

            if (string.IsNullOrEmpty(userInfo.Email))
            {
                await _bot.AnswerCallbackQuery(callbackQuery.Id, "⚠️ Email not set. Use /set_email command");
                return;
            }

            await _bot.AnswerCallbackQuery(callbackQuery.Id, $"File sent to {userInfo.Email}");

            var fileId = callbackQuery.Message.Document.FileId;
            var file = TelegramBotClientExtensions.GetFile(_bot, fileId).Result;
            var fileFullPath = $"https://api.telegram.org/file/bot{_token}/{file.FilePath}";
            // TODO: send file by email
            // Email: userInfo.Email
        }
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Document is not null)
        {
            await DocumentHandler(msg.Document, msg.Chat);
            return;
        }

        if (msg.Text is null) 
            return;

        if (!msg.Text.StartsWith('/'))
        {
            await TextHandler(msg);
            return;
        }

        var space = msg.Text.IndexOf(' ');
        if (space < 0) space = msg.Text.Length;

        var command = msg.Text[..space].ToLower();
        if (command.LastIndexOf('@') is > 0 and int at)
        {
            if (!command[(at + 1)..].Equals(_me.Username, StringComparison.OrdinalIgnoreCase))
                return; // command was not targeted at bot

            command = command[..at];
        }

        await CommandHandler(command, msg.Text[space..].TrimStart(), msg);
    }

    private async Task DocumentHandler(Document doc, Chat chat)
    {
        if (doc.FileSize is null or 0 || doc.FileName is null)
        {
            await _bot.SendMessage(chat, $"{doc.FileName} is empty");
            return;
        }

        var savedFilePath = await SaveFileToStorage(doc);
        var resultFilePath = await SendToMl(savedFilePath);
        await SendResultFile(chat.Id, resultFilePath, doc.FileName);
        System.IO.File.Delete(savedFilePath);
        System.IO.File.Delete(resultFilePath);
    }

    private async Task TextHandler(Message msg)
    {
        Console.WriteLine($"Received text '{msg.Text}' in {msg.Chat}");
        await CommandHandler("/start", "", msg);
    }

    private async Task CommandHandler(string command, string args, Message msg)
    {
        Console.WriteLine($"Received command: {command} {args}");

        switch (command)
        {
            case "/start":
                await OnStart(msg);
                break;
            case "/help":
                await OnHelp(msg);
                break;
            case "/set_email":
                await OnSetEmail(args, msg);
                break;
            default:
                break;
        }
    }

    async Task OnStart(Message msg)
    {
        await _bot.SendMessage(msg.Chat, "I can review your code. Just send me a file 😉");
    }

    async Task OnHelp(Message msg)
    {
        var sb = new StringBuilder("<b>Bot commands</b>\n");
        foreach (var botCommand in BotCommands)
        {
            sb.AppendLine($"{botCommand.Command} - {botCommand.Description}");
        }
        await _bot.SendMessage(msg.Chat, sb.ToString(), parseMode: ParseMode.Html);
    }

    async Task OnSetEmail(string email, Message msg)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            await _bot.SendMessage(msg.Chat, "⚠️ Email is empty");
            return;
        }

        if (!EmailValidator.IsValid(email))
        {
            await _bot.SendMessage(msg.Chat, "⚠️ Incorrect email format");
            return;
        }

        if (_usersContext is null)
            return;

        if (msg.From is null)
            throw new ArgumentNullException("msg.From");

        var userInfo = _usersContext.GetUserInfo(msg.From.Id);
        if (userInfo is null)
            return;

        userInfo.Email = email;
        await _bot.SendMessage(msg.Chat, "✅ Email successfully updated");
    }

    private async Task<string> SaveFileToStorage(Document doc)
    {
        var path = Path.Combine(_fileStoragePath, doc.FileName!);
        await using Stream fileStream = System.IO.File.Create(path);
        _ = await _bot.GetInfoAndDownloadFile(doc.FileId, fileStream);
        fileStream.Close();
        return path;
    }

    [Obsolete("ML")]
    private async Task<string> SendToMl(string sourceFilePath)
    {
        await Task.Delay(100);
        var resultPath = Path.Combine(_fileStoragePath, Path.GetFileNameWithoutExtension(sourceFilePath) + "-review.pdf");
        System.IO.File.Copy(sourceFilePath, resultPath, true);
        return resultPath;
    }

    private async Task SendResultFile(long chatId, string resultFilePath, string fileName)
    {
        await using Stream readStream = System.IO.File.OpenRead(resultFilePath);
        var inputFile = InputFile.FromStream(readStream);
        var inlineMarkup = _botKeyboard.GetReviewMarkup("email"); // We don't know file id here
        await _bot.SendDocument(new ChatId(chatId), inputFile, $"Review: {fileName}", replyMarkup: inlineMarkup);
    }
}

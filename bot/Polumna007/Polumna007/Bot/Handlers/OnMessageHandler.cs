using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Polumna007.Logic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler : IDisposable
{
    private readonly TelegramBotClient _bot;
    private const string _fileStoragePath = "FileStorage";

    public OnMessageHandler(TelegramBotClient bot)
    {
        Directory.CreateDirectory(_fileStoragePath);

        _bot = bot;
        _bot.OnMessage += OnMessage;
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
        Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
        await _bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");

        var generator = new ReactionsGenerator();
        await _bot.SendMessage(msg.Chat, $"rating ascii: {generator.GetAsciiEmoji(0.9f)}");
        var analyst = new FileAnalyst();
        await _bot.SendMessage(msg.Chat, $"txt.file length: {analyst.FileLength("D:\\hackaton\\5acceptor\\.gitignore")}");
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
        await SendResultFile(chat.Id, resultFilePath);
        System.IO.File.Delete(savedFilePath);
        System.IO.File.Delete(resultFilePath);
    }

    public void Dispose()
    {
        _bot.OnMessage -= OnMessage;
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
        System.IO.File.Copy(sourceFilePath, resultPath);
        return resultPath;
    }

    private async Task SendResultFile(long chatId, string resultFilePath)
    {
        await using Stream readStream = System.IO.File.OpenRead(resultFilePath);
        var inputFile = InputFile.FromStream(readStream);
        await _bot.SendDocument(new ChatId(chatId), inputFile);
    }
}

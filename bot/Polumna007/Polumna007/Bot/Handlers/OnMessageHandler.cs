using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Integration;

namespace Polumna007.Bot.Handlers;

public class OnMessageHandler : IDisposable
{
    private readonly NeuroSender _neuroSender;
    private readonly TelegramBotClient _bot;
    private const string _fileStoragePath = "FileStorage";

    public OnMessageHandler(TelegramBotClient bot)
    {
        Directory.CreateDirectory(_fileStoragePath);

        _bot = bot;
        _neuroSender = new NeuroSender();
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
        _neuroSender.Dispose();
    }

    private async Task<string> SaveFileToStorage(Document doc)
    {
        var path = Path.Combine(_fileStoragePath, doc.FileName!);
        await using Stream fileStream = System.IO.File.Create(path);
        _ = await _bot.GetInfoAndDownloadFile(doc.FileId, fileStream);
        fileStream.Close();
        return path;
    }

    private async Task<string> SendToMl(string sourceFilePath)
    {
        var resultPath = Path.Combine(_fileStoragePath, Path.GetFileNameWithoutExtension(sourceFilePath) + "-review.pdf");
        await _neuroSender.Send(sourceFilePath, resultPath);
        return resultPath;
    }

    private async Task SendResultFile(long chatId, string resultFilePath)
    {
        await using Stream readStream = System.IO.File.OpenRead(resultFilePath);
        var inputFile = InputFile.FromStream(readStream);
        await _bot.SendDocument(new ChatId(chatId), inputFile);
    }
}

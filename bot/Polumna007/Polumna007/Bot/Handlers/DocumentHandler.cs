using Integration;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Polumna007.Bot.Handlers;
public class DocumentHandler : IDisposable
{
    private readonly TelegramBotClient _bot;
    private readonly BotKeyboard _botKeyboard;
    private readonly NeuroSender _neuroSender;
    private const string _fileStoragePath = "FileStorage";

    public DocumentHandler(TelegramBotClient bot, BotKeyboard botKeyboard)
    {
        Directory.CreateDirectory(_fileStoragePath);
        _bot = bot;
        _botKeyboard = botKeyboard;
        _neuroSender = new NeuroSender();
    }

    public async Task Handle(Document doc, Chat chat)
    {
        if (doc.FileSize is null or 0 || doc.FileName is null)
        {
            await _bot.SendMessage(chat, $"{doc.FileName} is empty");
            return;
        }

        await _bot.SendMessage(chat, $"{doc.FileName} файл отправлен на ревью. Пожалуйста подождите...");
        var savedFilePath = await SaveFileToStorage(doc);
        var resultFilePath = await SendToMl(savedFilePath, chat);
        await SendResultFile(chat.Id, resultFilePath, doc.FileName);
        System.IO.File.Delete(savedFilePath);
        System.IO.File.Delete(resultFilePath);
    }

    public void Dispose()
    {
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

    private async Task<string> SendToMl(string sourceFilePath, Chat chat)
    {
        var resultPath = Path.Combine(_fileStoragePath, Path.GetFileNameWithoutExtension(sourceFilePath) + "-review.txt");
        try
        {
            await _neuroSender.Send(sourceFilePath, resultPath);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(chat, $"{sourceFilePath} ошибка при ревью файла: {ex.Message}");

        }
        return resultPath;
    }

    private async Task SendResultFile(long chatId, string resultFilePath, string fileName)
    {
        await using Stream readStream = System.IO.File.OpenRead(resultFilePath);
        var inputFile = InputFile.FromStream(readStream);
        var inlineMarkup = _botKeyboard.GetEmailKeyboard("email"); // We don't know file id here
        await _bot.SendDocument(new ChatId(chatId), inputFile, $"Review: {fileName}", replyMarkup: inlineMarkup);
    }
}

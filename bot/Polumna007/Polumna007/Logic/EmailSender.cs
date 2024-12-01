using Polumna007.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Polumna007.Logic;

public class EmailSender
{
    public async Task Send(CallbackQuery callbackQuery, TelegramBotClient bot, UsersContext usersContext, string token)
    {
        if ((callbackQuery.Message is null) || (callbackQuery.Message.Document is null))
            return;

        var userInfo = usersContext.GetUserInfo(callbackQuery.From.Id);
        if (userInfo is null)
            return;
        if (string.IsNullOrEmpty(userInfo.Email))
        {
            await bot.AnswerCallbackQuery(callbackQuery.Id, "⚠️ Email not set. Use /set_email command");
            return;
        }
        await bot.AnswerCallbackQuery(callbackQuery.Id, $"File sent to {userInfo.Email}");

        var fileId = callbackQuery.Message.Document.FileId;
        var file = TelegramBotClientExtensions.GetFile(bot, fileId).Result;
        var fileFullPath = $"https://api.telegram.org/file/bot{token}/{file.FilePath}";
        // TODO: send file by email
        // Email: userInfo.Email
    }
}

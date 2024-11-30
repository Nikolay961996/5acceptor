using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Polumna007.Bot;

public class BotKeyboard
{
    public InlineKeyboardMarkup GetReviewMarkup(string data)
    {
        var inlineMarkup = new InlineKeyboardMarkup();
        inlineMarkup.AddButton("Send to email", data);
        return inlineMarkup;
    }
}

using Telegram.Bot.Types.ReplyMarkups;

namespace Polumna007.Bot;

public class BotKeyboard
{
    public InlineKeyboardMarkup GetEmailKeyboard(string data)
    {
        var inlineMarkup = new InlineKeyboardMarkup();
        inlineMarkup.AddButton("Send to email", data);
        return inlineMarkup;
    }
}

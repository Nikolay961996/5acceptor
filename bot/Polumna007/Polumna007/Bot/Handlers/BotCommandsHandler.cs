using Polumna007.Logic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Polumna007.Bot.Handlers;

public class BotCommandsHandler
{
    private readonly TelegramBotClient _bot;
    private readonly UsersContext _usersContext;

    public BotCommandsHandler(TelegramBotClient bot, UsersContext usersContext)
    {
        _bot = bot;
        _usersContext = usersContext;
        TelegramBotClientExtensions.SetMyCommands(_bot, BotCommands);
    }

    public IEnumerable<BotCommand> BotCommands { get; } =
    [
        new() { Command = "/start", Description = "show an introductory message" },
        new() { Command = "/help", Description = "show all available commands" },
        new() { Command = "/set_email", Description = "set user email" }
    ];

    public async Task OnStart(Message msg)
    {
        await _bot.SendMessage(msg.Chat, "I can review your code. Just send me a file 😉");
    }

    public async Task OnHelp(Message msg)
    {
        var sb = new StringBuilder("<b>Bot commands</b>\n");
        foreach (var botCommand in BotCommands)
        {
            sb.AppendLine($"{botCommand.Command} - {botCommand.Description}");
        }
        await _bot.SendMessage(msg.Chat, sb.ToString(), parseMode: ParseMode.Html);
    }

    public async Task OnSetEmail(string email, Message msg)
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
}

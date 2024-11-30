using Polumna007.Bot.Handlers;
using Telegram.Bot;

namespace Polumna007.Bot;

public class Bot : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly OnMessageHandler _onMessageHandler;
    private readonly UsersContext _usersContext;
    private readonly BotKeyboard _botKeyboard;

    public Bot(string token)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        var bot = new TelegramBotClient(token, cancellationToken: _cancellationTokenSource.Token);
        _botKeyboard = new BotKeyboard();
        _usersContext = new UsersContext();
        _onMessageHandler = new OnMessageHandler(bot, _botKeyboard, _usersContext);
    }

    public void Dispose()
    {
        _onMessageHandler.Dispose();
        _cancellationTokenSource.Cancel();
    }
}

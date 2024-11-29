using Polumna007.Bot.Handlers;
using Telegram.Bot;

namespace Polumna007.Bot;

public class Bot : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly OnMessageHandler _onMessageHandler;

    public Bot(string token)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var bot = new TelegramBotClient(token, cancellationToken: _cancellationTokenSource.Token);
        _onMessageHandler = new OnMessageHandler(bot);
    }

    public void Dispose()
    {
        _onMessageHandler.Dispose();
        _cancellationTokenSource.Cancel();
    }
}

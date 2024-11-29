using Polumna007.Bot.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Polumna007.Bot;

public class Bot : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Handlers.OnMessageHandler _onMessageHandler;

    public Bot(string token)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var bot = new TelegramBotClient(token, cancellationToken: _cancellationTokenSource.Token);
        _onMessageHandler = new Handlers.OnMessageHandler(bot);

    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
}

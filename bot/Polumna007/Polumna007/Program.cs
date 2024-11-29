using Polumna007.Logic;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var generator = new ReactionsGenerator();
Console.WriteLine(generator.GetAsciiEmoji(0.9f));

//using Telegram.Bot;

//var token = "7457083066:AAHXkUwQLcfq5U1z9e2YlJHslGGMfvlb1oQ";
//var bot = new TelegramBotClient(token);
//var me = await bot.GetMe();
//Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

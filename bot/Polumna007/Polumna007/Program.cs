using Polumna007.Logic;
using Polumna007.Bot;

var generator = new ReactionsGenerator();
Console.WriteLine("Nigora hello");
Console.WriteLine(generator.GetAsciiEmoji(0.25f));


var token = "7457083066:AAHXkUwQLcfq5U1z9e2YlJHslGGMfvlb1oQ";
var bot = new Bot(token);

Console.WriteLine("Press any key for exit");
Console.ReadLine();
bot.Dispose();
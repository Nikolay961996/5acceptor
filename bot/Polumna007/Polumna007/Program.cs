using Polumna007.Logic;
using System.Text;
using Polumna007.Bot;

Console.OutputEncoding = Encoding.UTF8;

var generator = new ReactionsGenerator();
Console.WriteLine(generator.GetAsciiEmoji(0.9f));


var token = "7457083066:AAHXkUwQLcfq5U1z9e2YlJHslGGMfvlb1oQ";
var bot = new Bot(token);

Console.WriteLine("Press any key for exit");
Console.ReadLine();
bot.Dispose();
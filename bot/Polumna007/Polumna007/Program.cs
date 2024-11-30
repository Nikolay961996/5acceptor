using System.Text;
using Polumna007.Bot;
using Grpc.Net.Client;
using FileTransfer;
using Google.Protobuf;
using static FileTransfer.FileTransferService;

Console.OutputEncoding = Encoding.UTF8;

var token = "7457083066:AAHXkUwQLcfq5U1z9e2YlJHslGGMfvlb1oQ";
var bot = new Bot(token);

Console.WriteLine("Press any key for exit");
Console.ReadLine();
bot.Dispose();
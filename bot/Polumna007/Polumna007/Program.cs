﻿using System.Text;
using Polumna007.Bot;

Console.OutputEncoding = Encoding.UTF8;

var token = "7457083066:AAHXkUwQLcfq5U1z9e2YlJHslGGMfvlb1oQ";
var bot = new Bot(token);

Console.WriteLine("bot started...");
Console.ReadLine();
bot.Dispose();
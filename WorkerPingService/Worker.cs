using System.Net.NetworkInformation;
using System;
using System.Linq;
using TL;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

namespace WorkerPingService;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }


    private static string Token { set; get; } = "6175011160:AAG2u4ZBD4lw8_-2RNcnGUEwLPOCSclifAg";
    private static BotClient botClient = new BotClient(Token);


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string s = ""; 
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            StreamReader f = new StreamReader("Ips.txt");
            while (!f.EndOfStream && f != null)
            {
                try
                {
                    s = f.ReadLine();
                
                    Ping MyPing = new Ping();
                    if (s != null)
                    {
                        PingReply reply = MyPing.Send(s, 1000);

                        if (reply != null)
                        {
                            Console.WriteLine("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);
                            //Console.WriteLine(reply.ToString());

                        }
                    }
               }
                catch
                {

                    if (s != null)
                    {
                    
                        botClient.SendMessage(chatId: "-990387796", " ERROR: IP: " + s + "   is not available now"); // may be include var chatId by Message.update?

                    }
                }
            }
            f.Close();
            Console.ReadLine();

            await Task.Delay(1000, stoppingToken);
        }
    }
}


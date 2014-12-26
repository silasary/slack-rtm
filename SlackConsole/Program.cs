using SlackRTM;
using SlackRTM.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SlackConsole
{
    /// <summary>
    /// Sample Application
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var Instance = new Slack();
            string token="";
            bool success ;
            Instance.OnEvent += Instance_OnEvent;
            do
            {
                if (File.Exists("token.txt"))
                    token = File.ReadAllText("token.txt");
                if (String.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Please obtain a Token, and paste it below:");
                    token = Console.ReadLine();
                    File.WriteAllText("token.txt", token);

                }
                success = Instance.Init(token);
                if (!success)
                    File.WriteAllText("token.txt", "");
            } while (!success);
            Instance.Connect();
            while (Instance.Connecting || Instance.Connected)
                Thread.Sleep(0);
            Console.WriteLine("===Disconnected===");
            Console.ReadKey();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.WriteAllText("crash.txt", e.ExceptionObject.ToString());
        }

        static void Instance_OnEvent(object sender, SlackEventArgs e)
        {
            var instance = sender as Slack;
            if (e.Data.Type == "hello")
            {
                Console.WriteLine("Connected to '{0}' as '{1}'", instance.TeamInfo.Name, instance.Self.Name);
                instance.SendMessage("#botspam", "Whoo! Spam from a bot!");
            }
            else
            {
                Console.WriteLine(e.Data.Type);
                Console.WriteLine(">{0}", e.Data);
            }
            if (e.Data.Type == "message")
            {
                var message = e.Data as Message;

            }
        }
    }
}

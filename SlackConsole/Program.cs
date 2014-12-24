using SlackRTM;
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
            while (Instance.Connected)
                Thread.Sleep(0);
        }

        static void Instance_OnEvent(object sender, SlackEventArgs e)
        {
            var instance = sender as Slack;
            if (e.Data.Type == "hello")
            {
                Console.WriteLine("Connected to '{0}' as '{1}'", instance.TeamInfo.Name, instance.Self.Name);
            }
            else
            {
                Console.WriteLine(e.Data.Type);
                Console.WriteLine(">{0}", e.Data);
            }
        }
    }
}

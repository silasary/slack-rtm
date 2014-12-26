using System;
using NUnit.Framework;
using SlackRTM;
using System.Threading;
using System.IO;

namespace UnitTests
{
    public partial class UnitTests
    {
        public static string Token = null;
        public UnitTests()
        {
            Token = Environment.GetEnvironmentVariable("SLACK_TOKEN");
            if (String.IsNullOrWhiteSpace(Token))
                Token = File.ReadAllText("token.txt");
        }

        [Test]
        public void TestStartInvalid()
        {
            var slack = new Slack();
            Assert.IsFalse(slack.Init("invalid"));
        }

        [Test]
        public void TestStartValid()
        {
            var slack = new Slack();
            Assert.IsTrue(slack.Init(Token));
        }

        [Test]
        public void TestConnect()
        {
            var slack = new Slack();
            Assert.IsTrue(slack.Init(Token),"Invalid Token");
            Assert.IsTrue(slack.Connect(), "Failed to Connect");

        }

        [Test]
        public void TestSend()
        {
            var slack = new Slack();
            Assert.IsTrue(slack.Init(Token), "Invalid Token");
            Assert.IsTrue(slack.Connect(), "Failed to Connect");
            while (!slack.RecievedHello)
                Thread.Sleep(0);
            slack.SendMessage(slack.GetChannel("#botspam").Id, "Test!");
            slack.SendMessage("#botspam", "Test2");
        }
    }
}

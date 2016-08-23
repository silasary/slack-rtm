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
            if (String.IsNullOrWhiteSpace(Token) && File.Exists("token.txt"))
                Token = File.ReadAllText("token.txt");
        }

        [Test]
        public void TestStartInvalid()
        {
            var slack = new Slack("invalid");
            Assert.IsFalse(slack.Init());
        }

        [Test]
        public void TestStartValid()
        {
            if (String.IsNullOrWhiteSpace(Token))
            {
                Assert.Inconclusive("No valid token specified");
                return;
            }
            var slack = new Slack(Token);
            Assert.IsTrue(slack.Init());
        }

        [Test]
        public void TestConnect()
        {
            if (String.IsNullOrWhiteSpace(Token))
            {
                Assert.Inconclusive("No valid token specified");
                return;
            }
            var slack = new Slack(Token);
            Assert.IsTrue(slack.Init(),"Invalid Token");
            Assert.IsTrue(slack.Connect(), "Failed to Connect");

        }

        [Test]
        [Ignore("Spammy")]
        public void TestSend()
        {
            var slack = new Slack(Token);
            Assert.IsTrue(slack.Init(), "Invalid Token");
            Assert.IsTrue(slack.Connect(), "Failed to Connect");
            while (!slack.RecievedHello)
                Thread.Sleep(0);
            slack.SendMessage(slack.GetChannel("#botspam").Id, "Test!");
            slack.SendMessage("#botspam", "Test2");
        }
    }
}

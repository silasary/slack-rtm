using System;
using NUnit.Framework;
using SlackRTM;
using System.Threading;

namespace UnitTests
{
    public partial class UnitTests
    {
        // NOTE: You must create a partial class defining `const string Token = "xoxp-etc"`.

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
            //slack.Send(new Events.Message(
        }
    }
}

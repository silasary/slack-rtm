using System;
using NUnit.Framework;
using SlackRTM;

namespace UnitTests
{
    public partial class UnitTests
    {
        // NOTE: You must create a partial class defining `const string Token = "xoxp-etc"`.

        [Test]
        public void TestStartInvalid()
        {
            var slack = new Slack();
            Assert.IsFalse(slack.Start("invalid"));
        }

        [Test]
        public void TestStartValid()
        {
            var slack = new Slack();
            Assert.IsTrue(slack.Start(Token));
        }

        [Test]
        public void TestConnect()
        {
            var slack = new Slack();
            Assert.IsTrue(slack.Start(Token),"Invalid Token");
            Assert.IsTrue(slack.Connect(), "Failed to Connect");
        }
    }
}

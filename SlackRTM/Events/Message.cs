using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM.Events
{
    public class Message : Event
    {
        internal Message()
        {
        }
        internal Message(Newtonsoft.Json.Linq.JObject data)
        {
            this.Channel = data.Value<string>("channel");
            this.User = data.Value<string>("user");
            this.Text = data.Value<string>("text");
            this.TimeStamp = data.Value<string>("ts");
            this.SubType = data.Value<string>("subtype");
            this.Hidden = data.Value<string>("hidden");
            this.Id = -1;
        }

        internal Message(string channel, string text, int id)
        {
            // TODO: Complete member initialization
            this.Channel = channel;
            this.Text = text;
            this.Id = id;
        }

        public string Channel { get; private set; }

        public string User { get; private set; }

        public string Text { get; private set; }

        public string TimeStamp { get; private set; }

        public string SubType { get; private set; }

        public string Hidden { get; private set; }

        public override string Type { get { return "message"; } }

        public override string ToString()
        {
            return string.Format("<#{0}> <@{1}>: {2}", Channel, User, Text);
        }

        public int Id { get; set; }
    }
}

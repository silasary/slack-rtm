using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM.Events
{
    class Ack : Event
    {
        public override string Type
        {
            get { return "Ack"; }
        }

        public bool Ok { get; set; }

        public int ReplyTo { get; set; }

        [JsonProperty("ts")]
        public string Timestamp { get; set; }

        public string Text { get; set; }

        public dynamic Error { get; set; }

        public override string ToString()
        {
            return ToJson();
        }
    }
}

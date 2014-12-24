using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM.Events
{
    class UnknownEvent : Event
    {
        public Newtonsoft.Json.Linq.JObject data;

        public UnknownEvent(JObject data)
        {
            // TODO: Complete member initialization
            this.data = data;
        }

        public override string Type
        {
            get { return data["type"].ToString(); }
        }
    }
}

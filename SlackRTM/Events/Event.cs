using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM.Events
{
    public abstract class Event
    {
        public static Event NewEvent(string Json)
        {
            var data = JObject.Parse(Json);
            switch (data["type"].ToString())
            {
                case "hello":
                    return new Hello();
                case "message":
                    return new Message(data);
                default:
                    return new UnknownEvent(data);
            }
        }

        public abstract string Type { get; }
    }
}

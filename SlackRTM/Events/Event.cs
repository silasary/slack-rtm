using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
            return JsonConvert.DeserializeObject<Event>(Json, new SlackJsonConverter(null));
            //var data = JObject.Parse(Json);
            //if (data["type"] == null)
            //{
            //    return null;

            //}
            //else
            //switch (data["type"].ToString())
            //{
            //    case "hello":
            //        return new Hello();
            //    case "message":
            //        return new Message(data);
            //    default:
            //        return new UnknownEvent(data);
            //}
        }

        public abstract string Type { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new SlackJsonConverter(null));
        }
    }
}

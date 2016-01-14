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
        }

        public abstract string Type { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new SlackJsonConverter(null));
        }
    }
}

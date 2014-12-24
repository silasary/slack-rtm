using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM
{
  public  class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("real_name")]
        public string RealName { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("presence")]
        public string Presence { get; set; }


    }
}

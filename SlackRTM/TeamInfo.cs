using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM
{
  public  class TeamInfo
    {
        //[JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("domain")]
        public string Domain { get; set; }

        //There are more, but I don't need them right now.
    }
}

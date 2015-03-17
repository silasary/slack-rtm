using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public Slack SlackInstance { get; set; }

        public Im OpenIm()
        {
            var im = SlackInstance.Ims.FirstOrDefault(i => i.User == this.Id);
            if (im == null)
            {
                var r = SlackInstance.Api("im.open", new JProperty("user", this.Id));
                im = new Im();
                im.Id = r["channel"]["id"].Value<string>();
                im.User = this.Id;
                SlackInstance.Ims.Add(im);
            }
            return im;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebSocketSharp;

namespace SlackRTM
{
    /// <summary>
    /// The core Slack class.
    /// </summary>
    public class Slack
    {
        private TeamInfo teamInfo = null;

        private WebClient wc = new WebClient();

        private WebSocket webSocket;

        public List<Channel> Channels { get; set; }

        public User Self { get; set; }

        public List<User> Users { get; set; }

        private string Url { get; set; }

        public bool Start(string token)
        {
            JObject response = JObject.Parse(wc.DownloadString(
                new UriBuilder("https://slack.com/api/rtm.start") { Query = "token=" + token }.Uri));
            if (response["ok"].Value<bool>() == false)
            {
                Console.WriteLine(response["error"]);
                return false;
            }
            teamInfo = JsonConvert.DeserializeObject<TeamInfo>(response["team"].ToString());
            Channels = JsonConvert.DeserializeObject<List<Channel>>(response["channels"].ToString());
            Users = JsonConvert.DeserializeObject<List<User>>(response["users"].ToString());
            Self = Users.First(n => n.Id == response["self"]["id"].ToString());
            Url = response["url"].ToString();
            //TODO: Groups, Bots and IMs.
            return true;
        }

        public bool Connect()
        {
            if (String.IsNullOrEmpty(Url))
                throw new InvalidOperationException("Call Slack.Start() first.");
            webSocket = new WebSocket(Url);
            webSocket.OnMessage += webSocket_OnMessage;
            webSocket.Connect();
            return true;
        }

        void webSocket_OnMessage(object sender, MessageEventArgs e)
        {
            
        }

    }
}
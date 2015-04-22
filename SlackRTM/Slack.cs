using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackRTM.Events;
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
        public List<Channel> Channels { get; private set; }

        public IEnumerable<Channel> JoinedChannels
        {
            get
            {
                return Channels.Where(n => n.IsMember);
            }
        }

        public bool Connected { get { return webSocket.IsAlive; } }

        public bool Connecting { get { return !RecievedHello && webSocket != null; } }

        public List<Im> Ims { get; private set; }

        public Channel PrimaryChannel { get; private set; }
        public bool RecievedHello { get; private set; }

        public User Self { get; private set; }

        public TeamInfo TeamInfo { get; private set; }

        public List<User> Users { get; private set; }

        private int sendId = 0;
        private List<Event> SentMessages = new List<Event>();
        internal string token;
        private string Url;
        private WebClient wc = new WebClient();

        private WebSocket webSocket;
        public Slack()
        {
            OnAck += Slack_OnAck;
            JsonConverter = new SlackJsonConverter(this);
        }

        public Slack(string token)
        {
            OnAck += Slack_OnAck;
            this.token = token;
            JsonConverter = new SlackJsonConverter(this);
        }

        public event OnEventEvent OnAck;

        public event OnEventEvent OnEvent;
        
        private SlackJsonConverter JsonConverter;

        public delegate void OnEventEvent(object sender, SlackEventArgs e);

        public bool Connect()
        {
            if (String.IsNullOrEmpty(Url))
            {
                if (!string.IsNullOrEmpty(this.token))
                    Init();
                else
                    throw new InvalidOperationException("Call Slack.Init() first.");
            }
            if (webSocket != null)
                webSocket.Close();
            webSocket = new WebSocket(Url);
            webSocket.OnMessage += webSocket_OnMessage;
            webSocket.Connect();
            Url = null;
            return true;
        }

        public Channel GetChannel(string p)
        {
            if (string.IsNullOrEmpty(p) || p == "#" || p == "@")
                return null;
            if (p[0] == '@')
                return Ims.FirstOrDefault(c => c.Id == p || c.Name == p);
            if (p[0] == '#')
                p = p.Substring(1);
            return Channels.FirstOrDefault(c => c.Id == p || c.Name == p);
        }

        public User GetUser(string p)
        {
            if (string.IsNullOrEmpty(p) || p == "@")
                return null;
            if (p[0] == '@')
                p = p.Substring(1);
            return Users.FirstOrDefault(c => c.Id == p || c.Name == p);
        }

        [Obsolete("Pass the token into the constructor instead.")]
        public bool Init(string token)
        {
            this.token = token;
            return Init();
        }

        public bool Init()
        {
            JObject response = Api("rtm.start");
            if (response["ok"].Value<bool>() == false)
            {
                Console.WriteLine(response["error"]);
                return false;
            }
            TeamInfo = JsonConvert.DeserializeObject<TeamInfo>(response["team"].ToString(), JsonConverter);
            Channels = JsonConvert.DeserializeObject<List<Channel>>(response["channels"].ToString(), JsonConverter);
            Channels.AddRange(JsonConvert.DeserializeObject<List<Channel>>(response["groups"].ToString(), JsonConverter)); // Groups are glorified channels?
            Ims = JsonConvert.DeserializeObject<List<Im>>(response["ims"].ToString(), JsonConverter);  // They're also channels, but with 'User's instead of 'Name's.

            Users = JsonConvert.DeserializeObject<List<User>>(response["users"].ToString(), JsonConverter);
            Self = Users.First(n => n.Id == response["self"]["id"].ToString());
            Url = response["url"].ToString();
            PrimaryChannel = Channels.FirstOrDefault(n => n.IsGeneral);
            //TODO: Groups, Bots and IMs.
            return true;
        }

        public void SendMessage(string channel, string text, params object[] args)
        {
            Message message;
            var chan = GetChannel(channel);
            text = String.Format(text, args);
            if (chan == null)
                message = new Message(channel, text, sendId++); // The user might know what they're doing more than we do, so attempt it with the ID they provided.  Initally a fix for broken IM support.
            else
            {
                //if (!chan.IsMember)
                //    throw new NotInChannelException();
                message = new Message(chan.Id, text, sendId++);
            }
            SentMessages.Add(message);
            webSocket.Send(message.ToJson());
        }

        private void Slack_OnAck(object sender, SlackEventArgs e)
        {
            var ack = (e.Data as Ack);
        }
        private void webSocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (sender != webSocket)
                return;
            var data = Event.NewEvent(e.Data);
            if (data.Type == "hello")
                this.RecievedHello = true;
            if (data is Ack)
                this.OnAck(this, new SlackEventArgs(data));
            else if (this.OnEvent != null)
                this.OnEvent(this, new SlackEventArgs(data));
        }

        internal JObject Api(string method, params JProperty[] args)
        {
            var uri = new UriBuilder("https://slack.com/api/" + method);
            
            var query = "token=" + token;
            foreach (var arg in args)
            {
                query += string.Format("&{0}={1}", arg.Name, arg.Value);
            }
            uri.Query = query;
            return JObject.Parse(wc.DownloadString(uri.Uri));
        }
    }
}
using System;
using Newtonsoft.Json.Linq;

namespace SlackRTM
{
    public class Channel
    {
	 	public enum ChannelType {Channel, Group, Im};

        public string Id { get; set; }

        public bool IsArchived { get; set; }

        public bool IsChannel { get; set; }

        public bool IsGeneral { get; set; }

        public bool IsMember { get; set; }

        public string Name { get; set; }

		public virtual ChannelType Type {  get { return ChannelType.Channel;} }

        public Slack SlackInstance { get; internal set; }

        /// <summary>
        /// Join the channel.
        /// </summary>
        /// <remarks>Not valid for Bots.</remarks>
        /// <returns></returns>
        public virtual bool Join()
        {
            var resp = SlackInstance.Api("channels.join", new JProperty("name", Name));
            var ok=resp.Value<bool>("ok");
            if (ok)
            {
                IsMember = true;
            }
            else 
                throw new Exception(resp.Value<string>("error"));
            // TODO: Switch on error - Some are valid to just return false [Such as 'Already in Channel']
            return ok;
        }

        public virtual bool Leave()
        {
            var resp = SlackInstance.Api("channels.leave", new JProperty("name", Name));
            var ok = resp.Value<bool>("ok");
            if (ok)
            {
                IsMember = false;
            }
            else
                throw new Exception(resp.Value<string>("error"));
            // TODO: Switch on error - Some are valid to just return false [Such as 'Already in Channel']

            return ok;
        }
    }

	public class Group : Channel
	{
		public override ChannelType Type
		{
			get
			{
				return ChannelType.Group;
			}
		}
	}

	public class Im : Channel
	{
		public override ChannelType Type
		{
			get
			{
				return ChannelType.Im;
			}
		}

        public override bool Join()
        {
            throw new InvalidOperationException();
        }

        public string User
		{
			get { return Name; }
			set { Name = value; }
		}
	}
}
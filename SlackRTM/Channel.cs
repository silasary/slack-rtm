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

        private Slack slack;

        public virtual bool Join()
        {

            return false;
        }

        public virtual bool Leave()
        {
            return false;
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
		public string User
		{
			get { return Name; }
			set { Name = value; }
		}
	}
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM
{
    public class Channel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsChannel { get; set; }

        public bool IsArchived { get; set; }

        public bool IsMember { get; set; }

        public bool IsGeneral { get; set; }

        public bool IsChannel { get; set; }


    }
}

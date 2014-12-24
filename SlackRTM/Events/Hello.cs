using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM.Events
{
    class Hello : Event
    {

        public override string Type
        {
            get { return "hello"; }
        }
        public override string ToString()
        {
            return "hello";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackRTM
{
    public static class Ext
    {
        public static string ToUnderscoreLower(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in str)
            {
                if (char.IsUpper(c))
                {
                    if (sb.Length > 0)
                        sb.Append("_");
                    sb.Append(char.ToLower(c));
                }
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
        public static string FromUnderscoreLower(this string str)
        {
            StringBuilder sb = new StringBuilder();
            bool NextUpper = true;
            foreach (var c in str)
            {
                if (c == '_')
                    NextUpper = true;
                else if (NextUpper)
                {
                    sb.Append(char.ToUpper(c));
                    NextUpper = false;
                }
                else
                    sb.Append(c);

            }
            return sb.ToString();

        }
        
    }
}
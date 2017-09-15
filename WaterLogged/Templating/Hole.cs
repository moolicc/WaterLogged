using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class Hole
    {
        public HoleId Id { get; set; }
        public HolePrefix Prefix { get; set; }
        public string Suffix { get; set; }
        public int HoleIndex { get; set; }
        public int TemplateStartIndex { get; set; }
        public int TemplateEndIndex { get; set; }


        public Hole()
            : this("", HolePrefix.None, "")
        {
        }

        public Hole(string id)
            : this(id, HolePrefix.None, "")
        {
        }

        public Hole(string id, HolePrefix prefix)
            : this(id, prefix, "")
        {
        }

        public Hole(string id, HolePrefix prefix, string suffix)
        {
            Id = new HoleId(id);
            Prefix = prefix;
            Suffix = suffix;
        }

        public Hole(int id)
            : this(id, HolePrefix.None, "")
        {
        }

        public Hole(int id, HolePrefix prefix)
            : this(id, prefix, "")
        {
        }

        public Hole(int id, HolePrefix prefix, string suffix)
        {
            Id = new HoleId(id);
            Prefix = prefix;
            Suffix = suffix;
        }
        
        public Hole(HoleId id, HolePrefix prefix, string suffix)
        {
            Id = id;
            Prefix = prefix;
            Suffix = suffix;
        }


        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('{');
            if (Prefix == HolePrefix.Destructuring)
            {
                builder.Append('@');
            }
            else if (Prefix == HolePrefix.Stringification)
            {
                builder.Append('$');
            }
            builder.Append(Id);
            if (!string.IsNullOrWhiteSpace(Suffix))
            {
                builder.Append(':');
                builder.Append(Suffix);
            }
            builder.Append('}');
            return builder.ToString();
        }
    }
}

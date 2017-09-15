using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace WaterLogged.Templating
{
    public static class Extensions
    {
        public static StringBuilder AppendTemplateHole(this StringBuilder builder, HoleId id)
        {
            return AppendTemplateHole(builder, id, HolePrefix.None, "");
        }

        public static StringBuilder AppendTemplateHole(this StringBuilder builder, HoleId id, HolePrefix prefix)
        {
            return AppendTemplateHole(builder, id, prefix, "");
        }
        
        public static StringBuilder AppendTemplateHole(this StringBuilder builder, HoleId id, HolePrefix prefix, string suffix)
        {
            return AppendTemplateHole(builder, new Hole(id, prefix, suffix));
        }

        public static StringBuilder AppendTemplateHole(this StringBuilder builder, Hole hole)
        {
            return builder.Append(hole.ToString());
        }
    }
}

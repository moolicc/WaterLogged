using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace WaterLogged.Templating
{
    public class HoleId
    {
        public string NamedId { get; private set; }
        public int PositionalId { get; private set; }
        public HoleIdTypes IdType { get; private set; }

        public HoleId(string namedId)
        {
            NamedId = namedId;
            PositionalId = -1;
            IdType = HoleIdTypes.Named;
        }

        public HoleId(int positionalId)
        {
            NamedId = positionalId.ToString();
            PositionalId = positionalId;
            IdType = HoleIdTypes.Positional;
        }

        public override string ToString()
        {
            if (IdType == HoleIdTypes.Named)
            {
                return NamedId;
            }
            return PositionalId.ToString();
        }

        public static explicit operator string(HoleId holeId)
        {
            return holeId.NamedId;
        }

        public static explicit operator int(HoleId holeId)
        {
            return holeId.PositionalId;
        }

        public static implicit operator HoleId(string namedId)
        {
            return new HoleId(namedId);
        }

        public static implicit operator HoleId(int positionalId)
        {
            return new HoleId(positionalId);
        }
    }
}

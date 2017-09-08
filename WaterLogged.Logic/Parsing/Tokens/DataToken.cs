using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing.Tokens
{
    public abstract class DataToken : Token
    {
        public abstract object GetValue();
    }
}

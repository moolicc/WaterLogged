using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class StandardOutListener : Listener
    {
        public override void Write(string value, string tag)
        {
            Console.Write(value);
        }
    }
}

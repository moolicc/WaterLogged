using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Listeners
{
    public class FileOut : Listener
    {
        public string FilePath { get; private set; }

        public FileOut(string filePath)
        {
            FilePath = filePath;
        }

        public override void Write(string value, string tag)
        {
            System.IO.File.AppendAllText(FilePath, value);
        }
    }
}

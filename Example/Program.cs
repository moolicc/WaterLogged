using System;
using WaterLogged;
using WaterLogged.Serialization;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log("mylog");
            log.AddListener(new StandardOutListener());
            
            BasicFormatter form = new BasicFormatter();
            form.Format = "${tag} ${message} ${datetime}";
            log.Formatter = form;

            log.WriteLineTag("test", "DEBUG");

            Console.ReadLine();
        }
    }
}
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
            log.Formatter = form;

            log.WriteLineTag("test", "DEBUG");

            Serializer s= new Serializer();
            s.Logs.Add("mylog", log);
            
            Console.WriteLine(s.SaveJson());
            Console.ReadLine();
        }
    }
}
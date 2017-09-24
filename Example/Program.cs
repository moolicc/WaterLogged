using System;
using System.Collections.Generic;
using Example.Examples;

namespace Example
{
    class Program
    {
        private static List<ExampleBase> _examples;
        private static int _selectedExample;

        static void Main(string[] args)
        {
            _examples = new List<ExampleBase>();
            _examples.Add(new Example1());
            _examples.Add(new Example2());
            _examples.Add(new Example3());
            _examples.Add(new Example4());
            _examples.Add(new Example5());
            _examples.Add(new Example6());
            _examples.Add(new Example7());
            _examples.Add(new Example8());
            _examples.Add(new Example9());

            _selectedExample = -1;
            SelectExample();

            Console.WriteLine("Press the ANY key to continue...");
            Console.ReadLine();
        }

        private static void SelectExample()
        {
            Console.Clear();
            Console.WriteLine("Select an example by index. Or type 'quit' to end.");

            for (var i = 0; i < _examples.Count; i++)
            {
                var example = _examples[i];
                Console.WriteLine("{0}  --  {1}", i, example.Name);
            }

            var input = Console.ReadLine().Trim();
            if (input == "quit")
            {
                return;
            }
            var parsed = int.TryParse(input, out var index);

            if (!parsed || index < 0 || index >= _examples.Count)
            {
                SelectExample();
                return;
            }

            _selectedExample = index;
            RunExample();
        }

        private static void RunExample()
        {
            Console.WriteLine("Type 'end' to select a new example.");
            Console.WriteLine(_examples[_selectedExample].IntroText);
            _examples[_selectedExample].Selected();

            while (true)
            {
                string input = Console.ReadLine().Trim();
                if (input == "end")
                {
                    break;
                }
                _examples[_selectedExample].Echo(input);
            }

            _selectedExample = -1;
            SelectExample();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCMLink
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <1 || args[0].StartsWith("-h"))
            {
                Console.WriteLine("[Help Info]");
                foreach (string arg in args)
                {
                    Console.WriteLine(arg);
                    Console.ReadLine();
                }
            }
            else
            {
                foreach (string arg in args)
                {
                    Console.WriteLine(arg);
                    Console.ReadLine();
                }
            }
        }
    }
}

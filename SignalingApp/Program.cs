using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Write("Deneme");
            Console.Title = "Hello World";
            Console.WriteLine("This process has access to the entire public desktop API surface");
            Console.WriteLine("Press any key to exit ...");
            Console.ReadLine();
        }
    }
}

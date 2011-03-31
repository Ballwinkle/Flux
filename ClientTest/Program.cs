using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flux;
using Flux.Client;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine fluxEngine = new Engine();
            fluxEngine.StartFluxEngine();
            Console.Write("USER: ");
            string username = Console.ReadLine();
            Console.Write("PASS: ");
            string password = Console.ReadLine();
            Console.Write("LastError: " + fluxEngine.Login(username, password).LastError);
        }
    }
}

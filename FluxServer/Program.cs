using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flux;

namespace Flux.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initialize("server.log", LogLevel.All, true);
            new DB();
            GAS.StartServer(29301);
            WatchMan.Start();
        }
    }
}

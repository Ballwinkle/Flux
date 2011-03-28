using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Flux.Server
{
    public static class WatchMan
    {
        public static void Start() 
        {
            new Thread(InternalIO).Start();
        }

        private static void InternalIO() 
        {
            Log.Info("Starting WatchMan...");
            while (true)
            {
                System.Threading.Thread.Sleep(60000);
                TicketCleanup();
                SocketCleanup();
            }
        }

        private static void SocketCleanup()
        {
            try
            {
                foreach (GAS user in GAS.Users)
                {
                    if ((DateTime.Now - user.LastTouched).Minutes < 3)
                        GAS.Users.Remove(user);
                }
                Log.Info("Socket cleanup complete");
            }
            catch 
            {
                Log.Error("Socket cleanup failed");
            }
        }

        private static void TicketCleanup() 
        {
            try
            {
                lock (Tickets.Lock)
                {
                    foreach (Ticket t in Tickets.TicketsList)
                    {
                        if ((DateTime.Now - t.LastTouched).Minutes < 3)
                            Tickets.TicketsList.Remove(t);
                    }
                    Log.Info("Ticket cleanup complete");
                }
            }
            catch 
            {
                Log.Error("Ticket cleanup failed");
            }
        }
    }
}

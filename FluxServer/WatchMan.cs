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
                System.Threading.Thread.Sleep(6000);
                //TicketCleanup(); //TODO Fix this
                SocketCleanup();
            }
        }

        private static void SocketCleanup()
        {
            try
            {
                List<GAS> SocketsToDelete = new List<GAS>();
                foreach (GAS user in GAS.Users)
                {
                    if ((DateTime.Now - user.LastTouched).Minutes > 3)
                        SocketsToDelete.Add(user);
                }
                foreach (GAS user in SocketsToDelete)
                {
                    GAS.Users.Remove(user);
                }
                if (SocketsToDelete.Count > 0)
                    Log.Info(SocketsToDelete.Count + " socket reference(s) deleted");
                SocketsToDelete.Clear();
            }
            catch (Exception e)
            {
                Log.Error("Socket cleanup failed: " + e.Message);
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

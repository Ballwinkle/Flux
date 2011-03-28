using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Server
{
    public static class Tickets
    {
        public static List<Ticket> TicketsList = new List<Ticket>();

        public static object Lock = "";

        public static bool ValidateTicket(uint FluxID, ushort AppID, ushort AccessLevel)
        {
            lock (Lock)
            {
                foreach (Ticket t in TicketsList)
                {
                    if ((t.AppID == AppID) && (t.FluxID == FluxID) && (t.AccessLevel >= AccessLevel) && (DateTime.Now - t.LastTouched).Minutes < 3)
                    {
                        t.LastTouched = DateTime.Now;
                        return true;
                    }
                }
                return false;
            }
        }

        public static uint CreateTicket(uint FluxID, ushort AppID)
        {
            //TODO Validate Player
            if (true)
            {
                Ticket t = new Ticket();
                t.FluxID = FluxID;
                t.AppID = AppID;
                t.AccessLevel = 1; //TODO Get Access Level from DB
                t.LastTouched = DateTime.Now;
                return 1; //TODO Access Level again
            }
            else
                return 0;
        }
    }
}

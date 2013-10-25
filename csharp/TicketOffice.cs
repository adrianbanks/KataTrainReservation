using System;
using System.Collections.Generic;

namespace KataTrainReservation
{
    public class TicketOffice
    {
        public Reservation MakeReservation(ReservationRequest request)
        {
            return new Reservation("","", new List<Seat>());
        }
    }
}

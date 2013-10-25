using System;
using System.Collections.Generic;

namespace KataTrainReservation
{
    public class TicketOffice
    {
        public TicketOffice(IAvailableSeatsService mockAvailableSeatsService)
        {
            
        }

        public Reservation MakeReservation(ReservationRequest request)
        {
            return new Reservation("","", new List<Seat>());
        }
    }
}

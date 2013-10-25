using System.Collections.Generic;

namespace KataTrainReservation
{
    public interface IAvailableSeatsService
    {
        IList<Seat> GetUnreservedSeats(string trainId);
    }
}
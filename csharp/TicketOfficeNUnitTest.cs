using NUnit.Framework;

namespace KataTrainReservation
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        [Test]
        public void MakeReservation_GivesReservationWithEmptyBookingID_WhenTheTrainIsFull()
        {
            var ticketOffice = new TicketOffice();
            var reservationRequest = new ReservationRequest("express_2013", 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }


    }
}

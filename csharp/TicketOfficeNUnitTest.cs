using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace KataTrainReservation
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        [Test]
        public void MakeReservation_GivesReservationWithEmptyBookingID_WhenTheTrainIsFull()
        {
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats("express_2013").ReturnsForAnyArgs(new List<string>());
            var ticketOffice = new TicketOffice(mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest("express_2013", 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }

        [Test]
        public void MakeReservation_GivesReservationWithNonEmptyBookingID_WhenTheTrainIsNotFull_AndASingleSeatIsAskedFor()
        {
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats("express_2013").ReturnsForAnyArgs(new List<string>{"1A"});
            var ticketOffice = new TicketOffice(mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest("express_2013", 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Not.Empty);
        }
    }

    public interface IAvailableSeatsService
    {
        IList<string> GetUnreservedSeats(string trainId);
    }
}

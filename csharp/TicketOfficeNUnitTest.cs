using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace KataTrainReservation
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        private const string trainId = "express_2013";

        const string bookingId = "1234";

        private static IBookingIdService MockBookingIdService(string bookingId)
        {
            var mockBookingIdService = Substitute.For<IBookingIdService>();
            mockBookingIdService.GetNextId().Returns(bookingId);
            return mockBookingIdService;
        }

        [Test]
        public void MakeReservation_GivesReservationWithEmptyBookingID_WhenTheTrainIsFull()
        {
            var mockBookingIdService = Substitute.For<IBookingIdService>();
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat>());
            var ticketOffice = new TicketOffice(mockBookingIdService, mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }

        [Test]
        public void MakeReservation_GivesReservationWithNonEmptyBookingID_WhenTheTrainIsNotFull_AndASingleSeatIsAskedFor()
        {

            var mockBookingIdService = MockBookingIdService(bookingId);
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat> {new Seat("A", 1)});
            var ticketOffice = new TicketOffice(mockBookingIdService, mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Not.Empty);
        }

        [Test]
        public void MakeReservation_GivesTheSeat_WhenOnlyOneSeatIsAvailable()
        {
            var mockBookingIdService = Substitute.For<IBookingIdService>();
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            var availableSeat = new Seat("A", 1);
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat> {availableSeat});
            var ticketOffice = new TicketOffice(mockBookingIdService, mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 1);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            var bookedSeat = reservation.Seats.Single();
            Assert.That(bookedSeat, Is.EqualTo(availableSeat));
        }

        [Test]
        public void MakeReservation_GivesTwoSeats_WhenMoreThanTwoSeatsAreAvailable()
        {
            var mockBookingIdService = Substitute.For<IBookingIdService>();
            var mockAvailableSeatsService = Substitute.For<IAvailableSeatsService>();
            var availableSeat1 = new Seat("A", 1);
            var availableSeat2 = new Seat("A", 2);
            var availableSeat3 = new Seat("A", 3);
            mockAvailableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(
                new List<Seat> {availableSeat1, availableSeat2, availableSeat3});
            var ticketOffice = new TicketOffice(mockBookingIdService, mockAvailableSeatsService);
            var reservationRequest = new ReservationRequest(trainId, 2);

            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.Seats.Count(), Is.EqualTo(2));
        }

        [Test]
        public void MakeReservation_GetsTheBookingIdFromTheBookingService_WhenASuccessfulBookingIsMade()
        {
            var mockBookingIdService = MockBookingIdService(bookingId);
            var availableSeat1 = new Seat("A", 1);
            var availableSeatsService = Substitute.For<IAvailableSeatsService>();
            availableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(new List<Seat> {availableSeat1});
            var ticketOffice = new TicketOffice(mockBookingIdService, availableSeatsService);

            var reservationRequest = new ReservationRequest(trainId, 1);
            
            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.EqualTo(bookingId));
        }

        [Test]
        public void MakeReservation_GivesEmptyReservation_WhenThereAreNotEnoughSeatsInACoach()
        {
            var mockBookingIdService = MockBookingIdService(bookingId);
            var availableSeat1 = new Seat("A", 1);
            var availableSeat2 = new Seat("A", 2);
            var availableSeat3 = new Seat("B", 1);
            var availableSeat4 = new Seat("B", 2);
            var availableSeatsService = Substitute.For<IAvailableSeatsService>();
            availableSeatsService.GetUnreservedSeats(trainId).ReturnsForAnyArgs(
                new List<Seat> {availableSeat1,availableSeat2,availableSeat3,availableSeat4});
            var ticketOffice = new TicketOffice(mockBookingIdService, availableSeatsService);

            var reservationRequest = new ReservationRequest(trainId, 4);
            
            var reservation = ticketOffice.MakeReservation(reservationRequest);

            Assert.That(reservation.BookingId, Is.Empty);
        }
    }

    public interface IBookingIdService
    {
        string GetNextId();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tucson.Application.Service;
using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;
using Xunit;

namespace Tucson.Tests.Application.Service
{
    public class ReservationServiceTest
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly ReservationService _reservationService;

        public ReservationServiceTest()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _reservationService = new ReservationService(_reservationRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldCreateReservation_WhenTableIsAvailable()
        {
            // Arrange
            var customer = new Customer(1, "Brisa", MembershipCategoryEnum.Gold);
            var reservationDate = DateTime.Today.AddDays(1);
            var requestedSeatCount = 4;

            var availableTable = new Table(1, requestedSeatCount); 
            _reservationRepositoryMock.Setup(r => r.GetAllTables())
                .Returns(new List<Table> { availableTable });
            _reservationRepositoryMock.Setup(r => r.IsTableReservedForDate(availableTable.Id, reservationDate))
                .Returns(false);
            _reservationRepositoryMock.Setup(r => r.GetAllReservations())
                .Returns(new List<Reservation>());

            // Act
            var reservation = await _reservationService.CreateReservationAsync(customer, reservationDate, requestedSeatCount);

            // Assert
            reservation.Should().NotBeNull();
            reservation.TableId.Should().Be(availableTable.Id);
            reservation.ReservationDate.Should().Be(reservationDate);
            _reservationRepositoryMock.Verify(r => r.AddReservation(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldAddToWaitingList_WhenNoTableIsAvailable()
        {
            // Arrange
            var customer = new Customer(1, "Brisa", MembershipCategoryEnum.Gold);
            var reservationDate = DateTime.Today.AddDays(1);
            var requestedSeatCount = 4;

            _reservationRepositoryMock.Setup(r => r.GetAllTables())
                .Returns(new List<Table>());

            // Act
            Func<Task> act = async () => await _reservationService.CreateReservationAsync(customer, reservationDate, requestedSeatCount);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No available table. Customer added to the waiting list.");
            _reservationRepositoryMock.Verify(r => r.AddToWaitingList(customer, reservationDate, requestedSeatCount), Times.Once);
        }

        [Fact]
        public async Task CancelReservationAsync_ShouldRemoveReservationAndHandleWaitingList()
        {
            // Arrange
            var reservationId = 1;
            var reservation = new Reservation(reservationId, new Customer(1, "Brisa", MembershipCategoryEnum.Gold), 1, DateTime.Today);
            var waitingCustomer = new WaitingList(customer: new Customer(2, "John", MembershipCategoryEnum.Classic), reservationDate: DateTime.Today.AddDays(1), seatCount: 4);
            var table = new Table(1, 4);
            table.Reserve();

            _reservationRepositoryMock.Setup(r => r.GetAllReservations())
                .Returns(new List<Reservation> { reservation });
            _reservationRepositoryMock.Setup(r => r.GetWaitingList())
                .Returns(new List<WaitingList> { waitingCustomer });
            _reservationRepositoryMock.Setup(r => r.GetAllTables())
                .Returns(new List<Table> { new Table(2, waitingCustomer.SeatCount) }); // Ensure available tables
            _reservationRepositoryMock.Setup(r => r.GetTableById(reservation.TableId))
                .Returns(table);
            _reservationRepositoryMock.Setup(r => r.RemoveReservation(It.IsAny<Reservation>()));
            _reservationRepositoryMock.Setup(r => r.RemoveFromWaitingList(It.IsAny<Customer>(), It.IsAny<DateTime>()));

            // Act
            Func<Task> act = async () => await _reservationService.CancelReservationAsync(reservationId);

            // Assert
            await act.Should().NotThrowAsync();
            _reservationRepositoryMock.Verify(r => r.RemoveReservation(reservation), Times.Once);
            _reservationRepositoryMock.Verify(r => r.RemoveFromWaitingList(waitingCustomer.Customer, waitingCustomer.ReservationDate), Times.Once);
        }


        [Fact]
        public async Task CancelReservationAsync_ShouldThrowException_WhenReservationNotFound()
        {
            // Arrange
            var reservationId = 999;

            _reservationRepositoryMock.Setup(r => r.GetAllReservations())
                .Returns(new List<Reservation>());

            // Act
            Func<Task> act = async () => await _reservationService.CancelReservationAsync(reservationId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Reservation not found.");
        }
    }
}

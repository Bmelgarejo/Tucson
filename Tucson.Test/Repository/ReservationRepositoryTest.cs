using System;
using System.Linq;
using FluentAssertions;
using Tucson.Domain.Entities;
using Tucson.Domain.ValueObject;
using Tucson.Infrastructure.Repositories;
using Xunit;

namespace Tucson.Tests.Infrastructure.Repositories
{
    public class ReservationRepositoryTest
    {
        private readonly ReservationRepository _repository;

        public ReservationRepositoryTest()
        {
            _repository = new ReservationRepository();
        }

        [Fact]
        public void AddReservation_ShouldAddReservationSuccessfully()
        {
            // Arrange
            var customer = new Customer(1, "Brisa", MembershipCategoryEnum.Gold);
            var reservation = new Reservation(1, customer, 1, DateTime.Today);

            // Act
            _repository.AddReservation(reservation);
            var allReservations = _repository.GetAllReservations();

            // Assert
            allReservations.Should().Contain(reservation);
        }

        [Fact]
        public void RemoveReservation_ShouldRemoveReservationSuccessfully()
        {
            // Arrange
            var customer = new Customer(2, "Brisa", MembershipCategoryEnum.Gold);
            var reservation = new Reservation(2, customer, 2, DateTime.Today);
            _repository.AddReservation(reservation);

            // Act
            _repository.RemoveReservation(reservation);
            var allReservations = _repository.GetAllReservations();

            // Assert
            allReservations.Should().NotContain(reservation);
        }

        [Fact]
        public void GetReservationsByDate_ShouldReturnReservationsForSpecificDate()
        {
            // Arrange
            var date = new DateTime(2025, 4, 29);
            var customer = new Customer(3, "Brisa", MembershipCategoryEnum.Classic);
            var reservation = new Reservation(3, customer, 3, date);
            _repository.AddReservation(reservation);

            // Act
            var result = _repository.GetReservationsByDate(date);

            // Assert
            result.Should().ContainSingle(r => r.ReservationDate.Date == date.Date);
        }

        [Fact]
        public void AddAndRemoveFromWaitingList_ShouldHandleWaitingListCorrectly()
        {
            // Arrange
            var customer = new Customer(5, "Brisa", MembershipCategoryEnum.Gold);
            var date = DateTime.Today;
            var seats = 2;

            // Act
            _repository.AddToWaitingList(customer, date, seats);
            var waitingList = _repository.GetWaitingList();

            // Assert
            waitingList.Should().ContainSingle(w => w.Customer.Id == customer.Id);

            // Act
            _repository.RemoveFromWaitingList(customer, date);
            waitingList = _repository.GetWaitingList();

            // Assert
            waitingList.Should().NotContain(w => w.Customer.Id == customer.Id);
        }

        [Fact]
        public void GetTableById_ShouldReturnCorrectTable()
        {
            // Act
            var table = _repository.GetTableById(1);

            // Assert
            table.Should().NotBeNull();
            table.Id.Should().Be(1);
        }

        [Fact]
        public void GetCustomerById_ShouldReturnCorrectCustomer()
        {
            // Act
            var customer = _repository.GetCustomerById(1);

            // Assert
            customer.Should().NotBeNull();
            customer.Id.Should().Be(1);
        }

        [Fact]
        public void IsTableReservedForDate_ShouldReturnTrueIfTableIsReserved()
        {
            // Arrange
            var customer = new Customer(6, "Brisa", MembershipCategoryEnum.Gold);
            var tableId = 2;
            var date = DateTime.Today;
            var reservation = new Reservation(5, customer, tableId, date);
            _repository.AddReservation(reservation);

            // Act
            var isReserved = _repository.IsTableReservedForDate(tableId, date);

            // Assert
            isReserved.Should().BeTrue();
        }

        [Fact]
        public void IsTableReservedForDate_ShouldReturnFalseIfTableIsNotReserved()
        {
            // Act
            var isReserved = _repository.IsTableReservedForDate(999, DateTime.Today);

            // Assert
            isReserved.Should().BeFalse();
        }
    }
}

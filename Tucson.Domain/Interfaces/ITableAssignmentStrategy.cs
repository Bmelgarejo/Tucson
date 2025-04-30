using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Entities;

namespace Tucson.Domain.Interfaces
{
    public interface ITableAssignmentStrategy
    {
        Table AssignTable(List<Table> availableTables, List<Reservation> reservations, DateTime reservationDate);
    }
}

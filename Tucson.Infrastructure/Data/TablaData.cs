using System;
using System.Collections.Generic;
using Tucson.Domain.Entities;

namespace Tucson.Infrastructure.Data
{
    public class TableData
    {
        private static readonly Lazy<TableData> _instance =
            new Lazy<TableData>(() => new TableData());

        public static TableData Instance => _instance.Value;

        public List<Table> Tables { get; private set; }

        private TableData()
        {
            Tables = new List<Table>();

            for (int i = 1; i <= 2; i++)
            {
                Tables.Add(new Table(i, 2));
            }

            for (int i = 1; i <= 4; i++)
            {
                Tables.Add(new Table(i, 4));
            }

            for (int i = 1; i <= 2; i++)
            {
                Tables.Add(new Table(i, 6));
            }
        }
    }
}

using System.Collections.Generic;

namespace TddDemo
{
    public class Row
    {
        public List<Cell> Cells { get; }

        public Row(List<Cell> cells)
        {
            Cells = cells;
        }
    }
}
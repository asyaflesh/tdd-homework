using System.Collections.Generic;
using TddDemo;

namespace TddDemo
{
    public class CsvFile
    {
        public List<Row> Rows { get; set; }
        public bool IsProcessed { get; set; }
        
        public static bool CsvValidRow(Row x)
        {
            return x.Cells.Count == 4;
        }
    }
}
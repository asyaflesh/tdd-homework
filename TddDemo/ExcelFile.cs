using System.Collections.Generic;

namespace TddDemo
{
    public class ExcelFile
    {
        public List<Row> Rows { get; set; }
        public bool IsProcessed { get; set; }
        
        public static bool ExcelValidRow(Row x)
        {
            return x.Cells.Count == 3;
        }
    }
}
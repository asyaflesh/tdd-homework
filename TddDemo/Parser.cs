using System.Linq;

namespace TddDemo
{
    public class Parser
    {
        private readonly IAlertPublisher _alertPublisher;
        private readonly IStorage _storage;

        public Parser(IAlertPublisher alertPublisher, IStorage storage)
        {
            _alertPublisher = alertPublisher;
            _storage = storage;
        }

        public int ParseExcel(ExcelFile excelFile)
        {
            for (var i = 0; i < excelFile.Rows.Count(x => ExcelFile.ExcelValidRow(x) == false); i++)
                _alertPublisher.SendAlert();

            if (excelFile.Rows.Count(x => ExcelFile.ExcelValidRow(x) == true) == excelFile.Rows.Count())
            {
                excelFile.IsProcessed = true;
                _storage.SaveExcel(excelFile);
            }
            return excelFile.Rows.Count(x => ExcelFile.ExcelValidRow(x) == true);
        }
        
        public int ParseCvs(CsvFile csvFile)
        {
            for (var i = 0; i < csvFile.Rows.Count(x => CsvFile.CsvValidRow(x) == false); i++)
                _alertPublisher.SendAlert();;

            if (csvFile.Rows.Count(x => CsvFile.CsvValidRow(x) == true) == csvFile.Rows.Count())
            {
                csvFile.IsProcessed = true;
                _storage.SaveCsv(csvFile);
            }
            return csvFile.Rows.Count(x => CsvFile.CsvValidRow(x) == true);
        }
    }
}
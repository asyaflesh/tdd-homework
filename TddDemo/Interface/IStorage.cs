namespace TddDemo
{
    public interface IStorage
    {
        void SaveExcel(ExcelFile excelFile);
        
        void SaveCsv(CsvFile csvFile);
    }
}
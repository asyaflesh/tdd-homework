using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TddDemo;

namespace TddDemoTests
{
    public class ParserTests
    {
        private Mock<IAlertPublisher> _publisher;
        private Mock<IStorage> _storage;
        private Parser _parser;
        
        [SetUp]
        public void Setup()
        {
            _publisher = new Mock<IAlertPublisher>();
            _storage = new Mock<IStorage>();
            _parser = new Parser(_publisher.Object, _storage.Object);
        }

        [Test]
        public void ParseExcel_TwoRows_ReturnsRowsCount()
        {
            var parsedCount = _parser.ParseExcel(CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow()
            }));
            
            Assert.AreEqual(2, parsedCount);
        }
        
        [Test]
        public void ParseExcel_ThreeRows_ReturnsRowsCount()
        {
            var parsedCount = _parser.ParseExcel(CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow(),
                CreateValidExcelRow(),
            }));
            
            Assert.AreEqual(3, parsedCount);
        }

        [Test]
        public void ParseExcel_OneInvalidRow_SendAlert()
        {
            _parser.ParseExcel(CreateExcelFile(new List<Row>
            {
                new Row(new List<Cell>
                {
                    new Cell()
                })
            }));

            _publisher.Verify(x => x.SendAlert());
        }

        [Test]
        public void ParseExcel_RowsValid_DoesntSendAlert()
        {
            _parser.ParseExcel(CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow()
            }));

            _publisher.Verify(x => x.SendAlert(), Times.Never);
        }
        
        
        [Test]
        public void ParseExcel_AllRowsWereParsed_SetAsProcessed()
        {
            var excelFile = CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow(),
                CreateValidExcelRow()
            });

            _parser.ParseExcel(excelFile);
            Assert.AreEqual(true, excelFile.IsProcessed);
        }

        [Test]
        public void ParseExcel_OneRowsWereNotParsed_SetAsUnprocessed()
        {
            var excelFile = CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow(),
                new Row(new List<Cell>
                {
                    new Cell()
                })
            });

            _parser.ParseExcel(excelFile);
            Assert.AreEqual(false, excelFile.IsProcessed);
        }
        
        [Test]
        public void ParseExcel_AllRowsWereParsed_Save()
        {
            var excelFile = CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow(),
                CreateValidExcelRow()
            });
            _parser.ParseExcel(excelFile);

            _storage.Verify(x => x.SaveExcel(excelFile));
        }
        
        [Test]
        public void ParseExcel_OneRowsWereNotParsed_NotSave()
        {
            var excelFile = CreateExcelFile(new List<Row>
            {
                CreateValidExcelRow(),
                CreateValidExcelRow(),
                new Row(new List<Cell>
                {
                    new Cell()
                })
            });
            _parser.ParseExcel(excelFile);

            _storage.Verify(x => x.SaveExcel(excelFile), Times.Never);
        }
        
        [Test]
        public void ParseCvs_ThreeRows_ReturnsRowsCount()
        {
            var cvsFile = CreateCsvFile(new List<Row>
            {
                CreateValidCsvRow(),
                CreateValidCsvRow(),
                CreateValidCsvRow()
            });
            var count = _parser.ParseCvs(cvsFile);
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void ParseCsv_OneInvalidAndTwoValidRows_ReturnsTwoRowsCountAndSendAlert()
        {
            var cvsFile = CreateCsvFile(new List<Row>
            {
                CreateValidCsvRow(),
                CreateValidCsvRow(),
                new Row(new List<Cell>
                {
                    new Cell()
                })
            });
            var count = _parser.ParseCvs(cvsFile);
            
            Assert.AreEqual(2, count);
            _publisher.Verify(x => x.SendAlert());
        }
        
        private static ExcelFile CreateExcelFile(List<Row> rows)
        {
            return new ExcelFile()
            {
                Rows = rows
            };
        }
        
        private static CsvFile CreateCsvFile(List<Row> rows)
        {
            return new CsvFile()
            {
                Rows = rows
            };
        }
        
        private static Row CreateValidExcelRow()
        {
            return new Row(new List<Cell>
            {
                new Cell(),
                new Cell(),
                new Cell()
            });
        }
        
        private static Row CreateValidCsvRow()
        {
            return new Row(new List<Cell>
            {
                new Cell(),
                new Cell(),
                new Cell(),
                new Cell()
            });
        }
    }
}


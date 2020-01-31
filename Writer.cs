using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserFedresource
{
    public class Writer
    {
        Application _ObjExcel;
        Workbook _ObjWorkBook;
        Worksheet _ObjWorkSheet;

        int _startCell = 1;

        public int PageNum
        {
            get { return (int)(_ObjWorkSheet.Cells[1, 17]).Value; }
        }

        public string LastString
        {
            get { return _ObjWorkSheet.Cells[1, 18].ToString(); }
        }

        public Writer()
        {
            _ObjExcel = new Application();
            _ObjExcel.Visible = true;
            _ObjWorkBook = _ObjExcel.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Data.xlsx");

            _ObjWorkSheet = (Worksheet)_ObjWorkBook.Sheets[1];

            int counter = 2;
            
            while (true)
            {
                try
                {
                    var test = (int)(_ObjWorkSheet.Cells[counter++, 1]).Value;
                }
                catch
                {
                    try
                    {
                        _startCell = (int)_ObjWorkSheet.Cells[counter - 2, 1].Value + 1;
                        _ObjWorkSheet.Cells[counter - 1, 1] = _startCell;
                        break;
                    }
                    catch
                    {
                        _startCell = 2;
                        _ObjWorkSheet.Cells[2, 1] = _startCell - 1;
                        break;
                    }
                }
            }
        }

        

        public void Write(Info info, string str, int pageNum)
        {
            _ObjWorkSheet.Cells[_startCell, 1] = _startCell - 1;
            _ObjWorkSheet.Cells[_startCell, 2] = info.MessageNumber;
            _ObjWorkSheet.Cells[_startCell, 3] = info.Contract;
            _ObjWorkSheet.Cells[_startCell, 4] = info.Lessor;
            _ObjWorkSheet.Cells[_startCell, 5] = info.LessorINN;
            _ObjWorkSheet.Cells[_startCell, 6] = info.ContractNumber;
            _ObjWorkSheet.Cells[_startCell, 7] = info.RentalPeriod;
            _ObjWorkSheet.Cells[_startCell, 8] = info.Pledger;
            _ObjWorkSheet.Cells[_startCell, 9] = info.PledgerINN;
            _ObjWorkSheet.Cells[_startCell, 10] = info.Identifier;
            _ObjWorkSheet.Cells[_startCell, 11] = info.Classification;
            _ObjWorkSheet.Cells[_startCell, 12] = info.Description;
            _ObjWorkSheet.Cells[_startCell, 13] = info.LinkedMessages;
            _ObjWorkSheet.Cells[_startCell, 14] = info.File;
            _ObjWorkSheet.Cells[_startCell++, 15] = info.URL;

            _ObjWorkSheet.Cells[1, 17] = pageNum;
            _ObjWorkSheet.Cells[1, 18] = str;
        }

        public void Finish()
        {
            try
            {
                _ObjWorkBook.Close(SaveChanges: true);
                _ObjExcel.Quit();
            }
            catch
            {

            }
        }
    }
}

using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyChamThi.Utilities
{
    static class ExcelExporter
    {
        public static bool Export(List<List<string>> table, string filePath)
        {
            if(table == null)
                return false;

            try
            { 
                int NumberOfRows = table.Count;
                int NumberOfCols = NumberOfRows>0? table[0].Count : 0;

                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                Excel.Range rangeToHoldHyperlink;
                Excel.Range CellInstance;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlApp.DisplayAlerts = false;
                //Dummy initialisation to prevent errors.
                rangeToHoldHyperlink = xlWorkSheet.get_Range("A1", Type.Missing);
                CellInstance = xlWorkSheet.get_Range("A1", Type.Missing);

                for (int i = 0; i < NumberOfRows; i++)
                {
                    for (int j = 0; j < NumberOfCols; j++)
                    {
                        string cellContent = table[i][j];
                        xlWorkSheet.Cells[i + 1, j + 1] = cellContent;
                    }
                }

                xlWorkBook.SaveAs(filePath);
                xlWorkBook.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}

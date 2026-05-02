using CaloriePlanner.Model;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaloriePlanner.Services
{
    public class ExcelService
    {
        public void Export(User user, double bmr, double tdee, List<Fruit> fruits)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");

            ws.Cell(1, 1).Value = "Metric";
            ws.Cell(1, 2).Value = "Value";

            ws.Cell(2, 1).Value = "Age";
            ws.Cell(2, 2).Value = user.Age;

            ws.Cell(3, 1).Value = "BMR";
            ws.Cell(3, 2).Value = bmr;

            ws.Cell(4, 1).Value = "TDEE";
            ws.Cell(4, 2).Value = tdee;

            ws.Cell(6, 1).Value = "Fruit";
            ws.Cell(6, 2).Value = "Calories (per 100g)";

            int row = 7;
            foreach (var fruit in fruits)
            {
                ws.Cell(row, 1).Value = fruit.Name;
                ws.Cell(row, 2).Value = fruit.CaloriesPer100g;
                row++;
            }

            wb.SaveAs("CalorieReport.xlsx");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NPOI.HSSF.UserModel;
//using NPOI.XSSF.UserModel;
//using NPOI.SS.UserModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

using integraMobile.Infrastructure;
using integraMobile.Domain.Abstract;
using backOffice.Properties;
using backOffice.Models;

namespace backOffice.Controllers
{
    [HandleError]
    //[NoCache] 
    public class GridController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;

        public GridController(IBackOfficeRepository backOfficeRepository)
        {
            this.backOfficeRepository = backOfficeRepository;
        }

        #region Actions

        public FileResult Export([DataSourceRequest]DataSourceRequest request, string model, string columns, string format)
        {
            IEnumerable rows = null;
            /*switch (model)
            {
                case "OperationsExt":
                    rows = OperationExtDataModel.List(backOfficeRepository).ToDataSourceResult(request).Data;
                    break;
            }*/
            // Invoke static method 'List' from DataModel class
            Type modelType = Type.GetType(String.Format("backOffice.Models.{0}DataModel", model));
            //MethodInfo info = modelType.GetMethod("List", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            MethodInfo info = modelType.GetMethod("List", new Type[] { typeof(IBackOfficeRepository) });
            IQueryable value = (IQueryable) info.Invoke(null, new object[] { backOfficeRepository });            
            rows = value.ToDataSourceResult(request).Data;

            string[] arrColumns = columns.Split(',');

            MemoryStream output = new MemoryStream();
            string sContentType = "";
            string sFileName = "";

            switch (format)
            {
                case "xls":
                    ExportExcel(model, modelType, rows, arrColumns, output);
                    sContentType = "application/vnd.ms-excel";
                    sFileName = Resources.ResourceManager.GetString(String.Format("{0}Export_XLSFilename", model)) + ".xls";
                    //sContentType = "application/ms-excel";
                    //sFileName = Resources.ResourceManager.GetString(String.Format("{0}Export_XLSFilename", model)) + ".xlsx";
                    break;
                case "pdf":
                    ExportPdf(model, modelType, rows, arrColumns, output);
                    sContentType = "application/pdf";
                    sFileName = Resources.ResourceManager.GetString(String.Format("{0}Export_PDFFilename", model)) + ".pdf";
                    break;
            }

            //Return the result to the end user
            return File(output.ToArray(), sContentType, sFileName);            
        }

        public FileResult Export2([DataSourceRequest]DataSourceRequest request, string model, string columns, string format)
        {

            Response.Clear();
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            Response.Write("<BR><BR><BR>");
            Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = 30;

            for (int j = 0; j < columnscount; j++)
            {
                //Makes headers bold
                Response.Write("<B>");
                Response.Write("Col" + j);
                Response.Write("</B>");
                Response.Write("</Td>");
            }
            Response.Write("</TR>");            
            for (int j=0; j<66000; j++)
            {
                Response.Write("<TR>");
                for (int i = 0; i < columnscount; i++)
                {
                    Response.Write("<Td>");
                    Response.Write("Value" + j + "-" + i);
                    Response.Write("</Td>");
                }
                Response.Write("</TR>");
            }
            Response.Write("</Table>");
            Response.Write("</font>");
            Response.Flush();
            Response.End();

            return File(Response.OutputStream, "application/ms-excel", "Reports.xls");
        }

        #endregion

        #region Methods

        private void ExportExcel(string model, Type modelType, IEnumerable rows, string[] columns, MemoryStream output)
        {
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();
            //IWorkbook workbook = new XSSFWorkbook();
            //SXSSFWorkbook wb = new SXSSFWorkbook(wb_template); 

            //Create new Excel sheet
            var sheet = ExportExcel_CreateSheet(workbook, model, columns, modelType);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (object row in rows)
            {
                if (rowNumber >= 0xFFFF)
                {
                    sheet = ExportExcel_CreateSheet(workbook, model, columns, modelType);
                    rowNumber = 1;
                }
                //Create a new row
                var sheetRow = sheet.CreateRow(rowNumber++);

                var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;

                //Set values for the cells
                int j = 0;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != "")
                    {
                        string value = "";
                        string value2 = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(columns[i] + "_FK");                        
                        if (propInfo == null) propInfo = row.GetType().GetProperty(columns[i]);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null)
                        {
                            if (propInfo.PropertyType != typeof(DateTime) && propInfo.PropertyType != typeof(DateTime?))
                                value = obj.ToString();
                            else
                            {
                                value = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortDatePattern);
                                value2 = Convert.ToDateTime(obj).ToString(dateTimeFormat.ShortTimePattern);
                            }
                        }
                        sheetRow.CreateCell(i+j).SetCellValue(value);
                        if (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?))
                        {
                            j += 1;
                            sheetRow.CreateCell(i+j).SetCellValue(value2);
                        }
                    }
                }
            }

            //Write the workbook to a memory stream            
            workbook.Write(output);

        }

        private NPOI.SS.UserModel.ISheet ExportExcel_CreateSheet(HSSFWorkbook/*IWorkbook*/ workbook, string model, string[] columns, Type modelType)
        {
            var sheet = workbook.CreateSheet();

            /*for (int i = 0; i < columns.Length; i++)
            {
                sheet.SetColumnWidth(i, 10 * 256);
            }*/

            var headerRow = sheet.CreateRow(0);

            PropertyInfo propInfo = null;
            int j = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    headerRow.CreateCell(i+j).SetCellValue(Resources.ResourceManager.GetString(String.Format("{0}DataModel_{1}", model, columns[i])));
                    propInfo = modelType.GetProperty(columns[i]);
                    if (propInfo != null && (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
                    {
                        j += 1;
                        headerRow.CreateCell(i+j).SetCellValue(Resources.ResourceManager.GetString(String.Format("{0}DataModel_{1}_Time", model, columns[i])));
                    }
                }
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            return sheet;
        }

        private void ExportPdf(string model, Type modelType, IEnumerable rows, string[] columns, MemoryStream output)
        {
            Rectangle pageSize = PageSize.A4;
            if (columns.Length > 5) pageSize = pageSize.Rotate();

            var document = new Document(pageSize, 10, 10, 10, 10);

            PdfWriter.GetInstance(document, output);

            document.Open();

            var numOfColumns = columns.Count(e => e != "");
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Font hFont = new Font(Font.FontFamily.COURIER, 8, Font.BOLD);
            Font rFont = new Font(Font.FontFamily.COURIER, 8, Font.NORMAL);

            // Adding headers
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    dataTable.AddCell(new PdfPCell(new Phrase(Resources.ResourceManager.GetString(String.Format("{0}DataModel_{1}", model, columns[i])), hFont)));
                    //dataTable.AddCell(Resources.ResourceManager.GetString("Account_Op_" + columns[i]));                
                }
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            long iCount = 0;
            foreach (object row in rows)
            {
                foreach (string column in columns)
                {
                    if (column != "")
                    {
                        string value = "";
                        PropertyInfo propInfo = row.GetType().GetProperty(column + "_FK");
                        if (propInfo == null) propInfo = row.GetType().GetProperty(column);
                        object obj = propInfo.GetValue(row, null);
                        if (obj != null) value = obj.ToString();
                        dataTable.AddCell(new PdfPCell(new Phrase(value, rFont)));
                    }
                }
                iCount++;
            }

            if (iCount == 0)
            {
                for (int i = 0; i < columns.Length; i++)
                    if (columns[i] != "") dataTable.AddCell("");
            }

            document.Add(dataTable);

            document.Close();

        }

        #endregion

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

    }
}

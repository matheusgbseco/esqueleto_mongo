using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Common.Domain.Base;
using System.IO;
using OfficeOpenXml;
using System.Drawing;

namespace Common.API
{
    public class ExportExcel<T>
    {
        private FilterBase _filter;

        protected string _fileName;
        protected Dictionary<string, string> _customHeaders;
        protected Dictionary<string, string> _defaultHeaders;

        public ExportExcel(FilterBase filter)
        {
            this._filter = filter;
            if (_defaultHeaders.IsNull()) _defaultHeaders = new Dictionary<string, string>();
            if (_customHeaders.IsNull()) _customHeaders = new Dictionary<string, string>();
        }

        public virtual string GetFileName()
        {
            if (!this._fileName.IsSent())
                this._fileName = Guid.NewGuid().ToString();

            return this._fileName + ".xlsx";
        }

        public virtual string ContentTypeExcel()
        {
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }

        public virtual byte[] ExportFile(HttpResponse response, IEnumerable<T> data, string nome, string rootPath)
        {
            response.Headers.Add("content-disposition", "attachment; filename=Information" + this.GetFileName());
            response.ContentType = this.ContentTypeExcel();
            var path = Path.Combine(rootPath, this.GetFileName());
            var content = this.ExportByComponent(data, nome, new FileInfo(path));
            return content;

        }

        public virtual string ExportByXml(IEnumerable<T> data, string nome)
        {

            var xml = string.Empty;

            xml = "<?xml version=\"1.0\"?><ss:Workbook xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">" +
                "<ss:Styles><ss:Style ss:ID=\"1\"><ss:Font ss:Bold=\"1\"/></ss:Style></ss:Styles>";

            xml += "<ss:Worksheet ss:Name=\"" + nome + "\">";
            xml += " <ss:Table>";

            xml += "  <ss:Row ss:StyleID=\"1\">";

            foreach (var item in data.FirstOrDefault().GetType().GetTypeInfo().GetProperties()
                .Where(_ => _.GetType().GetTypeInfo().IsClass))
            {
                var propriedade = item.Name;
                xml += "<ss:Cell><ss:Data ss:Type=\"String\">" + propriedade + "</ss:Data></ss:Cell>";
            };
            xml += "  </ss:Row>";

            foreach (var item in data)
            {
                var intancia = item;
                xml += " <ss:Row>";
                foreach (var subItem in item.GetType().GetTypeInfo().GetProperties()
                    .Where(_ => _.GetType().GetTypeInfo().IsClass))
                {
                    var valor = subItem.GetValue(item);
                    if (valor.IsNotNull())
                    {
                        var ehNumber = new Regex("/^\\d +$/").IsMatch(valor.ToString());
                        xml += "<ss:Cell><ss:Data ss:Type=\"" + ((ehNumber) ? "Number" : "String") + "\">" + valor + "</ss:Data></ss:Cell>";
                    }
                    else xml += "<ss:Cell><ss:Data ss:Type=\"String\"></ss:Data></ss:Cell>";
                }
                xml += " </ss:Row>";
            }

            xml += "</ss:Table>";
            xml += "</ss:Worksheet>";
            xml += "</ss:Workbook>";

            return xml;
        }

        public virtual byte[] ExportByComponent(IEnumerable<T> data, string nome, FileInfo file)
        {
            var content = default(byte[]);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets.Add(nome);
                var columnIndex = 1;
                var rowIndex = 1;

                foreach (var item in data
                    .FirstOrDefault()
                    .GetType().GetTypeInfo().GetProperties()
                    .Where(_ => _.GetType().GetTypeInfo().IsClass))
                {
                    var propriedade = item.Name;

                    var defaultH = _defaultHeaders.Where(_ => _.Key.ToLower() == item.Name.ToLower()).SingleOrDefault();
                    if (defaultH.Value.IsNotNull()) propriedade = defaultH.Value;

                    var customH = _customHeaders.Where(_ => _.Key.ToLower() == item.Name.ToLower()).SingleOrDefault();
                    if (customH.Value.IsNotNull()) propriedade = customH.Value;

                    worksheet.Cells[rowIndex, columnIndex].Value = propriedade.ToUpper();
                    columnIndex++;
                };


                rowIndex = 2;
                foreach (var item in data)
                {
                    var intancia = item;
                    columnIndex = 1;
                    foreach (var subItem in item.GetType().GetTypeInfo().GetProperties()
                        .Where(_ => _.GetType().GetTypeInfo().IsClass))
                    {
                        var valor = subItem.GetValue(item);
                        if (valor != null)
                        {
                            var isList = valor.GetType().IsInstanceOfType(new List<string>());
                            if (isList)
                            {
                                var novovalor = string.Empty;
                                foreach (var item2 in valor as List<string>) novovalor += item2 + ", ";
                                if (novovalor != string.Empty) novovalor = novovalor.Substring(0, novovalor.Length - 2);
                                worksheet.Cells[rowIndex, columnIndex].Value = novovalor;
                                columnIndex++;
                                continue;
                            }

                            var isDate = valor.IsDate();
                            if (isDate)
                            {
                                var date = Convert.ToDateTime(valor);
                                if (date.Hour == 0 && date.Minute == 0)
                                    worksheet.Cells[rowIndex, columnIndex].Style.Numberformat.Format = "dd/MM/yyyy";
                                else
                                    worksheet.Cells[rowIndex, columnIndex].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                            }

                            var isBoolean = Convert.ToString(valor) == "True" || Convert.ToString(valor) == "False";
                            if (isBoolean)
                                valor = Convert.ToString(valor) == "True" ? "Sim" : "Não";

                            var isDecimal = valor.GetType() == typeof(Decimal);
                            if (isDecimal)
                            {
                                worksheet.Cells[rowIndex, columnIndex].Style.Numberformat.Format = "#,##0.00";
                            }
                        }

                        worksheet.Cells[rowIndex, columnIndex].Value = valor;
                        columnIndex++;
                    }
                    rowIndex++;
                }

                columnIndex--;

                worksheet.Cells[1, 1, 1, columnIndex].AutoFilter = true;
                worksheet.Cells[1, 1, 1, columnIndex].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, columnIndex].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[1, 1, 1, columnIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, columnIndex].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                worksheet.Cells[1, 1, rowIndex, columnIndex].AutoFitColumns();

                content = package.GetAsByteArray();
            }

            return content;

        }

    }
}

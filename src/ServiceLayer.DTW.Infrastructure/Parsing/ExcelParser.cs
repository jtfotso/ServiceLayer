using OfficeOpenXml;
using ServiceLayer.DTW.Application.DTOs;
using ServiceLayer.DTW.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.DTW.Infrastructure.Parsing
{
    public class ExcelParser : IFileParser
    {
        public Task<List<ParsedRowDto>> ParseAsync(Stream stream, string fileName, CancellationToken ct = default)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            int colCount = worksheet.Dimension?.Columns ?? 0;
            int rowCount = worksheet.Dimension?.Rows ?? 0;

            if(colCount == 0 || rowCount < 2)
                return Task.FromResult(new List<ParsedRowDto>());

            // First row = headers
            var headers = Enumerable.Range(1, colCount)
                .Select(i => worksheet.Cells[1, i].Text)
                .ToArray();

            var rows = new List<ParsedRowDto>();
            for (int row = 2; row <= rowCount; row++)
            {
                var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (int col = 1; col <= colCount; col++)
                {
                    var header = headers[col - 1];
                    fields[header] = worksheet.Cells[row, col].Text.Trim();
                }
                rows.Add(new ParsedRowDto { RowNumber = row - 1, Fields = fields });
            }

            return Task.FromResult(rows);
        }

        public bool Supports(string fileName)
            => Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);
    }
}

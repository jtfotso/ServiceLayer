using CsvHelper.Configuration;
using ServiceLayer.DTW.Application.DTOs;
using ServiceLayer.DTW.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ServiceLayer.DTW.Infrastructure.Parsing
{
    public class CsvParser : IFileParser
    {
        public Task<List<ParsedRowDto>> ParseAsync(Stream stream, string fileName, CancellationToken ct = default)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) 
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvHelper.CsvReader(reader, config);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? [];

            var rows = new List<ParsedRowDto>();
            int rowNum = 1;

            while (csv.Read())
            {
                var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var header in headers)
                    fields[header] = csv.GetField(header) ?? string.Empty;

                rows.Add(new ParsedRowDto { RowNumber = rowNum++, Fields = fields });
            }
            return Task.FromResult(rows);
        }

        public bool Supports(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext is ".csv" or ".txt";
        }
    }
}

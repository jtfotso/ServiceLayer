using ServiceLayer.DTW.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.DTW.Application.Interfaces
{
    public interface IFileParser
    {
        Task<List<ParsedRowDto>> ParseAsync(Stream stream, string fileName, CancellationToken ct = default);
        bool Supports(string fileName);
    }
}

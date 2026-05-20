using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.DTW.Application.DTOs
{
    public class ParsedRowDto
    {
        public int RowNumber { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
    public class Response
    {
        public int? code { get; set; } 
        public string? message { get; set; } 
        public object? content { get; set; }
    }


    public class ResponseResult
    {  
        public object? result { get; set; }
    }

    public class FullResponse
    {
        public int? code { get; set; }

        public string? message { get; set; }

        public ResponseResult? content { get; set; }

    }
}

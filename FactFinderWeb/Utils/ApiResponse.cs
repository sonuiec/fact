using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactFinderWeb.Utils
{
    public class ApiResponse
    {
        public string Status { get; set; }
        public int Code { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(string status, int code, int id, string message)
        {
            Status = status;
            Code = code;
            Id = id;
            Message = message;
        }
    }
}

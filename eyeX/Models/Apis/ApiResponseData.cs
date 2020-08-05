using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eyeX.Models.Apis
{
    public class ApiResponseData
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponseData()
        {
            var x = 1;
        }

        public ApiResponseData(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}

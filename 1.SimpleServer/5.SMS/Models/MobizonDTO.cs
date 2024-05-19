using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5.SMS.Models
{
    public class MobizonDTO<T> where T : class
    {
        public int Code { get; set; }
        public T Data { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
    }
}

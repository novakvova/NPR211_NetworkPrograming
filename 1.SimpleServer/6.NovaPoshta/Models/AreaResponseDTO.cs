using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models
{
    public class AreaResponseDTO
    {
        public bool Success { get; set; }
        public List<AreaItemResponseDTO> Data { get; set; }
    }
}

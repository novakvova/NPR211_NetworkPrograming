using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.City
{
    public class SettlementResponseDTO
    {
        public bool Success { get; set; }
        public List<SettlementItemResponseDTO> Data { get; set; }
    }
}

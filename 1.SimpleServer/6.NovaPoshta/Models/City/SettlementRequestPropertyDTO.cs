using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6.NovaPoshta.Models.City
{
    public class SettlementRequestPropertyDTO
    {
        /// <summary>
        /// Номер сторінки
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Наявність нової пошти в населеному пункті
        /// </summary>
        public int Warehouse { get; set; } = 1;
        /// <summary>
        /// Кількість населених пунктів за 1 запит
        /// </summary>
        public int Limit { get; set; } = 200;
    }
}

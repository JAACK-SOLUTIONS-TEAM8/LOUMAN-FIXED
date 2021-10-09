using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Order
{
    public class CardDetailDto
    {
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
    }
}

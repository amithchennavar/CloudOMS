using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class OrderStatusUpdate
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string NewStatus { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

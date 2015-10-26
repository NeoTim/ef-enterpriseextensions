﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosquare.Labs.EntityFramework.EnterpriseExtensions.Sample.Database
{
    public class Order
    {
        public Order()
        {
            Details = new HashSet<OrderDetail>();
        }

        [Key]
        public int OrderID { get; set; }

        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public bool IsShipped { get; set; }

        public decimal Amount { get; set; }
        public DateTime ShippedDate { get; set; }

        public string CreatedUserId { get; set; }

        public int? WarehouseID { get; set; }

        public int OrderType { get; set; }
        
        public ICollection<OrderDetail> Details { get; set; }
    }
}

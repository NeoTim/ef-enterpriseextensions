﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EntityFramework.EnterpriseExtensions.ObjectModel;

namespace Unosquare.Labs.EntityFramework.EnterpriseExtensions.Sample.Database
{
    class AuditTrailEntry : IAuditTrailEntry
    {
        [Key]
        public int AuditId { get; set; }

        public string UserId { get; set; }
        public string TableName { get; set; }
        public int Action { get; set; }
        public string JsonBody { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

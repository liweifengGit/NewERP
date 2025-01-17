﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infobasis.Data.DataEntity
{
    [Table("SYtbSystemNum")]
    public class SystemNum : TenantEntity
    {
        [Required, StringLength(20)]
        public string NumType { get; set; }
        public int Num { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OptiSoftBlazor.Shared.Data.Tenant
{
    public class Tenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? ID { get; set; }

        public string? Name { get; set; }

        public string? ConnectionString { get; set; }
    }
}

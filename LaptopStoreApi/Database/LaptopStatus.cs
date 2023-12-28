﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{
    [Table("LaptopStatus")]
    public class LaptopStatus
    {
        [Key]
        public int LaptopStatusId { get; set; }
        public ICollection<Image>? Images { get; set; }
        public string Information { get; set; }
    
        [ForeignKey("LaptopId")]
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; } // Navigation Property
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookAPI.Model
{
    public partial class Word
    {
        public int WordId { get; set; }
        public int? BookId { get; set; }
        [Required]
        [Column("Word")]
        [StringLength(8000)]
        public string Word1 { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Word")]
        public virtual Book Book { get; set; }
    }
}

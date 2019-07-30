using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookAPI.Model
{
    public partial class Book
    {
        public Book()
        {
            Word = new HashSet<Word>();
        }

        public int BookId { get; set; }
        [Required]
        [StringLength(2555)]
        public string BookTitle { get; set; }
        [Required]
        [StringLength(2555)]
        public string BookAuthor { get; set; }
        public int BookPages { get; set; }
        [Required]
        [Column("WebURL")]
        [StringLength(8000)]
        public string WebUrl { get; set; }
        [Required]
        [Column("ThumbnailURL")]
        [StringLength(8000)]
        public string ThumbnailUrl { get; set; }
        [Column("isRead")]
        public bool IsRead { get; set; }

        [InverseProperty("Book")]
        public virtual ICollection<Word> Word { get; set; }
    }
}

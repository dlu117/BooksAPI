using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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
    [DataContract]
    // DTO defines what part of object that is sent to user
    // Here still allow user to see everything 
    public class BookDTO
    {
        [DataMember]
        public int BookId { get; set; }

        [DataMember]
        public string BookTitle { get; set; }

        [DataMember]
        public string BookAuthor { get; set; }

        [DataMember]
        public int BookPages { get; set; }

        [DataMember]
        public string WebUrl { get; set; }

        [DataMember]
        public string ThumbnailUrl { get; set; }

        [DataMember]
        public bool IsRead { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace accountapi.Models
{
    [Table("tokenhistory")]
    public class TokenHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenHistoryId { get; set; }

        [ForeignKey("UserForeignKey")]
        public User User { get; set; }
        
        public String AuthToken { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime AccessTime { get; set; }

    }
}

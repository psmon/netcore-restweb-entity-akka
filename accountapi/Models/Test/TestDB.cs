using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace accountapi.Models.Test
{
    [Table("test_blog")]
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }

        public string Url { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }

    [Table("test_person")]
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }

        // ENGTIP - LastName:성 FirstName:이름
        [ConcurrencyCheck]
        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}

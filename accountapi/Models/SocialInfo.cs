using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace accountapi.Models
{
    [Table("socaialinfo")]
    public class SocialInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SocialId { get; set; }

        [ForeignKey("UserForeignKey")]
        public User User { get; set; }


        public String SocialProviderName { get; set; }

        public String SocialUserNo { get; set; }

        public String NickName { get; set; }
        

    }
}

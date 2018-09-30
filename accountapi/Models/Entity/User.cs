using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace accountapi.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public Profile Profile { get; set; }

        public List<SocialInfo> SocialInfos { get; set; }

        public List<TokenHistory> TokenHistorys { get; set; }

        public Boolean IsSocialActive { get; set; }

        [StringLength(50,MinimumLength = 3)]
        [Required]
        public String NickName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public String MyId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public String PassWord { get; set; }

        [Required]
        public DateTime RegDate { get; set; }
        

    }
}

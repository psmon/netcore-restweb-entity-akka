using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace accountapi.Models
{
    [Table("student", Schema = "db_account")]
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime RegDate { get; set; }

    }
}

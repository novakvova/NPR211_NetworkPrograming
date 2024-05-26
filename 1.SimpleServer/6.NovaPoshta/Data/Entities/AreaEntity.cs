using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace _6.NovaPoshta.Data.Entities
{
    [Table("tblAreas")]
    public class AreaEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Ref { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}

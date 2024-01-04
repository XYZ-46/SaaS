using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public abstract class ABaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool IsDelete { get; set; } = false;

        public string CreateBy { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CreateDate { get; set; }
        public string? LastUpdateBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
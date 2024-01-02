namespace DataEntity
{
    public abstract class ABaseModel
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; } = false;

        public string CreateBy { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string? LastUpdateBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}

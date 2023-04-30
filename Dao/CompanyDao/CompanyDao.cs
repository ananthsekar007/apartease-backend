namespace apartease_backend.Dao.CompanyDao
{
    public class CompanyDao
    {
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public string CompanyZip { get; set; } = string.Empty;

        public int CategoryId { get; set; }
    }
}

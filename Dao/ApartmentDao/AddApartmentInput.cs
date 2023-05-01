namespace apartease_backend.Dao.ApartmentDao
{
    public class AddApartmentInput
    {
        public string Name { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;

        public int ManagerId { get; set; }
    }
}

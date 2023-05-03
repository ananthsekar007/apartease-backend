namespace apartease_backend.Dao
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public string Error { get; set; }
    }
}

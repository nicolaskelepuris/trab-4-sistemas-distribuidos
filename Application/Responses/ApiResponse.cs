namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public Error? Error { get; set; }
    }

    public class Error
    {
        public Error(string? message)
        {
            Message = message;
        }

        public string? Message { get; private set; }
    }
}

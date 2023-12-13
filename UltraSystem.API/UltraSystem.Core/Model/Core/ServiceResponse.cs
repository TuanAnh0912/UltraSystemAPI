namespace UltraSystem.Core.Model.Core
{
    public class ServiceResponse
    {
        public ServiceResponse(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public ServiceResponse()
        {
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}

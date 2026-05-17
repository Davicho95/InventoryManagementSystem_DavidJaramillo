namespace Aplicacion.Wrappers
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T? data, string message = null!)
        {
            Succeeded = true;
            Data = data;
            Message = message;
        }

        public Response(string message = null!, List<string>? errores = null)
        {
            Succeeded = false;
            Message = message;
            Errors = errores ?? new List<string>();
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }
    }
}

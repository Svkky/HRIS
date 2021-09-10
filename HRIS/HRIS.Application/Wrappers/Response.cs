using System.Collections.Generic;

namespace HRIS.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T Data, int StatusCode = 0, bool Succeeded = false, string Message = "", List<string> Errors = null)
        {
            Succeeded = this.Succeeded;
            Message = this.Message;
            Data = this.Data;
            Errors = this.Errors;
            StatusCode = this.StatusCode;
        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public int StatusCode { get; set; }
        public string ResponseCode { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}

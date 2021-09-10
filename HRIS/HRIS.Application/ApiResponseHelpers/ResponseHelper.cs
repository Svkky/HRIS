using HRIS.Application.Wrappers;

namespace HRIS.Application.ApiResponseHelpers
{
    public class ResponseHelper
    {
        public static Response<string> SuccessMessage(string message)
        {
            var response = new Response<string>
            {
                Data = message,
                Message = message,
                ResponseCode = "00",
                StatusCode = 200,
                Succeeded = true,
            };
            return response;
        }

        public static Response<string> FailureMessage(string message)
        {
            var response = new Response<string>
            {
                Data = message,
                Message = message,
                ResponseCode = "-1",
                StatusCode = 400,
                Succeeded = false,
            };
            return response;
        }
        public static Response<string> AlreadyExistMessage(string message)
        {
            var response = new Response<string>
            {
                Data = message,
                Message = message,
                ResponseCode = "-1",
                StatusCode = 409,
                Succeeded = false,
            };
            return response;
        }
    }
}

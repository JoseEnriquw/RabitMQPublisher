using Core.Domain.Classes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaApi.Helpers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        private ISender _sender = null!;

        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        public IActionResult Result<T>(Response<T> response)
        {
            AddHeaders(this, response);
            return response.IsValid ? RequestSucess(response) : RequestError(response);
        }

        private IActionResult RequestError<T>(Response<T> response)
        {
            return new JsonResult(response.Notifications) { StatusCode = (int)response.StatusCode };
        }
        private IActionResult RequestSucess<T>(Response<T> response)
        {
            return new JsonResult(response.Content) { StatusCode = (int)response.StatusCode };
        }
        private void AddHeaders<T>(ControllerBase controller, Response<T> response)
        {
            if (response.Headers.Any())
            {
                foreach (var header in response.Headers)
                    controller.Response.Headers.Add(header.Key, header.Value);
            }
        }

    }
}

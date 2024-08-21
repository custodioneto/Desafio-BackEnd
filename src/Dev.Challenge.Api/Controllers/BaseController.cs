using Dev.Challenge.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Net;

namespace Dev.Challenge.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IMediator _mediator;

        public IMediator Mediator => _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (DomainException exception)
            {
                return BadRequest(new
                {
                    error = exception.Message,
                    statusCode = (int)HttpStatusCode.BadRequest,
                    details = "One or more validation errors occurred."
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new
                {
                    error = exception.Message,
                    statusCode = (int)HttpStatusCode.BadRequest,
                    details = exception.StackTrace
                });
            }
        }
    }
}

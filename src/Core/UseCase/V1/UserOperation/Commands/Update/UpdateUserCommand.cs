using Core.Common.Interfaces;
using Core.Domain.Classes;
using Core.Domain.Common;
using Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Core.UseCase.V1.UserOperation.Commands.Update
{
    public class UpdateUserCommand : IRequest<Response<User>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<User>>
    {
        private readonly IRepositoryEF _repository;
        private readonly IPublisherRabbitMQ _publisher;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IRepositoryEF repository, IPublisherRabbitMQ publisher, ILogger<UpdateUserCommandHandler> logger)
        {
            _repository = repository;
            _publisher = publisher;
            _logger = logger;
        }
        public async Task<Response<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.FindAsync<User>(x => x.Id == request.Id);
            var response = new Response<User>();
            if (result is null)
            {
                response.AddNotification("#123", nameof(request.Id), string.Format(ErrorMessage.NOT_FOUND_GET_BY_ID, request.Id, nameof(User)));
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                result.Name = request.Name;
                result.Email = request.Email;
                _repository.Update(result);
                await _repository.SaveChangesAsync();
                response.Content = result;
                response.StatusCode = HttpStatusCode.OK;
                await _publisher.Publish(result, "user.update");
            }
            return response;
        }
    }
}

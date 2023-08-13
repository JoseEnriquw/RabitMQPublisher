using Core.Common.Interfaces;
using Core.Domain.Classes;
using Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Core.UseCase.V1.UserOperation.Commands.Create
{
    public class CreateUserCommand : IRequest<Response<CreateUserResponse>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<CreateUserResponse>>
    {
        private readonly IRepositoryEF _repository;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IRepositoryEF repository, ILogger<CreateUserCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User entity = new()
            {
                Name = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.Now,
            };

            _repository.Insert(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"User created successfully with id: {entity.Id}");

            return new Response<CreateUserResponse>
            {
                Content = new CreateUserResponse
                {
                    id = entity.Id,
                    message = "User created successfully"
                },
                StatusCode = HttpStatusCode.Created,
            };
        }
    }
}

using Core.Common.Interfaces;
using Core.Domain.Classes;
using Core.Domain.Common;
using Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Core.UseCase.V1.UserOperation.Commands.Delete
{
    public class DeleteUserCommand : IRequest<Response<DeleteUserResponse>>
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<DeleteUserResponse>>
    {
        private readonly IRepositoryEF _repository;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(IRepositoryEF repository, ILogger<DeleteUserCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response<DeleteUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.FindAsync<User>(x => x.Id == request.Id);

            var response = new Response<DeleteUserResponse>();
            if (result is null)
            {
                response.AddNotification("#123", nameof(request.Id), string.Format(ErrorMessage.NOT_FOUND_GET_BY_ID, request.Id, nameof(User)));
                response.StatusCode = HttpStatusCode.NotFound;
                _logger.LogError(ErrorMessage.NOT_FOUND_GET_BY_ID, request.Id, nameof(User));
            }
            else
            {
                _repository.Delete(result);
                await _repository.SaveChangesAsync();
                _logger.LogInformation(ErrorMessage.USER_DELETED, result.Name);
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new DeleteUserResponse
                {
                    message = string.Format(ErrorMessage.USER_DELETED, result.Name)
                };
            }

            return response;
        }
    }

}

using Core.Common.Interfaces;
using Core.Domain.Classes;
using Core.Domain.Entities;
using MediatR;
using System.Net;

namespace Core.UseCase.V1.UserOperation.Queries.List
{
    public class GetAllUser : IRequest<Response<List<User>>>
    {
    }

    public class GetAllUserHandler : IRequestHandler<GetAllUser, Response<List<User>>>
    {
        private readonly IRepositoryEF _repository;
        public GetAllUserHandler(IRepositoryEF repository)
        {
            _repository = repository;
        }
        public async Task<Response<List<User>>> Handle(GetAllUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync<User>();
            return new Response<List<User>>
            {
                Content = result,
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}

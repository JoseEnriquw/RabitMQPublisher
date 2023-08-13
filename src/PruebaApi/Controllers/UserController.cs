using Core.Domain.Classes;
using Core.Domain.Entities;
using Core.UseCase.V1.UserOperation.Commands.Create;
using Core.UseCase.V1.UserOperation.Commands.Delete;
using Core.UseCase.V1.UserOperation.Commands.Update;
using Core.UseCase.V1.UserOperation.Queries.List;
using Microsoft.AspNetCore.Mvc;
using PruebaApi.Helpers;
using PruebaApi.Models;

namespace PruebaApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ApiControllerBase
    {
        /// <summary>
        /// Creación de nuevo usuario
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateUserCommand body) => Result(await Sender.Send(body));


        /// <summary>
        /// Actualización de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateVM body) => Result(await Sender.Send(new UpdateUserCommand
        {
            Id = id,
            Name = body.name,
            Email = body.email
        }));

        /// <summary>
        /// Eliminación de un usuario
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) => Result(await Sender.Send(new DeleteUserCommand { Id = id }));

        /// Listado de Usuarios de la base de datos
        /// <remarks>en los remarks podemos documentar información más detallada</remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get() => Result(await Sender.Send(new GetAllUser()));

    }
}

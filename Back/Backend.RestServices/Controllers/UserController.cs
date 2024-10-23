using Backend.Domain.Entities.Entities.User;
using Backend.Domain.Entities.Util;
using Backend.Infraestructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendCMS.RestServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Return>> Login(UserDTO request)
        {
            Return response = await _unitOfWork.UserRepository.Login(request);
            if (!response.Valid)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("Search")]
        public async Task<ActionResult<Return>> Search(UserSearchDTO request)
        {
            Return response = await _unitOfWork.UserRepository.Search(request);
            if (!response.Valid)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Return>> Add(UserModel request)
        {
            using IUnitOfWork u = _unitOfWork;
            Return response = await u.UserRepository.Add(request);
            u.Commit();
            if (!response.Valid)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<Return>> Delete(Entity request)
        {
            using IUnitOfWork u = _unitOfWork;
            Return response = await u.UserRepository.Delete(request);
            u.Commit();
            if (!response.Valid)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}

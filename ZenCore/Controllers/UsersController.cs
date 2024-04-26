using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZenCore.DataAccess;
using ZenCore.Entities;
using ZenCore.Models;

namespace ZenCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userRepository.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute]Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]Models.UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = new Guid();
            await _userRepository.CreateUserAsync(user);
            return Ok(user.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody]UserForUpdateDto userDto)
        {
            var updatedUser = await _userRepository.UpdateUserAsync(_mapper.Map<User>(userDto));
            if (updatedUser != null)
            {
                return Ok(updatedUser);
            }
            return NotFound();
        }

    }
}

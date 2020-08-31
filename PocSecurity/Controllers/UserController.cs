using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PocSecurity.Models;
using PocSecurity.Sensitive;
using PocSecurity.Services;
using System.Collections.Generic;
using System.Linq;

namespace PocSecurity.Controllers
{
    [Route("user")]
    public class UserController : ControllerBase
    {
        private List<User> _users = new List<User>() 
        {
            new User(1, "test", "99999999999"),
            new User(2, "test2", "00000000000"),
            new User(3, "test3", "55555555555")
        };

        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Route("get")]
        public IActionResult Get([SensitiveParameter] string id)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == int.Parse(id)).FirstOrDefault());
            return Ok(user);
        }

        [Route("get2"), HttpPost]
        public IActionResult Get([SensitiveParameter, FromBody] List<UserQueryModel> userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == int.Parse(userQueryModel[0].Id)).FirstOrDefault());
            return Ok(user);
        }

        [Route("list")]
        public IActionResult Get()
        {
            var users = _mapper.Map<List<UserQueryModel>>(_users);
            return Ok(users);
        }

        [Route("list/first")]
        public IActionResult GetFirst()
        {
            var user = _mapper.Map<UserQueryModel>(_users[0]);
            return Ok(user);
        }
    }
}

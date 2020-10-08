using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PocSecurityDotNetCore.Attributes;
using PocSecurityDotNetCore.Http;
using PocSecurityDotNetCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace PocSecurityDotNetCore.Controllers
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
        public IActionResult Get([ModelBinder(typeof(SensitiveRijndaelParameterBinder))] Sensitive id)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get2"), HttpPost]
        public IActionResult Get([ModelBinder(typeof(SensitiveRijndaelClassBinder))] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel.Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get3"), HttpPost]
        public IActionResult Get([ModelBinder(typeof(SensitiveRijndaelClassBinder))] List<UserQueryModel> userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel[0].Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get4"), HttpPost]
        public IActionResult Get4([ModelBinder(typeof(SensitiveRijndaelClassBinder))] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel.CommandId.Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("list")]
        public IActionResult Get()
        {
            var users = _mapper.Map<List<UserQueryModel>>(_users);
            return this.OkSensitive(users);
        }

        [Route("list/first")]
        public IActionResult GetFirst()
        {
            var user = _mapper.Map<UserQueryModel>(_users[0]);
            return this.OkSensitive(user);
        }
    }
}

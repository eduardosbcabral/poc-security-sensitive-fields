using AutoMapper;
using PocSecurityDotNetFramework.Models;
using PocSecurityDotNetFramework.Sensitive;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PocSecurity.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        private List<User> _users = new List<User>() 
        {
            new User(1, "test", "99999999999"),
            new User(2, "test2", "00000000000"),
            new User(3, "test3", "55555555555")
        };

        private readonly IMapper _mapper;

        public UserController()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserQueryModel>());
            _mapper = config.CreateMapper();
        }

        [Route("get")]
        public IHttpActionResult Get([SensitiveParameter] string id)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == int.Parse(id)).FirstOrDefault());
            return Ok(user);
        }

        [Route("get2"), HttpPost]
        public IHttpActionResult Get([SensitiveParameter, FromBody] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == int.Parse(userQueryModel.Id)).FirstOrDefault());
            return Ok(user);
        }

        [Route("list")]
        public IHttpActionResult Get()
        {
            var users = _mapper.Map<List<UserQueryModel>>(_users);
            return Ok(users);
        }

        [Route("list/first")]
        public IHttpActionResult GetFirst()
        {
            var user = _mapper.Map<UserQueryModel>(_users[0]);
            return Ok(user);
        }
    }
}

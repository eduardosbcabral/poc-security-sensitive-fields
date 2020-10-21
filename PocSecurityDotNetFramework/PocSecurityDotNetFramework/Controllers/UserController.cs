using AutoMapper;
using PocSecurityDotNetFramework.Http;
using PocSecurityDotNetFramework.Models;
using PocSecurityDotNetFramework.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

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
        private readonly ICipherService _cipherService;

        public UserController()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserQueryModel>());
            _mapper = config.CreateMapper();

            _cipherService = new RijndaelCipherService();
        }

        [Route("get")]
        public IHttpActionResult Get(int id)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get2"), HttpPost]
        public IHttpActionResult Get([FromBody] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel.Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get3"), HttpPost]
        public IHttpActionResult Get([FromBody] List<UserQueryModel> userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel[0].Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("get4"), HttpPost]
        public IHttpActionResult Get4([FromBody] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel.CommandId.Id).FirstOrDefault());
            return this.OkSensitive(user);
        }

        [Route("list")]
        public IHttpActionResult Get()
        {
            var users = _mapper.Map<List<UserQueryModel>>(_users);
            return this.OkSensitive(users);
        }

        [Route("list/first")]
        public IHttpActionResult GetFirst()
        {
            var user = _mapper.Map<UserQueryModel>(_users[0]);
            return this.OkSensitive(user);
        }
    }
}

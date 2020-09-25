using AutoMapper;
using PocSecurityDotNetFramework.Http;
using PocSecurityDotNetFramework.Models;
using PocSecurityDotNetFramework.Sensitive;
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
        public IHttpActionResult Get([SensitiveParameter] string id)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == int.Parse(id)).FirstOrDefault());
            return Ok(user);
        }

        [Route("get2"), HttpPost]
        public IHttpActionResult Get([ModelBinder(typeof(SensitiveClassBinder))] UserQueryModel userQueryModel)
        {
            var user = _mapper.Map<UserQueryModel>(_users.Where(x => x.Id == userQueryModel.Id).FirstOrDefault());
            return this.OkSensitive(user, _cipherService);
        }

        [Route("list")]
        public IHttpActionResult Get()
        {
            var users = _mapper.Map<List<UserQueryModel>>(_users);
            return this.OkSensitive(users, _cipherService);
        }

        [Route("list/first")]
        public IHttpActionResult GetFirst()
        {
            var user = _mapper.Map<UserQueryModel>(_users[0]);
            return this.OkSensitive(user, _cipherService);
        }
    }
}

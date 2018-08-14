using System.Collections.Generic;
using AgenciaDeEmpleoVirutal.Entities.Referentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private AppSettings _appSettings;
        private SendMailData _sendMail;
        private UserSecretSettings _secrets;
        public ValuesController(IOptions<AppSettings> appSettings, IOptions<SendMailData> sendGridOptions, IOptions<UserSecretSettings> secrets)
        {
            _appSettings = appSettings.Value;
            _sendMail = sendGridOptions.Value;
            _secrets = secrets.Value;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { _secrets.SendMailApiKey, _secrets.TableStorage, _sendMail.EmailAddressFrom };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Core.Models.Sessions;
using Core.Service;

namespace API.Controllers.Sessions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET: api/Session
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetAllSessions()
        {
            var sessions = await _sessionService.GetAllSessions();
            return Ok(sessions);
        }

        // GET api/Session/5
        [HttpGet("{id}")]
        public SessionService GetSessionById(int id)
        {
            return GetSessionById(id);
        }

        // POST api/<SessionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SessionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SessionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

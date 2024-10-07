using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VictuZ_2._0_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VictuZ_2 : ControllerBase
    {
        // GET: api/<VictuZ_2>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VictuZ_2>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VictuZ_2>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VictuZ_2>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VictuZ_2>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

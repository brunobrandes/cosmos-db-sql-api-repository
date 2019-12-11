using Azure.Cosmos;
using Foo.Domain.Entities.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foo.Ui.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FooController : ControllerBase
    {
        private readonly IFooRepository _fooRepository;

        public FooController(IFooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(Domain.Entities.Foo foo)
        {
            await _fooRepository.AddAsync(foo, new PartitionKey(foo.City));
            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            var result = new List<Domain.Entities.Foo> { };

            await foreach (var item in _fooRepository.GetAllAsync())
                result.Add(item);

            return Ok(result);
        }

        [HttpGet("{id}/{partitionKey}")]
        public async Task<IActionResult> GetAsync(string id, string partitionKey)
        {
            var foo = await _fooRepository.GetByIdAsync(id, new PartitionKey(partitionKey));
            return Ok(foo);
        }

        [HttpGet("{neighborhood}")]
        public async Task<IActionResult> GetAsync(string neighborhood)
        {
            var foos = await _fooRepository.GetByNeighborhood(neighborhood);
            return Ok(foos);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync(Domain.Entities.Foo foo)
        {
            await _fooRepository.UpdateAsync(foo, new PartitionKey(foo.City));
            return Ok();
        }

        [HttpDelete("{id}/{partitionKey}")]
        public async Task<IActionResult> DeleteAsync(string id, string partitionKey)
        {
            await _fooRepository.DeleteAsync(id, new PartitionKey(partitionKey));
            return Ok();
        }


    }
}
using CodeBridgeTestApp.Dtos;
using CodeBridgeTestApp.Models;
using CodeBridgeTestApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeBridgeTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;

        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDogs(string attribute, string order = "asc", int pageNumber = 1, int pageSize = 10)
        {
            var dogs = await _dogService.GetDogsAsync(attribute, order, pageNumber, pageSize);
            return Ok(dogs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDog([FromBody] DogCreateDto dogDto)
        {
            var result = await _dogService.CreateDogAsync(dogDto);
            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok();
        }
    }
}

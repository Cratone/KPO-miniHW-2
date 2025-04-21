using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosureController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly IAnimalRepository _animalRepository;

        public EnclosureController(IEnclosureRepository enclosureRepository, IAnimalRepository animalRepository)
        {
            _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetAllEnclosures()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return Ok(enclosures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
                return NotFound();

            return Ok(enclosure);
        }

        [HttpPost]
        public async Task<ActionResult<Enclosure>> CreateEnclosure([FromBody] CreateEnclosureRequest request)
        {
            try
            {
                var animalType = AnimalTypeValueObject.FromString(request.AnimalType);
                var size = new PositiveIntegerValueObject(request.Size);
                var maxCapacity = new PositiveIntegerValueObject(request.MaxCapacity);

                var enclosure = new Enclosure(animalType, size, maxCapacity);
                
                await _enclosureRepository.AddAsync(enclosure);

                return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosure);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
                return NotFound();

            // Check if enclosure has animals
            var animals = await _animalRepository.GetByEnclosureIdAsync(id);
            if (animals.GetEnumerator().MoveNext())
                return BadRequest("Cannot delete enclosure that contains animals");

            await _enclosureRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/clean")]
        public async Task<IActionResult> CleanEnclosure(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
                return NotFound();

            enclosure.Clean();
            await _enclosureRepository.UpdateAsync(enclosure);
            return Ok("Enclosure cleaned successfully");
        }

        [HttpGet("{id}/animals")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetEnclosureAnimals(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
                return NotFound();

            var animals = await _animalRepository.GetByEnclosureIdAsync(id);
            return Ok(animals);
        }
    }

    public class CreateEnclosureRequest
    {
        public string AnimalType { get; set; }
        public int Size { get; set; }
        public int MaxCapacity { get; set; }
    }
} 
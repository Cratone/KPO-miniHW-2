using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly AnimalTransferService _transferService;
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;

        public AnimalController(IAnimalRepository animalRepository, AnimalTransferService transferService, IFeedingScheduleRepository feedingScheduleRepository)
        {
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
            _transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
            _feedingScheduleRepository = feedingScheduleRepository ?? throw new ArgumentNullException(nameof(feedingScheduleRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAllAnimals()
        {
            var animals = await _animalRepository.GetAllAsync();
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(Guid id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] CreateAnimalRequest request)
        {
            try
            {
                AnimalTypeValueObject animalType = AnimalTypeValueObject.FromString(request.AnimalType);
                var species = new SpeciesValueObject(new NonEmptyStringValueObject(request.Species), animalType);
                var name = new NonEmptyStringValueObject(request.Name);
                var birthDate = new DateValueObject(request.BirthDate);
                var gender = GenderValueObject.FromString(request.IsMale ? "Male" : "Female");
                var favoriteFood = new NonEmptyStringValueObject(request.FavoriteFood);
                var healthStatus = HealthStatusValueObject.FromString(request.IsHealthy ? "Healthy" : "Sick");

                var animal = new Animal(species, name, birthDate, gender, favoriteFood, healthStatus);
                
                await _animalRepository.AddAsync(animal);

                return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/feeding-schedules")]
        public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetSchedulesByAnimal(Guid id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
                return NotFound("Animal not found");

            var schedules = await _feedingScheduleRepository.GetByAnimalIdAsync(id);
            return Ok(schedules);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(Guid id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
                return NotFound();

            await _animalRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/feed")]
        public async Task<IActionResult> FeedAnimal(Guid id, [FromBody] FeedAnimalRequest request)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
                return NotFound();

            try
            {
                var food = new NonEmptyStringValueObject(request.Food);
                animal.Feed(food);
                await _animalRepository.UpdateAsync(animal);
                return Ok("Animal fed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/heal")]
        public async Task<IActionResult> HealAnimal(Guid id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
                return NotFound();

            animal.Heal();
            await _animalRepository.UpdateAsync(animal);
            return Ok("Animal healed successfully");
        }

        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> TransferAnimal(Guid id, [FromBody] TransferAnimalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _transferService.TransferAnimalAsync(id, request.TargetEnclosureId);
            
            if (!result)
                return BadRequest("Transfer failed. Please check if the animal and target enclosure exist and are compatible.");

            return Ok("Animal transferred successfully");
        }
    }

    public class CreateAnimalRequest
    {
        public string Species { get; set; }
        public string AnimalType { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsMale { get; set; }
        public string FavoriteFood { get; set; }
        public bool IsHealthy { get; set; }
    }

    public class FeedAnimalRequest
    {
        public string Food { get; set; }
    }

    public class TransferAnimalRequest
    {
        public Guid TargetEnclosureId { get; set; }
    }
} 
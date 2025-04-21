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
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly FeedingOrganizationService _feedingService;

        public FeedingScheduleController(
            IFeedingScheduleRepository feedingScheduleRepository,
            IAnimalRepository animalRepository,
            FeedingOrganizationService feedingService)
        {
            _feedingScheduleRepository = feedingScheduleRepository ?? throw new ArgumentNullException(nameof(feedingScheduleRepository));
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
            _feedingService = feedingService ?? throw new ArgumentNullException(nameof(feedingService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetAllSchedules()
        {
            var schedules = await _feedingScheduleRepository.GetAllAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedingSchedule>> GetSchedule(Guid id)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return NotFound();

            return Ok(schedule);
        }

        [HttpGet("performed")]
        public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetPerformedSchedules()
        {
            var schedules = await _feedingScheduleRepository.GetPerformedFeedingSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("time-range")]
        public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetSchedulesByTimeRange([FromBody] TimeRangeRequest request)
        {
            if (request.StartTime > request.EndTime)
                return BadRequest("Start time must be earlier than end time");

            var schedules = await _feedingScheduleRepository.GetFeedingSchedulesByTimeRangeAsync(request.StartTime, request.EndTime);
            return Ok(schedules);
        }

        [HttpPut("{id}/time")]
        public async Task<IActionResult> UpdateScheduleTime(Guid id, UpdateScheduleTimeRequest request)
        {
            try
            {
                var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
                if (schedule == null)
                    return NotFound("Feeding schedule not found");

                var time = new TimeValueObject(request.NewFeedingTime);
                schedule.ChangeSchedule(time);
                await _feedingScheduleRepository.UpdateAsync(schedule);
                
                return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                return NotFound();

            await _feedingScheduleRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelSchedule(Guid id)
        {
            var result = await _feedingService.CancelFeedingScheduleAsync(id);
            
            if (!result)
                return NotFound("Feeding schedule not found");

            return Ok("Feeding schedule cancelled successfully");
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreSchedule(Guid id)
        {
            var result = await _feedingService.RestoreFeedingScheduleAsync(id);
            
            if (!result)
                return NotFound("Feeding schedule not found");

            return Ok("Feeding schedule restored successfully");
        }
        
        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecuteFeeding(Guid id)
        {
            var result = await _feedingService.ExecuteFeedingAsync(id);
            
            if (!result)
                return BadRequest("Failed to execute feeding. The schedule or animal may not exist.");

            return Ok("Feeding executed successfully");
        }
        
    }

    public class CreateScheduleRequest
    {
        public Guid AnimalId { get; set; }
        public TimeOnly FeedingTime { get; set; }
        public string Food { get; set; }
    }

    public class UpdateScheduleTimeRequest
    {
        public TimeOnly NewFeedingTime { get; set; }
    }

    public class TimeRangeRequest
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
} 
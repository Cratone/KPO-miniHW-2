using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZooStatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _statisticsService;

        public ZooStatisticsController(ZooStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("animals/count")]
        public async Task<ActionResult<int>> GetTotalAnimalCount()
        {
            var count = await _statisticsService.GetTotalAnimalCountAsync();
            return Ok(count);
        }

        [HttpGet("animals/count/types")]
        public async Task<ActionResult<List<int>>> GetCountOfAnimalsByTypes()
        {
            var counts = await _statisticsService.GetCountOfAnimalsByTypesAsync();
            return Ok(counts);
        }
        
        [HttpGet("animals/count/health")]
        public async Task<ActionResult<List<int>>> GetCountOfAnimalsByHealthStatus()
        {
            var counts = await _statisticsService.GetCountOfAnimalsByHealthStatusAsync();
            return Ok(counts);
        }

        [HttpGet("enclosures/count")]
        public async Task<ActionResult<int>> GetTotalEnclosuresCount()
        {
            var count = await _statisticsService.GetTotalEnclosuresCountAsync();
            return Ok(count);
        }

        [HttpGet("feeding-schedules/count")]
        public async Task<ActionResult<int>> GetTotalFeedingSchedulesCount()
        {
            var count = await _statisticsService.GetTotalFeedingSchedulesCountAsync();
            return Ok(count);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<ZooDashboardResponse>> GetDashboardStatistics()
        {
            var animalCount = await _statisticsService.GetTotalAnimalCountAsync();
            var healthStats = await _statisticsService.GetCountOfAnimalsByHealthStatusAsync();
            var speciesStats = await _statisticsService.GetCountOfAnimalsByTypesAsync();
            var enclosureCount = await _statisticsService.GetTotalEnclosuresCountAsync();
            var feedingsCount = await _statisticsService.GetTotalFeedingSchedulesCountAsync();

            var dashboard = new ZooDashboardResponse
            {
                TotalAnimals = animalCount,
                HealthStatistics = healthStats,
                SpeciesStatistics = speciesStats,
                EnclosureCount = enclosureCount,
                FeedingsCount = feedingsCount
            };

            return Ok(dashboard);
        }
    }

    public class ZooDashboardResponse
    {
        public int TotalAnimals { get; set; }
        public Dictionary<string, int> HealthStatistics { get; set; }
        public Dictionary<string, int> SpeciesStatistics { get; set; }
        public int EnclosureCount { get; set; }
        public int FeedingsCount { get; set; }
    }
} 
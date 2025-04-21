using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class InMemoryDatabase
    {
        // Singleton instance
        private static readonly Lazy<InMemoryDatabase> _instance = new Lazy<InMemoryDatabase>(() => new InMemoryDatabase());
        
        public static InMemoryDatabase Instance => _instance.Value;
        
        // In-memory collections for entities
        public List<Animal> Animals { get; } = new List<Animal>();
        public List<Enclosure> Enclosures { get; } = new List<Enclosure>();
        public List<FeedingSchedule> FeedingSchedules { get; } = new List<FeedingSchedule>();

        // Private constructor to prevent instantiation
        private InMemoryDatabase() { }

        // Methods to initialize with sample data if needed
        public void SeedSampleData()
        {
            // You can add initial data here if needed for testing
        }
    }
} 
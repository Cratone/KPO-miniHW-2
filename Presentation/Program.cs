using Application;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Domain.Events;

namespace Presentation
{
    /// <summary>
    /// Класс, содержащий точку входа в приложение
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDomainEventHandlers();

            // Register repositories
            builder.Services.AddSingleton<IAnimalRepository, AnimalRepository>();
            builder.Services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            builder.Services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

            // Register application services
            builder.Services.AddScoped<AnimalTransferService>();
            builder.Services.AddScoped<FeedingOrganizationService>();
            builder.Services.AddScoped<ZooStatisticsService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                serviceProvider.ConfigureDomainEventHandlers();
            }

            // Seed initial data for testing
            using (var scope = app.Services.CreateScope())
            {
                SeedInitialData();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Заполняет базу данных начальными тестовыми данными
        /// </summary>
        static void SeedInitialData()
        {
            var db = InMemoryDatabase.Instance;
        
            
            var predatorEnclosure = new Enclosure(AnimalTypeValueObject.Predator, new PositiveIntegerValueObject(20), new PositiveIntegerValueObject(10));
            var herbivoreEnclosure = new Enclosure(AnimalTypeValueObject.Herbivore, new PositiveIntegerValueObject(20), new PositiveIntegerValueObject(10));
            var birdEnclosure = new Enclosure(AnimalTypeValueObject.Bird, new PositiveIntegerValueObject(20), new PositiveIntegerValueObject(10));
            var aquarium = new Enclosure(AnimalTypeValueObject.Aquatic, new PositiveIntegerValueObject(20), new PositiveIntegerValueObject(10));
            
            db.Enclosures.Add(predatorEnclosure);
            db.Enclosures.Add(herbivoreEnclosure);
            db.Enclosures.Add(birdEnclosure);
            db.Enclosures.Add(aquarium);
            
            var lion = new Animal(
                new SpeciesValueObject(new NonEmptyStringValueObject("Lion"), AnimalTypeValueObject.Predator),
                new NonEmptyStringValueObject("Simba"),
                new DateValueObject(new DateTime(2018, 5, 12)), 
                GenderValueObject.Male, 
                new NonEmptyStringValueObject("Meat"),
                HealthStatusValueObject.Healthy);
                
            var zebra = new Animal(
                new SpeciesValueObject(new NonEmptyStringValueObject("Zebra"), AnimalTypeValueObject.Herbivore),
                new NonEmptyStringValueObject("Marty"),
                new DateValueObject(new DateTime(2019, 3, 25)), 
                GenderValueObject.Male, 
                new NonEmptyStringValueObject("Grass"),
                HealthStatusValueObject.Healthy);
                
            var parrot = new Animal(
                new SpeciesValueObject(new NonEmptyStringValueObject("Parrot"), AnimalTypeValueObject.Bird),
                new NonEmptyStringValueObject("Rio"),
                new DateValueObject(new DateTime(2020, 1, 5)), 
                GenderValueObject.Female, 
                new NonEmptyStringValueObject("Seeds"),
                HealthStatusValueObject.Healthy);

            var fish = new Animal(
                new SpeciesValueObject(new NonEmptyStringValueObject("Fish"), AnimalTypeValueObject.Aquatic),
                new NonEmptyStringValueObject("Nemo"),
                new DateValueObject(new DateTime(2021, 2, 14)), 
                GenderValueObject.Male, 
                new NonEmptyStringValueObject("Algae"),
                HealthStatusValueObject.Healthy);
            
            lion.MoveToEnclosure(predatorEnclosure.Id);
            zebra.MoveToEnclosure(herbivoreEnclosure.Id);
            parrot.MoveToEnclosure(birdEnclosure.Id);
            fish.MoveToEnclosure(aquarium.Id);
            
            predatorEnclosure.AddAnimal(lion.Id);
            herbivoreEnclosure.AddAnimal(zebra.Id);
            birdEnclosure.AddAnimal(parrot.Id);
            aquarium.AddAnimal(fish.Id);
            
            db.Animals.Add(lion);
            db.Animals.Add(zebra);
            db.Animals.Add(parrot);
            db.Animals.Add(fish);

            // Create feeding schedules with one-time and recurring schedules
            var lionFeeding = new FeedingSchedule(
                lion.Id, 
                new TimeValueObject(TimeOnly.FromDateTime(DateTime.Now.Date.AddHours(10))), 
                new NonEmptyStringValueObject("Fresh meat"));
            
            var zebraFeeding = new FeedingSchedule(
                zebra.Id,
                new TimeValueObject(TimeOnly.FromDateTime(DateTime.Now.Date.AddHours(9))),
                new NonEmptyStringValueObject("Fresh hay"));
            
            // Recurring schedule for the parrot - feed every 2 days
            var parrotFeeding = new FeedingSchedule(
                parrot.Id,
                new TimeValueObject(TimeOnly.FromDateTime(DateTime.Now.Date.AddHours(8))),
                new NonEmptyStringValueObject("Mixed seeds"));
            
            var fishFeeding = new FeedingSchedule(
                fish.Id,
                new TimeValueObject(TimeOnly.FromDateTime(DateTime.Now.Date.AddHours(7))),
                new NonEmptyStringValueObject("Algae"));

            
            db.FeedingSchedules.Add(lionFeeding);
            db.FeedingSchedules.Add(zebraFeeding);
            db.FeedingSchedules.Add(parrotFeeding);
            db.FeedingSchedules.Add(fishFeeding);
        }
    }
}
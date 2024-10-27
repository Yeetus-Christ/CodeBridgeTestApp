using CodeBridgeTestApp.Data;
using CodeBridgeTestApp.Dtos;
using CodeBridgeTestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeBridgeTestApp.Services
{
    public class DogService : IDogService
    {
        private readonly AppDbContext _context;

        public DogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize)
        {
            IQueryable<Dog> query = _context.Dogs;

            query = attribute.ToLower() switch
            {
                "weight" => order.ToLower() == "desc" ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                "taillength" => order.ToLower() == "desc" ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                _ => query.OrderBy(d => d.Name)
            };

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<string?> CreateDogAsync(DogCreateDto dogDto)
        {
            if (string.IsNullOrEmpty(dogDto.Name) || string.IsNullOrEmpty(dogDto.Color)) 
            {
                return "Name or color cannot be empty.";
            }

            if (await _context.Dogs.AnyAsync(d => d.Name == dogDto.Name))
            {
                return "A dog with the same name already exists.";
            }

            if (dogDto.TailLength <= 0)
            {
                return "Tail length must be a positive number.";
            }
            if (dogDto.Weight <= 0)
            {
                return "Weight must be a positive number.";
            }

            _context.Dogs.Add(new Dog(dogDto));
            await _context.SaveChangesAsync();
            return null;
        }
    }
}

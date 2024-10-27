using CodeBridgeTestApp.Dtos;
using CodeBridgeTestApp.Models;

namespace CodeBridgeTestApp.Services
{
    public interface IDogService
    {
        Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize);

        Task<string?> CreateDogAsync(DogCreateDto dog);
    }
}

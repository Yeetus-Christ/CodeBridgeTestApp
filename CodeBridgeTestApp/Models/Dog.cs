using CodeBridgeTestApp.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CodeBridgeTestApp.Models
{
    public class Dog
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int TailLength { get; set; }

        [Required]
        public int Weight { get; set; }

        public Dog(DogCreateDto dogDto)
        {
            Name = dogDto.Name;
            Color = dogDto.Color;
            TailLength = dogDto.TailLength;
            Weight = dogDto.Weight;
        }

        public Dog()
        {
            
        }
    }
}

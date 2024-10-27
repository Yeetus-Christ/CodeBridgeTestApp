using System.ComponentModel.DataAnnotations;

namespace CodeBridgeTestApp.Dtos
{
    public class DogCreateDto
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public int TailLength { get; set; }

        public int Weight { get; set; }
    }
}

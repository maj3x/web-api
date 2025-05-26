namespace Uyg.API.DTOs
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 
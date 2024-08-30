namespace WebApp.Models
{
    public class InstructorsModel
    {
        public int Id { get; set; }

        public string? name_instructor { get; set; }

        public double rate { get; set; }

        public int available_courses { get; set; }

        public string? image_instructor { get; set; }

        public string? cover_letter { get; set; }
    }
}

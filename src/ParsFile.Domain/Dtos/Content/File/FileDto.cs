namespace ParsFile.Domain.Dtos.Content.File
{
    public class FileDto
    {
        public String? Id { get; set; } = null;

        public String? Name { get; set; } = null;

        public DateTime CreateTime { get; set; }

        public double Price { get; set; }

        public int Downloads { get; set; }

        public bool Accepted { get; set; } = false;
    }
}

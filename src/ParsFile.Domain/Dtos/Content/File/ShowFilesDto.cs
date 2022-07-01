namespace ParsFile.Domain.Dtos.Content.File
{
    public class ShowFilesDto
    {
        public String? Id { get; set; } = null;

        public String? Name { get; set; } = null;

        public String? CreateTime { get; set; } = null;

        public double Price { get; set; }

        public int Downloads { get; set; }

        public bool Accepted { get; set; } = false;

        public String? Username { get; set; } = null;

        public Boolean Deleted { get; set; }
    }
}

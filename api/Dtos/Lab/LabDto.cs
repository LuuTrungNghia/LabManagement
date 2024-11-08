namespace api.Dtos.Lab
{
    public class LabDto
    {
        public int Id { get; set; }
        public string LabName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsBorrowed { get; set; }
    }
}

namespace api.Dtos.Server
{
    public class UpdateUserApprovalDto
    {
        public string UserId { get; set; }
        public bool IsApproved { get; set; }
    }
}

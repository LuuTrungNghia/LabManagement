using System.Collections.Generic;

namespace api.Dtos.User
{
    public class ImportUserDto
    {
        public List<RegisterUserDto> Users { get; set; }
    }
}

using System.Collections.Generic;

namespace api.Dtos.User
{
    public class ImportUserDto
    {
        public required List<RegisterUserDto> Users { get; set; }
    }
}

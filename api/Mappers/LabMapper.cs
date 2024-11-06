using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Lab;
using api.Models;
namespace api.Mappers
{
    public static class LabMapper
    {
        public static LabDto ToDto(this Lab lab)
        {
            return new LabDto
            {
                Id = lab.Id,
                Name = lab.Name,
                Description = lab.Description,
                Location = lab.Location,
            };
        }

        public static Lab ToModel(this CreateLabRequestDto dto)
        {
            return new Lab
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location
            };
        }

        public static void UpdateModel(this Lab lab, UpdateLabRequestDto dto)
        {
            lab.Name = dto.Name;
            lab.Description = dto.Description;
            lab.Location = dto.Location;
        }
    }
}

using SeeSharp.Egzaminer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeSharp.Egzaminer.Shared.DTO
{
    public class TestDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        // Relacja wiele-do-wielu z pytaniami:
        public ICollection<QuestionDto>? Questions { get; set; } = new List<QuestionDto>();
    }
}

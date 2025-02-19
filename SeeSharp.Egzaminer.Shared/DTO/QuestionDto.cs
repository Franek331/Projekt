using SeeSharp.Egzaminer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeSharp.Egzaminer.Shared.DTO
{

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        TrueFalse,
        Open
        // Możesz dodać więcej
    }


    public class QuestionDto
        
    {
        public string Content { get; set; } = default!;
        public QuestionType QuestionType { get; set; }

        // Maksymalna liczba punktów za to pytanie
        public decimal Points { get; set; } = 1;

        // Ewentualnie: Tagowanie, relacje do Testu, itp.
        public ICollection<QuestionAnswer>? Answers { get; set; } = new List<QuestionAnswer>();
        public ICollection<TestDto> Tests { get; set; } = new List<TestDto>();
    }
}

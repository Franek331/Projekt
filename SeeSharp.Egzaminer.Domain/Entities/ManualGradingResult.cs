using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeSharp.Egzaminer.Domain.Entities
{
    public class ManualGradingResult : BaseEntity
    {
        public Guid AnswerSubmittedId { get; set; }  // ID odpowiedzi, której dotyczy ocenianie
        public AnswerSubmitted AnswerSubmitted { get; set; } = default!;

        public decimal Points { get; set; } // Przyznane punkty
        public string? Comment { get; set; } // Komentarz nauczyciela

        // Data oceniania
        public DateTime GradedDate { get; set; }
    }
}

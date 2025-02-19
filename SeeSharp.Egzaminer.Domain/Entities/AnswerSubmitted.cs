namespace SeeSharp.Egzaminer.Domain.Entities
{
    public class AnswerSubmitted : BaseEntity
    {
        public Guid TestSubmissionId { get; set; }
        public TestSubmission TestSubmission { get; set; } = default!;

        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = default!;

        // Dla pytania otwartego można wpisać tekst:
        public string? ProvidedAnswer { get; set; }

        // Dla pytań wielokrotnego/pojedynczego wyboru
        public string? SelectedOptions { get; set; } 

        // Punkty przyznane automatycznie
        public decimal AutoScore { get; set; } = 0;

        // Punkty przyznane ręcznie
        public decimal ManualScore { get; set; } = 0;

        // Komentarz egzaminatora
        public string? ManualComment { get; set; }

        // Kolumna wskazująca, czy odpowiedź została oceniona
        public bool IsManuallyGraded { get; set; } = false;

        public Guid TestPublicationId { get; set; }  // Klucz obcy
        public TestPublication TestPublication { get; set; } = default!;  // Nawigacja do obiektu TestPublication

        // Relacja z ManualGradingResult - kolekcja odpowiedzi ocenianych
        public ICollection<ManualGradingResult> ManualGradingResults { get; set; } = new List<ManualGradingResult>();
    }
}

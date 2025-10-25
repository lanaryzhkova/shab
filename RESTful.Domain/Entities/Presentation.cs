using System.ComponentModel.DataAnnotations;

namespace RESTful.Domain.Entities
{
    public class Presentation
    {
      public int Id { get; set; }

        [Required]
[MaxLength(300)]
 public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
     public string? Abstract { get; set; }

        [Required]
     public int MainSpeakerId { get; set; }
        
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public PresentationStatus Status { get; set; } = PresentationStatus.Pending;

        public int? ModeratedBy { get; set; }
        public DateTime? ModerationDate { get; set; }
        [MaxLength(500)]
        public string? ModerationComment { get; set; }

        // Навигационные свойства
        public Participant MainSpeaker { get; set; } = null!;
        public Participant? Moderator { get; set; }
     public ICollection<CoSpeaker> CoSpeakers { get; set; } = new List<CoSpeaker>();
    }

    public enum PresentationStatus
    {
        Pending = 1,      // Ожидает модерации
      Approved = 2,  // Одобрен
        Rejected = 3    // Отклонен
    }
}

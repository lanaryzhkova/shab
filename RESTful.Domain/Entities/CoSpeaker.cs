using System.ComponentModel.DataAnnotations;

namespace RESTful.Domain.Entities
{
    public class CoSpeaker
    {
        public int Id { get; set; }

  [Required]
        public int PresentationId { get; set; }

   [Required]
 public int ParticipantId { get; set; }

    public bool CanEdit { get; set; } = false;

public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Presentation Presentation { get; set; } = null!;
 public Participant Participant { get; set; } = null!;
    }
}

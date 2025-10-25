using System.ComponentModel.DataAnnotations;

namespace RESTful.Domain.Entities
{
    public class Participant
    {
  public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

    [Required]
  [MaxLength(100)]
  public string LastName { get; set; } = string.Empty;

      [Required]
      [MaxLength(100)]
   public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string WorkPlace { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AcademicDegree { get; set; }

 [MaxLength(100)]
        public string? AcademicTitle { get; set; }

        [Required]
        [EmailAddress]
   [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
    [MaxLength(20)]
  public string Phone { get; set; } = string.Empty;

        [Required]
        public ParticipantRole Role { get; set; }

public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public ICollection<Presentation> PresentationsAsSpeaker { get; set; } = new List<Presentation>();
        public ICollection<CoSpeaker> CoSpeakers { get; set; } = new List<CoSpeaker>();
    }

    public enum ParticipantRole
    {
        Listener = 1,
        Speaker = 2,
        Moderator = 3
    }
}

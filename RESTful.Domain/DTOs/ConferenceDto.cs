using System.ComponentModel.DataAnnotations;
using RESTful.Domain.Entities;

namespace RESTful.Domain.DTOs
{
    public class ParticipantRegistrationDto : IValidatableObject
    {
    [Required(ErrorMessage = "��� �����������")]
 [MaxLength(100)]
   public string FirstName { get; set; } = string.Empty;

[Required(ErrorMessage = "������� �����������")]
     [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "�������� �����������")]
        [MaxLength(100)]
        public string MiddleName { get; set; } = string.Empty;

   [Required(ErrorMessage = "����� ������ �����������")]
        [MaxLength(200)]
    public string WorkPlace { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AcademicDegree { get; set; }

      [MaxLength(100)]
        public string? AcademicTitle { get; set; }

        [Required(ErrorMessage = "Email ����������")]
 [EmailAddress(ErrorMessage = "������������ ������ email")]
        [MaxLength(100)]
   public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "������� ����������")]
   [Phone(ErrorMessage = "������������ ������ ��������")]
   [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

 [Required(ErrorMessage = "���� ��������� �����������")]
   public ParticipantRole Role { get; set; }

  // ��� �����������
        public PresentationDto? Presentation { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
          // ���� ���� - ���������, �� ������ ����������
     if (Role == ParticipantRole.Speaker)
            {
        if (Presentation == null)
    {
            yield return new ValidationResult(
      "���������� � ������� ����������� ��� �����������",
   new[] { nameof(Presentation) });
 }
    else
                {
   if (string.IsNullOrWhiteSpace(Presentation.Title))
            {
               yield return new ValidationResult(
       "�������� ������� �����������",
            new[] { nameof(Presentation) + "." + nameof(Presentation.Title) });
              }
      }
    }
        }
    }

    public class PresentationDto
    {
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Abstract { get; set; }

     public List<int>? CoSpeakerIds { get; set; }
    }

    public class ModerationDto
    {
   [Required(ErrorMessage = "ID ������� ����������")]
        public int PresentationId { get; set; }

        [Required(ErrorMessage = "������� ���������� �����������")]
        public PresentationStatus Status { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
  }

    public class ParticipantResponseDto
    {
      public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
   public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
   public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public string WorkPlace { get; set; } = string.Empty;
        public string? AcademicDegree { get; set; }
        public string? AcademicTitle { get; set; }
        public string Email { get; set; } = string.Empty;
public string Phone { get; set; } = string.Empty;
      public ParticipantRole Role { get; set; }
  public DateTime RegistrationDate { get; set; }
        public List<PresentationResponseDto>? Presentations { get; set; }
    }

    public class PresentationResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Abstract { get; set; }
        public int MainSpeakerId { get; set; }
     public string MainSpeakerName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
public PresentationStatus Status { get; set; }
        public string StatusText => Status switch
        {
 PresentationStatus.Pending => "������� ���������",
            PresentationStatus.Approved => "�������",
    PresentationStatus.Rejected => "��������",
      _ => "����������"
        };
   public int? ModeratedBy { get; set; }
     public string? ModeratorName { get; set; }
        public DateTime? ModerationDate { get; set; }
        public string? ModerationComment { get; set; }
public List<CoSpeakerResponseDto>? CoSpeakers { get; set; }
    public bool CanEdit { get; set; } = true;
    }

    public class CoSpeakerResponseDto
    {
        public int Id { get; set; }
 public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public bool CanEdit { get; set; }
    }
}

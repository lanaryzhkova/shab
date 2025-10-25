using Microsoft.AspNetCore.Mvc;
using RESTful.Domain.DTOs;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;

namespace RESTful.API.Controllers
{
 [ApiController]
    [Route("api/[controller]")]
    public class PresentationsController : ControllerBase
    {
    private readonly IPresentationRepository _presentationRepository;
 private readonly IParticipantRepository _participantRepository;
private readonly ICoSpeakerRepository _coSpeakerRepository;
        private readonly ILogger<PresentationsController> _logger;

        public PresentationsController(
            IPresentationRepository presentationRepository,
    IParticipantRepository participantRepository,
     ICoSpeakerRepository coSpeakerRepository,
  ILogger<PresentationsController> logger)
     {
   _presentationRepository = presentationRepository;
_participantRepository = participantRepository;
    _coSpeakerRepository = coSpeakerRepository;
   _logger = logger;
      }

      [HttpGet]
        public async Task<ActionResult<IEnumerable<PresentationResponseDto>>> GetAll()
        {
  var presentations = await _presentationRepository.GetAllAsync();
 return Ok(presentations.Select(p => MapToResponseDto(p, 0)));
      }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<PresentationResponseDto>>> GetApproved()
        {
 var presentations = await _presentationRepository.GetByStatusAsync(PresentationStatus.Approved);
return Ok(presentations.Select(p => MapToResponseDto(p, 0)));
        }

  [HttpGet("{id}")]
   public async Task<ActionResult<PresentationResponseDto>> GetById(int id, [FromQuery] int? participantId = null)
 {
            var presentation = await _presentationRepository.GetByIdAsync(id, true);
 if (presentation == null)
  {
       return NotFound(new { message = "Доклад не найден" });
 }

    var currentUserId = participantId ?? 0;
     var canEdit = presentation.MainSpeakerId == currentUserId ||
        (presentation.CoSpeakers?.Any(cs => cs.ParticipantId == currentUserId && cs.CanEdit) ?? false);

     // Содокладчики могут только просматривать
       var isCoSpeaker = presentation.CoSpeakers?.Any(cs => cs.ParticipantId == currentUserId) ?? false;
       var isMainSpeaker = presentation.MainSpeakerId == currentUserId;

   if (!isMainSpeaker && !isCoSpeaker && currentUserId > 0)
   {
   return Forbid();
   }

 return Ok(MapToResponseDto(presentation, currentUserId));
    }

     private PresentationResponseDto MapToResponseDto(Presentation presentation, int currentUserId)
        {
     var isMainSpeaker = presentation.MainSpeakerId == currentUserId;
  var coSpeaker = presentation.CoSpeakers?.FirstOrDefault(cs => cs.ParticipantId == currentUserId);
 var canEdit = isMainSpeaker || (coSpeaker?.CanEdit ?? false);

   return new PresentationResponseDto
    {
 Id = presentation.Id,
 Title = presentation.Title,
  Abstract = presentation.Abstract,
 MainSpeakerId = presentation.MainSpeakerId,
    MainSpeakerName = $"{presentation.MainSpeaker?.LastName} {presentation.MainSpeaker?.FirstName} {presentation.MainSpeaker?.MiddleName}",
       CreatedDate = presentation.CreatedDate,
                Status = presentation.Status,
   ModeratedBy = presentation.ModeratedBy,
 ModeratorName = presentation.Moderator != null
      ? $"{presentation.Moderator.LastName} {presentation.Moderator.FirstName} {presentation.Moderator.MiddleName}"
        : null,
     ModerationDate = presentation.ModerationDate,
 ModerationComment = presentation.ModerationComment,
   CoSpeakers = presentation.CoSpeakers?.Select(cs => new CoSpeakerResponseDto
    {
        Id = cs.Id,
   ParticipantId = cs.ParticipantId,
      ParticipantName = $"{cs.Participant?.LastName} {cs.Participant?.FirstName} {cs.Participant?.MiddleName}",
       CanEdit = cs.CanEdit
        }).ToList(),
         CanEdit = canEdit
          };
        }
    }
}

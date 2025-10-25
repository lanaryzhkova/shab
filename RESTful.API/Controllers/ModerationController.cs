using Microsoft.AspNetCore.Mvc;
using RESTful.Domain.DTOs;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;

namespace RESTful.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationController : ControllerBase
    {
private readonly IPresentationRepository _presentationRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ILogger<ModerationController> _logger;

  public ModerationController(
IPresentationRepository presentationRepository,
        IParticipantRepository participantRepository,
     ILogger<ModerationController> logger)
   {
  _presentationRepository = presentationRepository;
            _participantRepository = participantRepository;
         _logger = logger;
      }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<PresentationResponseDto>>> GetPendingPresentations()
        {
            try
        {
     var presentations = await _presentationRepository.GetByStatusAsync(PresentationStatus.Pending);
    return Ok(presentations.Select(MapToResponseDto));
  }
          catch (Exception ex)
        {
    _logger.LogError(ex, "Ошибка при получении докладов на модерации");
            return StatusCode(500, new { message = "Произошла ошибка" });
    }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PresentationResponseDto>>> GetAllForModeration()
        {
       try
       {
                var presentations = await _presentationRepository.GetAllAsync();
      return Ok(presentations.Select(MapToResponseDto));
      }
      catch (Exception ex)
            {
  _logger.LogError(ex, "Ошибка при получении всех докладов");
                return StatusCode(500, new { message = "Произошла ошибка" });
            }
 }

        [HttpPost("moderate")]
        public async Task<ActionResult> ModeratePresentation([FromBody] ModerationDto dto, [FromQuery] int moderatorId)
        {
            try
     {
       // Проверка существования модератора
     var moderator = await _participantRepository.GetByIdAsync(moderatorId);
     if (moderator == null || moderator.Role != ParticipantRole.Moderator)
                {
         return BadRequest(new { message = "Недействительный модератор" });
        }

         // Получение доклада
    var presentation = await _presentationRepository.GetByIdAsync(dto.PresentationId, true);
 if (presentation == null)
                {
            return NotFound(new { message = "Доклад не найден" });
                }

        // Проверка статуса
       if (dto.Status != PresentationStatus.Approved && dto.Status != PresentationStatus.Rejected)
   {
     return BadRequest(new { message = "Недопустимый статус. Используйте Approved или Rejected" });
            }

     // Обновление статуса
       presentation.Status = dto.Status;
       presentation.ModeratedBy = moderatorId;
      presentation.ModerationDate = DateTime.UtcNow;
        presentation.ModerationComment = dto.Comment;

        await _presentationRepository.UpdateAsync(presentation);

      var result = await _presentationRepository.GetByIdAsync(presentation.Id, true);
    return Ok(new 
      { 
          message = dto.Status == PresentationStatus.Approved ? "Доклад одобрен" : "Доклад отклонен",
                  presentation = MapToResponseDto(result!)
     });
   }
      catch (Exception ex)
            {
      _logger.LogError(ex, "Ошибка при модерации доклада");
       return StatusCode(500, new { message = "Произошла ошибка при модерации" });
            }
   }

  [HttpGet("statistics")]
     public async Task<ActionResult> GetModerationStatistics()
        {
  try
            {
             var allPresentations = await _presentationRepository.GetAllAsync();
  var statistics = new
          {
               Total = allPresentations.Count(),
            Pending = allPresentations.Count(p => p.Status == PresentationStatus.Pending),
              Approved = allPresentations.Count(p => p.Status == PresentationStatus.Approved),
               Rejected = allPresentations.Count(p => p.Status == PresentationStatus.Rejected)
         };

    return Ok(statistics);
          }
            catch (Exception ex)
            {
 _logger.LogError(ex, "Ошибка при получении статистики модерации");
     return StatusCode(500, new { message = "Произошла ошибка" });
            }
        }

        private PresentationResponseDto MapToResponseDto(Presentation presentation)
        {
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
     CanEdit = false
   };
}
    }
}

using Microsoft.AspNetCore.Mvc;
using RESTful.Domain.DTOs;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;

namespace RESTful.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
      private readonly IParticipantRepository _participantRepository;
        private readonly IPresentationRepository _presentationRepository;
  private readonly ICoSpeakerRepository _coSpeakerRepository;
     private readonly ILogger<ParticipantsController> _logger;

        public ParticipantsController(
  IParticipantRepository participantRepository,
     IPresentationRepository presentationRepository,
        ICoSpeakerRepository coSpeakerRepository,
 ILogger<ParticipantsController> logger)
        {
    _participantRepository = participantRepository;
            _presentationRepository = presentationRepository;
            _coSpeakerRepository = coSpeakerRepository;
          _logger = logger;
        }

        [HttpPost("register")]
 public async Task<ActionResult<ParticipantResponseDto>> Register([FromBody] ParticipantRegistrationDto dto)
        {
            try
  {
      // Проверка существования email
                if (await _participantRepository.EmailExistsAsync(dto.Email))
       {
 return BadRequest(new { message = "Участник с таким email уже зарегистрирован" });
         }

  // Создание участника
           var participant = new Participant
     {
  FirstName = dto.FirstName,
   LastName = dto.LastName,
            MiddleName = dto.MiddleName,
        WorkPlace = dto.WorkPlace,
      AcademicDegree = dto.AcademicDegree,
            AcademicTitle = dto.AcademicTitle,
          Email = dto.Email,
   Phone = dto.Phone,
       Role = dto.Role
    };

       participant = await _participantRepository.AddAsync(participant);

     // Если докладчик и есть доклад
             if (dto.Role == ParticipantRole.Speaker && dto.Presentation != null)
  {
        var presentation = new Presentation
           {
       Title = dto.Presentation.Title,
        Abstract = dto.Presentation.Abstract,
            MainSpeakerId = participant.Id
          };

         presentation = await _presentationRepository.AddAsync(presentation);

            // Добавление содокладчиков
      if (dto.Presentation.CoSpeakerIds != null && dto.Presentation.CoSpeakerIds.Any())
       {
            foreach (var coSpeakerId in dto.Presentation.CoSpeakerIds)
   {
          if (await _participantRepository.ExistsAsync(coSpeakerId))
  {
           await _coSpeakerRepository.AddAsync(new CoSpeaker
       {
         PresentationId = presentation.Id,
           ParticipantId = coSpeakerId,
        CanEdit = false
          });
  }
               }
         }
          }

         var result = await _participantRepository.GetByIdAsync(participant.Id);
  return Ok(MapToResponseDto(result!));
      }
      catch (Exception ex)
    {
        _logger.LogError(ex, "Ошибка при регистрации участника");
      return StatusCode(500, new { message = "Произошла ошибка при регистрации" });
     }
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantResponseDto>>> GetAll()
        {
            var participants = await _participantRepository.GetAllAsync();
            return Ok(participants.Select(MapToResponseDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantResponseDto>> GetById(int id)
        {
    var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null)
    {
       return NotFound(new { message = "Участник не найден" });
          }

   return Ok(MapToResponseDto(participant));
        }

        [HttpGet("{id}/presentations")]
        public async Task<ActionResult<IEnumerable<PresentationResponseDto>>> GetParticipantPresentations(int id)
 {
            var participant = await _participantRepository.GetByIdAsync(id);
       if (participant == null)
      {
   return NotFound(new { message = "Участник не найден" });
            }

            var presentations = await _presentationRepository.GetBySpeakerIdAsync(id);
            var coPresentations = await _presentationRepository.GetByCoSpeakerIdAsync(id);

          var allPresentations = presentations.Concat(coPresentations).Distinct();

    return Ok(allPresentations.Select(p => MapToPresentationResponseDto(p, id)));
   }

 private ParticipantResponseDto MapToResponseDto(Participant participant)
     {
     return new ParticipantResponseDto
       {
       Id = participant.Id,
       FirstName = participant.FirstName,
  LastName = participant.LastName,
  MiddleName = participant.MiddleName,
            WorkPlace = participant.WorkPlace,
     AcademicDegree = participant.AcademicDegree,
           AcademicTitle = participant.AcademicTitle,
    Email = participant.Email,
       Phone = participant.Phone,
     Role = participant.Role,
   RegistrationDate = participant.RegistrationDate,
        Presentations = participant.PresentationsAsSpeaker?.Select(p => MapToPresentationResponseDto(p, participant.Id)).ToList()
            };
        }

        private PresentationResponseDto MapToPresentationResponseDto(Presentation presentation, int currentUserId)
        {
        var isMainSpeaker = presentation.MainSpeakerId == currentUserId;
            var isCoSpeaker = presentation.CoSpeakers?.Any(cs => cs.ParticipantId == currentUserId) ?? false;

     return new PresentationResponseDto
      {
   Id = presentation.Id,
  Title = presentation.Title,
        Abstract = presentation.Abstract,
        MainSpeakerId = presentation.MainSpeakerId,
     MainSpeakerName = $"{presentation.MainSpeaker?.LastName} {presentation.MainSpeaker?.FirstName} {presentation.MainSpeaker?.MiddleName}",
     CreatedDate = presentation.CreatedDate,
      CoSpeakers = presentation.CoSpeakers?.Select(cs => new CoSpeakerResponseDto
  {
      Id = cs.Id,
ParticipantId = cs.ParticipantId,
      ParticipantName = $"{cs.Participant?.LastName} {cs.Participant?.FirstName} {cs.Participant?.MiddleName}",
     CanEdit = cs.CanEdit
         }).ToList(),
    CanEdit = isMainSpeaker || (isCoSpeaker && presentation.CoSpeakers.First(cs => cs.ParticipantId == currentUserId).CanEdit)
            };
        }
    }
}

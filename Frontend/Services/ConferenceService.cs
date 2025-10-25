using RESTful.Domain.DTOs;
using RESTful.Domain.Entities;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class ConferenceService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ConferenceService> _logger;

        public ConferenceService(HttpClient httpClient, ILogger<ConferenceService> logger)
      {
  _httpClient = httpClient;
        _logger = logger;
        }

  public async Task<ParticipantResponseDto?> RegisterParticipantAsync(ParticipantRegistrationDto dto)
        {
         try
  {
   var response = await _httpClient.PostAsJsonAsync("api/participants/register", dto);
   
         if (response.IsSuccessStatusCode)
       {
        return await response.Content.ReadFromJsonAsync<ParticipantResponseDto>();
    }
         
   var error = await response.Content.ReadAsStringAsync();
         _logger.LogError($"Ошибка регистрации: {error}");
        return null;
      }
            catch (Exception ex)
 {
         _logger.LogError(ex, "Ошибка при регистрации участника");
       return null;
            }
     }

     public async Task<List<ParticipantResponseDto>> GetAllParticipantsAsync()
   {
      try
    {
      return await _httpClient.GetFromJsonAsync<List<ParticipantResponseDto>>("api/participants") ?? new List<ParticipantResponseDto>();
        }
 catch (Exception ex)
      {
_logger.LogError(ex, "Ошибка при получении списка участников");
   return new List<ParticipantResponseDto>();
  }
}

        public async Task<ParticipantResponseDto?> GetParticipantByIdAsync(int id)
 {
 try
  {
        return await _httpClient.GetFromJsonAsync<ParticipantResponseDto>($"api/participants/{id}");
 }
   catch (Exception ex)
  {
    _logger.LogError(ex, $"Ошибка при получении участника {id}");
       return null;
 }
     }

  public async Task<List<PresentationResponseDto>> GetParticipantPresentationsAsync(int participantId)
        {
     try
  {
       return await _httpClient.GetFromJsonAsync<List<PresentationResponseDto>>($"api/participants/{participantId}/presentations") ?? new List<PresentationResponseDto>();
       }
   catch (Exception ex)
   {
    _logger.LogError(ex, $"Ошибка при получении докладов участника {participantId}");
 return new List<PresentationResponseDto>();
  }
 }

  public async Task<List<PresentationResponseDto>> GetAllPresentationsAsync()
        {
        try
   {
    return await _httpClient.GetFromJsonAsync<List<PresentationResponseDto>>("api/presentations") ?? new List<PresentationResponseDto>();
 }
      catch (Exception ex)
 {
    _logger.LogError(ex, "Ошибка при получении списка докладов");
      return new List<PresentationResponseDto>();
 }
        }

        public async Task<List<PresentationResponseDto>> GetApprovedPresentationsAsync()
        {
     try
   {
  return await _httpClient.GetFromJsonAsync<List<PresentationResponseDto>>("api/presentations/approved") ?? new List<PresentationResponseDto>();
   }
   catch (Exception ex)
   {
  _logger.LogError(ex, "Ошибка при получении одобренных докладов");
           return new List<PresentationResponseDto>();
            }
}

        public async Task<PresentationResponseDto?> GetPresentationByIdAsync(int id, int? participantId = null)
        {
      try
   {
  var url = $"api/presentations/{id}";
       if (participantId.HasValue)
    {
      url += $"?participantId={participantId.Value}";
    }
   return await _httpClient.GetFromJsonAsync<PresentationResponseDto>(url);
      }
            catch (Exception ex)
     {
    _logger.LogError(ex, $"Ошибка при получении доклада {id}");
    return null;
     }
        }

 // Методы модерации
   public async Task<List<PresentationResponseDto>> GetPendingPresentationsAsync()
        {
   try
            {
       return await _httpClient.GetFromJsonAsync<List<PresentationResponseDto>>("api/moderation/pending") ?? new List<PresentationResponseDto>();
     }
       catch (Exception ex)
            {
      _logger.LogError(ex, "Ошибка при получении докладов на модерации");
             return new List<PresentationResponseDto>();
          }
}

        public async Task<List<PresentationResponseDto>> GetAllForModerationAsync()
        {
            try
          {
       return await _httpClient.GetFromJsonAsync<List<PresentationResponseDto>>("api/moderation/all") ?? new List<PresentationResponseDto>();
  }
  catch (Exception ex)
   {
       _logger.LogError(ex, "Ошибка при получении всех докладов для модерации");
return new List<PresentationResponseDto>();
  }
        }

        public async Task<bool> ModeratePresentationAsync(int presentationId, PresentationStatus status, string? comment, int moderatorId)
  {
    try
            {
                var dto = new ModerationDto
{
     PresentationId = presentationId,
    Status = status,
 Comment = comment
        };

          var response = await _httpClient.PostAsJsonAsync($"api/moderation/moderate?moderatorId={moderatorId}", dto);
     return response.IsSuccessStatusCode;
     }
       catch (Exception ex)
  {
    _logger.LogError(ex, $"Ошибка при модерации доклада {presentationId}");
     return false;
          }
  }

        public async Task<ModerationStatistics?> GetModerationStatisticsAsync()
        {
 try
  {
            return await _httpClient.GetFromJsonAsync<ModerationStatistics>("api/moderation/statistics");
 }
   catch (Exception ex)
            {
  _logger.LogError(ex, "Ошибка при получении статистики модерации");
       return null;
   }
}
    }

    public class ModerationStatistics
    {
        public int Total { get; set; }
 public int Pending { get; set; }
        public int Approved { get; set; }
 public int Rejected { get; set; }
    }
}

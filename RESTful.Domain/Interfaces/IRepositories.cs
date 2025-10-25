using RESTful.Domain.Entities;

namespace RESTful.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> GetByIdAsync(int id);
        Task<Participant?> GetByEmailAsync(string email);
        Task<IEnumerable<Participant>> GetAllAsync();
        Task<Participant> AddAsync(Participant participant);
        Task UpdateAsync(Participant participant);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }

    public interface IPresentationRepository
    {
        Task<Presentation?> GetByIdAsync(int id, bool includeCoSpeakers = false);
        Task<IEnumerable<Presentation>> GetAllAsync();
        Task<IEnumerable<Presentation>> GetBySpeakerIdAsync(int speakerId);
        Task<IEnumerable<Presentation>> GetByCoSpeakerIdAsync(int participantId);
        Task<IEnumerable<Presentation>> GetByStatusAsync(PresentationStatus status);
        Task<Presentation> AddAsync(Presentation presentation);
        Task UpdateAsync(Presentation presentation);
        Task DeleteAsync(int id);
    }

    public interface ICoSpeakerRepository
    {
        Task<CoSpeaker?> GetByIdAsync(int id);
        Task<IEnumerable<CoSpeaker>> GetByPresentationIdAsync(int presentationId);
        Task<CoSpeaker> AddAsync(CoSpeaker coSpeaker);
        Task DeleteAsync(int id);
        Task<bool> IsCoSpeakerAsync(int presentationId, int participantId);
    }
}

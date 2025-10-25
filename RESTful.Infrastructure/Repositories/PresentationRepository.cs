using Microsoft.EntityFrameworkCore;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;
using RESTful.Infrastructure.Data;

namespace RESTful.Infrastructure.Repositories
{
    public class PresentationRepository : IPresentationRepository
    {
        private readonly Context _context;

public PresentationRepository(Context context)
        {
    _context = context;
 }

    public async Task<Presentation?> GetByIdAsync(int id, bool includeCoSpeakers = false)
    {
      var query = _context.Presentations
       .Include(p => p.MainSpeaker)
            .Include(p => p.Moderator)
  .AsQueryable();

          if (includeCoSpeakers)
     {
    query = query.Include(p => p.CoSpeakers)
     .ThenInclude(cs => cs.Participant);
      }

    return await query.FirstOrDefaultAsync(p => p.Id == id);
   }

      public async Task<IEnumerable<Presentation>> GetAllAsync()
 {
         return await _context.Presentations
      .Include(p => p.MainSpeaker)
          .Include(p => p.Moderator)
   .Include(p => p.CoSpeakers)
        .ThenInclude(cs => cs.Participant)
    .ToListAsync();
    }

      public async Task<IEnumerable<Presentation>> GetBySpeakerIdAsync(int speakerId)
 {
        return await _context.Presentations
         .Include(p => p.MainSpeaker)
                .Include(p => p.Moderator)
          .Include(p => p.CoSpeakers)
  .ThenInclude(cs => cs.Participant)
        .Where(p => p.MainSpeakerId == speakerId)
     .ToListAsync();
}

  public async Task<IEnumerable<Presentation>> GetByCoSpeakerIdAsync(int participantId)
   {
       return await _context.Presentations
    .Include(p => p.MainSpeaker)
  .Include(p => p.Moderator)
       .Include(p => p.CoSpeakers)
      .ThenInclude(cs => cs.Participant)
     .Where(p => p.CoSpeakers.Any(cs => cs.ParticipantId == participantId))
       .ToListAsync();
        }

        public async Task<IEnumerable<Presentation>> GetByStatusAsync(PresentationStatus status)
        {
         return await _context.Presentations
         .Include(p => p.MainSpeaker)
      .Include(p => p.Moderator)
            .Include(p => p.CoSpeakers)
     .ThenInclude(cs => cs.Participant)
     .Where(p => p.Status == status)
   .ToListAsync();
 }

  public async Task<Presentation> AddAsync(Presentation presentation)
        {
     _context.Presentations.Add(presentation);
        await _context.SaveChangesAsync();
     return presentation;
   }

    public async Task UpdateAsync(Presentation presentation)
        {
            _context.Presentations.Update(presentation);
         await _context.SaveChangesAsync();
        }

  public async Task DeleteAsync(int id)
    {
       var presentation = await _context.Presentations.FindAsync(id);
            if (presentation != null)
       {
 _context.Presentations.Remove(presentation);
   await _context.SaveChangesAsync();
     }
   }
    }
}

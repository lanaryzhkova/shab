using Microsoft.EntityFrameworkCore;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;
using RESTful.Infrastructure.Data;

namespace RESTful.Infrastructure.Repositories
{
public class CoSpeakerRepository : ICoSpeakerRepository
    {
    private readonly Context _context;

 public CoSpeakerRepository(Context context)
{
            _context = context;
        }

      public async Task<CoSpeaker?> GetByIdAsync(int id)
        {
 return await _context.CoSpeakers
          .Include(cs => cs.Participant)
           .Include(cs => cs.Presentation)
         .FirstOrDefaultAsync(cs => cs.Id == id);
   }

        public async Task<IEnumerable<CoSpeaker>> GetByPresentationIdAsync(int presentationId)
  {
        return await _context.CoSpeakers
       .Include(cs => cs.Participant)
     .Where(cs => cs.PresentationId == presentationId)
.ToListAsync();
        }

      public async Task<CoSpeaker> AddAsync(CoSpeaker coSpeaker)
        {
   _context.CoSpeakers.Add(coSpeaker);
      await _context.SaveChangesAsync();
          return coSpeaker;
        }

 public async Task DeleteAsync(int id)
        {
       var coSpeaker = await _context.CoSpeakers.FindAsync(id);
  if (coSpeaker != null)
{
      _context.CoSpeakers.Remove(coSpeaker);
        await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCoSpeakerAsync(int presentationId, int participantId)
        {
        return await _context.CoSpeakers
             .AnyAsync(cs => cs.PresentationId == presentationId && cs.ParticipantId == participantId);
        }
    }
}

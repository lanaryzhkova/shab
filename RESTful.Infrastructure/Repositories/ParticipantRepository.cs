using Microsoft.EntityFrameworkCore;
using RESTful.Domain.Entities;
using RESTful.Domain.Interfaces;
using RESTful.Infrastructure.Data;

namespace RESTful.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly Context _context;

     public ParticipantRepository(Context context)
        {
          _context = context;
        }

        public async Task<Participant?> GetByIdAsync(int id)
        {
     return await _context.Participants
   .Include(p => p.PresentationsAsSpeaker)
      .ThenInclude(pr => pr.CoSpeakers)
   .ThenInclude(cs => cs.Participant)
      .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Participant?> GetByEmailAsync(string email)
        {
 return await _context.Participants
        .FirstOrDefaultAsync(p => p.Email == email);
     }

     public async Task<IEnumerable<Participant>> GetAllAsync()
        {
         return await _context.Participants
     .Include(p => p.PresentationsAsSpeaker)
    .ToListAsync();
     }

     public async Task<Participant> AddAsync(Participant participant)
        {
            _context.Participants.Add(participant);
      await _context.SaveChangesAsync();
            return participant;
  }

   public async Task UpdateAsync(Participant participant)
        {
       _context.Participants.Update(participant);
     await _context.SaveChangesAsync();
  }

        public async Task DeleteAsync(int id)
        {
         var participant = await _context.Participants.FindAsync(id);
   if (participant != null)
            {
   _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
    }
 }

 public async Task<bool> ExistsAsync(int id)
        {
  return await _context.Participants.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
     return await _context.Participants.AnyAsync(p => p.Email == email);
        }
    }
}

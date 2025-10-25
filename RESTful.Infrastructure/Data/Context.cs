using Microsoft.EntityFrameworkCore;
using RESTful.Domain.Entities;

namespace RESTful.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
     { 
        }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<CoSpeaker> CoSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
      {
            // Настройка Participant
       modelBuilder.Entity<Participant>(entity =>
      {
             entity.HasKey(e => e.Id);
      entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
     entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
 entity.Property(e => e.MiddleName).IsRequired().HasMaxLength(100);
          entity.Property(e => e.WorkPlace).IsRequired().HasMaxLength(200);
              entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
          entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
    entity.Property(e => e.Role).IsRequired();

 entity.HasIndex(e => e.Email).IsUnique();
   });

         // Настройка Presentation
  modelBuilder.Entity<Presentation>(entity =>
            {
    entity.HasKey(e => e.Id);
      entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
       entity.Property(e => e.Abstract).HasMaxLength(2000);
     entity.Property(e => e.Status).IsRequired();
       entity.Property(e => e.ModerationComment).HasMaxLength(500);
    
          entity.HasOne(e => e.MainSpeaker)
 .WithMany(p => p.PresentationsAsSpeaker)
       .HasForeignKey(e => e.MainSpeakerId)
       .OnDelete(DeleteBehavior.Restrict);

     entity.HasOne(e => e.Moderator)
          .WithMany()
               .HasForeignKey(e => e.ModeratedBy)
         .OnDelete(DeleteBehavior.Restrict);
    });

            // Настройка CoSpeaker
modelBuilder.Entity<CoSpeaker>(entity =>
  {
   entity.HasKey(e => e.Id);
       
         entity.HasOne(e => e.Presentation)
        .WithMany(p => p.CoSpeakers)
              .HasForeignKey(e => e.PresentationId)
   .OnDelete(DeleteBehavior.Cascade);
      
   entity.HasOne(e => e.Participant)
 .WithMany(p => p.CoSpeakers)
      .HasForeignKey(e => e.ParticipantId)
   .OnDelete(DeleteBehavior.Restrict);
             
  entity.HasIndex(e => new { e.PresentationId, e.ParticipantId }).IsUnique();
       });
 }
    }
}

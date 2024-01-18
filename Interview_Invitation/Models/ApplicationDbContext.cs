using Interview_Invitation.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace interview_invitation.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Interviewee> Interviewees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships using Fluent API if needed
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Interview)
                .WithMany(i => i.Subjects)
                .HasForeignKey(s => s.InterviewId);

            modelBuilder.Entity<Interviewee>()
                .HasOne(i => i.Interview)
                .WithMany(i => i.Interviewees)
                .HasForeignKey(i => i.InterviewId);

            // Add other configurations as needed
        }
    }


}


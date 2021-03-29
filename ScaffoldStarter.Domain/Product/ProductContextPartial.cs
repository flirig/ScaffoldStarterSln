using Microsoft.EntityFrameworkCore;

namespace ScaffoldStarter.Domain.Product
{
    public partial class ProductContext
    {
        // Uncomment it
        /*partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>(entity =>
            {
                entity.HasOne(bug => bug.Developer)
                    .WithMany(developer => developer.Bugs)
                    .HasForeignKey(bug => bug.DeveloperId);
            });
        }*/
    }
}
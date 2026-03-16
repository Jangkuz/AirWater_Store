using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirWaterStore.API.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ==== USER ====
        builder.Entity<User>(user =>
        {
            user.ToTable("Users"); // Optional: Rename default Identity table

            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            //user.Property(u => u.Password)
            //    .IsRequired();

            //user.Property(u => u.Role)
            //    .IsRequired();

            user.Property(u => u.IsBan)
                .HasDefaultValue(false);
        });

        // ==== CHATROOM ====
        //builder.Entity<ChatRoom>(chat =>
        //{
        //    chat.HasKey(c => c.ChatRoomId);

        //    chat.HasOne(c => c.Customer)
        //        .WithMany(u => u.ChatRoomCustomers)
        //        .HasForeignKey(c => c.CustomerId)
        //        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        //    chat.HasOne(c => c.Staff)
        //        .WithMany(u => u.ChatRoomStaffs)
        //        .HasForeignKey(c => c.StaffId)
        //        .OnDelete(DeleteBehavior.SetNull); // Staff can be nullable
        //});

        // ==== MESSAGE ====
        //builder.Entity<Message>(msg =>
        //{
        //    msg.HasKey(m => m.MessageId);

        //    msg.Property(m => m.Content)
        //        .IsRequired()
        //        .HasMaxLength(1000);

            //msg.Property(m => m.SentAt)
            //    .HasDefaultValueSql("GETDATE()");

        //    msg.HasOne(m => m.ChatRoom)
        //        .WithMany(c => c.Messages)
        //        .HasForeignKey(m => m.ChatRoomId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    msg.HasOne(m => m.User)
        //        .WithMany(u => u.Messages)
        //        .HasForeignKey(m => m.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //});
    }

}

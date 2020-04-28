using Corporate.Chat.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corporate.Chat.API.Context
{
    public class UserChatConfig : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> modelBuilder)
        {
            //Property Configurations
            modelBuilder.ToTable("USER_CHAT");

            modelBuilder.HasKey(x => x.UserChatId)
            .IsClustered(true);

            modelBuilder
                .Property(s => s.UserChatId)
                .HasColumnName("USER_CHAT_ID")
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder
                .Property(s => s.ConnectionId)
                .HasColumnName("CONNECTION_ID")
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            modelBuilder
                .Property(s => s.Name)
                .HasColumnName("NAME")
                .HasMaxLength(120)
                .HasColumnType("varchar(120)");

            modelBuilder.Property(t => t.CreatedDate)
               .IsRequired()
               .HasColumnName("CREATED_DATE")
               .HasColumnType("DateTime")
               .HasDefaultValueSql("GetDate()");




        }

    }
}
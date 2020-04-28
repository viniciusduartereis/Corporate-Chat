using Corporate.Chat.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corporate.Chat.API.Context
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> modelBuilder)
        {
            modelBuilder.ToTable("MESSAGE");

            modelBuilder.HasKey(o => o.MessageId)
            .IsClustered(true);

            modelBuilder
                .Property(s => s.MessageId)
                .HasColumnName("MESSAGE_ID")
                .UseIdentityColumn()
                .IsRequired();

            modelBuilder
                .Property(s => s.Name)
                .HasColumnName("NAME")
                .HasMaxLength(120)
                .HasColumnType("varchar(120)")
                .IsRequired();

            modelBuilder
                .Property(s => s.Text)
                .HasColumnName("TEXT")
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsRequired();

            modelBuilder
                .Property(s => s.CreatedDate)
                .HasColumnType("DateTime")
                .HasColumnName("CREATED_DATE")
                .HasDefaultValueSql("GetDate()")
                .IsRequired();

        }


    }
}
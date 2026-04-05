using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SharpPet.Infrastructure.Persistence;

namespace SharpPet.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
public sealed class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        modelBuilder.Entity("SharpPet.Domain.Entities.Project", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Projects");

            b.Navigation("Tasks");
        });

        modelBuilder.Entity("SharpPet.Domain.Entities.TaskItem", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<string>("Description")
                .HasMaxLength(4000)
                .HasColumnType("TEXT");

            b.Property<DateTimeOffset?>("DueDate")
                .HasColumnType("TEXT");

            b.Property<Guid>("ProjectId")
                .HasColumnType("TEXT");

            b.Property<int>("Status")
                .HasColumnType("INTEGER");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("ProjectId");

            b.ToTable("TaskItems");

            b.HasOne("SharpPet.Domain.Entities.Project", "Project")
                .WithMany("Tasks")
                .HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Project");
        });
    }
}

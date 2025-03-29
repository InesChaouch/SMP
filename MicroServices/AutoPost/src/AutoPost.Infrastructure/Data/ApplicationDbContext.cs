using System.Reflection;
using AutoPost.Application.Common.Interfaces;
using AutoPost.Domain.Entities;
using AutoPost.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutoPost.Domain.Entities.PostEntities;
using AutoPost.Domain.Entities.ChannelEntities;
using System.Reflection.Emit;
using AutoPost.Domain.Entities.Enums.PostFormat;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoPost.Domain.Entities.TemplateEntities;

namespace AutoPost.Infrastructure.Data;

public class ApplicationDbContext :  IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();


    public DbSet<ChannelProfile> ChannelProfiles => Set<ChannelProfile>();
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<TextFormat> TextFormats { get; set; }
    public DbSet<ImageFormat> ImageFormats { get; set; }
    public DbSet<CarrouselFormat> CarrouselFormats { get; set; }

    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateSlide> TemplateSlides { get; set; }
    public DbSet<TemplateSlideElement> TemplateSlideElements { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        return SaveChangesAsync(CancellationToken.None);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder
           .Entity<Channel>()
               .HasMany(c => c.ChannelProfiles)
               .WithOne(cp => cp.Channel)
               .HasForeignKey(cp => cp.ChannelName)
               .OnDelete(DeleteBehavior.Cascade);

        builder
            .Entity<Post>()
                .HasOne(p => p.PostFormat)
                .WithMany(pf => pf.Posts)
                .HasForeignKey(p => p.PostFormatId)
                .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Post>()
                .HasOne(p => p.PostStatus)
                .WithMany(ps => ps.Posts)
                .HasForeignKey(p => p.PostStatusId)
                .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<Post>()
                .HasOne(p => p.ChannelProfile)
                .WithMany(cp => cp.Posts)
                .HasForeignKey(p => p.ChannelProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PostFormat>()
    .UseTphMappingStrategy()
    .HasDiscriminator<string>("PostFormatType")
    .HasValue<TextFormat>("Text")
    .HasValue<ImageFormat>("Image")
    .HasValue<CarrouselFormat>("Carrousel");

        builder.Entity<Template>()
                  .HasMany(t => t.TemplateSlides)
                  .WithOne(ts => ts.Template)
                  .HasForeignKey(ts => ts.TemplateId)
                  .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TemplateSlide>()
            .HasMany(ts => ts.Elements)
            .WithOne(e => e.TemplateSlide)
            .HasForeignKey(e => e.TemplateSlideId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Template>()
    .HasMany(t => t.Posts)
    .WithOne(p => p.Template)
    .HasForeignKey(p => p.TemplateId)
    .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

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

namespace AutoPost.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
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

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
    }

}

using SMP.Domain.Constants;
using SMP.Domain.Entities;
using SMP.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SMP.Domain.Entities.TemplateEntities;
using SMP.Domain.Entities.Enums.PostFormat;

namespace SMP.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                //await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Templates", "Template1");

        var slide1 = File.ReadAllBytes(Path.Combine(basePath, "slide1.png")); 
        var slide2 = File.ReadAllBytes(Path.Combine(basePath, "slide2.png"));
        var slide3 = File.ReadAllBytes(Path.Combine(basePath, "slide3.png"));
        var slide4 = File.ReadAllBytes(Path.Combine(basePath, "slide4.png"));
        var slide5 = File.ReadAllBytes(Path.Combine(basePath, "slide5.png"));


        var template = new Template
        {
            Format = "Document",
            BrandDomain = "IT",
            TemplateSlides = new List<TemplateSlide>
    {
        new TemplateSlide
        {
            SlideNumber = 1,
            BackgroundType = "Image",
            BackgroundValue = slide1,
            LogoValue = [],
            Elements = new List<TemplateSlideElement>
            {
                new TemplateSlideElement
                {
                    TextFont = "Bosk",
                    TextSize = 144,
                    TextWeight = "Bold",
                    TextColor = "#374c7a",
                    HorizontalAlignment = "Center",
                    VerticalAlignment = "Center",
                    XPadding = 0,
                    YPadding = 0,
                    ElementType = "Title",
                    Text = ""
                }
            }
        },
        new TemplateSlide
        {
            SlideNumber = 3,
            BackgroundType = "Image",
            BackgroundValue = slide3,
            LogoValue = [],
            Elements = new List<TemplateSlideElement>
            {
                new TemplateSlideElement
                {
                    TextFont = "Arial",
                    TextSize = 20,
                    TextWeight = "SemiBold",
                    TextColor = "#333333",
                    HorizontalAlignment = "Left",
                    VerticalAlignment = "Top",
                    XPadding = 15,
                    YPadding = 15,
                    ElementType = "Subtitle",
                    Text = "Introduction"
                },
                new TemplateSlideElement
                {
                    TextFont = "Arial",
                    TextSize = 16,
                    TextWeight = "Regular",
                    TextColor = "#555555",
                    HorizontalAlignment = "Left",
                    VerticalAlignment = "Center",
                    XPadding = 15,
                    YPadding = 10,
                    ElementType = "Content",
                    Text = "Voici une description du sujet..."
                }
            }
        },
        new TemplateSlide
        {
            SlideNumber = 4,
            BackgroundType = "Image",
            BackgroundValue = slide4,
            LogoValue = [],
            Elements = new List<TemplateSlideElement>
            {
                new TemplateSlideElement
                {
                    TextFont = "Arial",
                    TextSize = 20,
                    TextWeight = "SemiBold",
                    TextColor = "#333333",
                    HorizontalAlignment = "Left",
                    VerticalAlignment = "Top",
                    XPadding = 15,
                    YPadding = 15,
                    ElementType = "Subtitle",
                    Text = "Détails"
                },
                new TemplateSlideElement
                {
                    TextFont = "Arial",
                    TextSize = 16,
                    TextWeight = "Regular",
                    TextColor = "#555555",
                    HorizontalAlignment = "Left",
                    VerticalAlignment = "Center",
                    XPadding = 15,
                    YPadding = 10,
                    ElementType = "Content",
                    Text = "Plus d'informations techniques ici..."
                }
            }
        },
        new TemplateSlide
        {
            SlideNumber = 5,
            BackgroundType = "Image",
            BackgroundValue = slide5,
            LogoValue = [],
            Elements = new List<TemplateSlideElement>
            {
                new TemplateSlideElement
                {
                    TextFont = "Arial",
                    TextSize = 18,
                    TextWeight = "Regular",
                    TextColor = "#000000",
                    HorizontalAlignment = "Center",
                    VerticalAlignment = "Bottom",
                    XPadding = 10,
                    YPadding = 20,
                    ElementType = "Contact",
                    Text = "Contactez-nous : contact@example.com"
                }
            }
        }
    }
        }; 

        _context.Templates.Add(template);
        await _context.SaveChangesAsync();

    }
}

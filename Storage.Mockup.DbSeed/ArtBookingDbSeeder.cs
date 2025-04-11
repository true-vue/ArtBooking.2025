using Microsoft.Extensions.DependencyInjection;
using Business.Model.Data;
using Business.Model.Entities.Organizations;
using Business.Model.Entities.Events;
using Business.Model.Entities.Events.Enums;
using Business.Application.Services.Users.Dtos;
using Business.Application.Services.Users;
using Business.Model.Entities.Users;

namespace Storage.Mockup.DbSeed;

public class ArtBookingDbSeeder
{
    public static void SeedNow(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ArtBookingDbContext>();

        if (!context.Users.Any())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            userService.CreateUser(new CreateUserDto
            {
                Username = "admin",
                Email = "admin@admin.com",
                Password = "admin01",
                Role = UserRole.MasterAdmin,
                FirstName = "Admin",
                LastName = "Admin",
            });
        }

        // Check if data already exists to avoid duplicates
        // For in memory database not necessary by for true database scenario is better to check.
        if (!context.ArtOrganizations.Any())
        {
            var orgBagatela = new ArtOrganization
            {
                Name = "Teatr Bagatela",
                Description = "Jeden z najbardziej znanych teatrów w Krakowie, działający od 1919 roku.",
                Kind = OrganizationKind.Theatre,
                Email = "kontakt@bagatela.pl",
                PhoneNumber = "+48 12 422 66 77",
                Website = "https://www.bagatela.pl",
                Street = "ul. Karmelicka",
                AddressNumber = "6",
                Town = "Kraków",
                PostalCode = "31-128",
                Country = "Polska",
                LogoUrl = "https://bagatela.pl/wp-content/themes/bagatela/images/logo.svg",
                CreatedAt = new DateTime(2024, 1, 1, 10, 0, 0),
                CreatedById = 1,
                UpdatedAt = new DateTime(2024, 1, 1, 10, 0, 0),
                UpdatedById = 1
            };

            var orgMultikino = new ArtOrganization
            {
                Name = "Multikino Kraków",
                Description = "Sieć kin oferująca szeroki repertuar filmowy, również premiery kinowe.",
                Kind = OrganizationKind.Cinema,
                Email = "krakow@multikino.pl",
                PhoneNumber = "+48 22 123 45 67",
                Website = "https://multikino.pl/pl/wszystkie-kina/krakow",
                Street = "ul. Dobrego Pasterza",
                AddressNumber = "128",
                Town = "Kraków",
                PostalCode = "31-416",
                Country = "Polska",
                LogoUrl = "https://multikino.pl/-/jssmedia/multikino/img/logo-multikino.svg",
                CreatedAt = new DateTime(2024, 1, 2, 11, 0, 0),
                CreatedById = 1,
                UpdatedAt = new DateTime(2024, 1, 2, 11, 0, 0),
                UpdatedById = 1
            };

            var orgMuzeumNarodowe = new ArtOrganization
            {
                Name = "Muzeum Narodowe w Krakowie",
                Description = "Największe muzeum w Polsce, z bogatymi zbiorami sztuki polskiej i zagranicznej.",
                Kind = OrganizationKind.Gallery,
                Email = "info@mnk.pl",
                PhoneNumber = "+48 12 433 55 00",
                Website = "https://mnk.pl",
                Street = "al. 3 Maja",
                AddressNumber = "1",
                Town = "Kraków",
                PostalCode = "30-062",
                Country = "Polska",
                LogoUrl = "https://logowik.com/content/uploads/images/muzeum-narodowe-w-krakowie-new2995.logowik.com.webp",
                CreatedAt = new DateTime(2024, 1, 3, 12, 0, 0),
                CreatedById = 1,
                UpdatedAt = new DateTime(2024, 1, 3, 12, 0, 0),
                UpdatedById = 1
            };

            var orgMuzeumPowstania = new ArtOrganization
            {
                Name = "Muzeum Powstania Warszawskiego",
                Description = "Interaktywne muzeum poświęcone Powstaniu Warszawskiemu z 1944 roku.",
                Kind = OrganizationKind.Gallery,
                Email = "muzeum@1944.pl",
                PhoneNumber = "+48 22 539 79 05",
                Website = "https://www.1944.pl",
                Street = "ul. Grzybowska",
                AddressNumber = "79",
                Town = "Warszawa",
                PostalCode = "00-844",
                Country = "Polska",
                LogoUrl = "https://www.1944.pl/img/logo-color.svg",
                CreatedAt = new DateTime(2024, 1, 4, 13, 0, 0),
                CreatedById = 1,
                UpdatedAt = new DateTime(2024, 1, 4, 13, 0, 0),
                UpdatedById = 1
            };

            context.ArtOrganizations.AddRange(
                orgBagatela,
                orgMultikino,
                orgMuzeumNarodowe,
                orgMuzeumPowstania
            );

            orgBagatela.Events = new List<ArtEvent>
            {
                new ArtEvent
                {
                    Name = "Szalone nożyczki",
                    Description = "Kultowa komedia kryminalna, w której widzowie stają się detektywami.",
                    ArtOrganizationId = 1,
                    Status = EventStatus.TicketSalesOpen,
                    Category = EventCategory.Theater,
                    ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/szalone-nozyczki.jpg",
                    CreatedAt = new DateTime(2024, 1, 5, 14, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 5, 14, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Pijacy",
                    Description = "Spektakl na podstawie komedii Franciszka Bohomolca, ukazujący społeczne obyczaje XVIII wieku.",
                    ArtOrganizationId = 1,
                    Status = EventStatus.TicketSalesOpen,
                    Category = EventCategory.Theater,
                    ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/pijacy.jpg",
                    CreatedAt = new DateTime(2024, 1, 6, 15, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 6, 15, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Bajki dla niegrzecznych",
                    Description = "Muzyczne przedstawienie oparte na opowieściach Heinricha Hoffmanna, pełne czarnego humoru.",
                    ArtOrganizationId = 1,
                    Status = EventStatus.Published,
                    Category = EventCategory.Theater,
                    ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/bajki-dla-niegrzecznych.jpg",
                    CreatedAt = new DateTime(2024, 1, 7, 16, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 7, 16, 0, 0),
                    UpdatedById = 1
                }
            };

            orgMultikino.Events = new List<ArtEvent>
            {
                new ArtEvent
                {
                    Name = "Amator",
                    Description = "Thriller z Ramim Malekiem w roli kryptologa CIA, który po stracie żony w zamachu terrorystycznym postanawia odnaleźć sprawców.",
                    ArtOrganizationId = 2,
                    Status = EventStatus.TicketSalesOpen,
                    Category = EventCategory.Film,
                    ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/amator.jpg",
                    CreatedAt = new DateTime(2024, 1, 8, 17, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 8, 17, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Kaiju No. 8: Mission Recon",
                    Description = "Japoński film animowany opowiadający o walce z potężnymi potworami zagrażającymi ludzkości.",
                    ArtOrganizationId = 2,
                    Status = EventStatus.TicketSalesOpen,
                    Category = EventCategory.Film,
                    ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/kaiju-no-8.jpg",
                    CreatedAt = new DateTime(2024, 1, 9, 18, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 9, 18, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Surfer",
                    Description = "Nadchodząca premiera filmu o pasji do surfingu i pokonywaniu własnych słabości.",
                    ArtOrganizationId = 2,
                    Status = EventStatus.Published,
                    Category = EventCategory.Film,
                    ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/surfer.jpg",
                    CreatedAt = new DateTime(2024, 1, 10, 19, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 10, 19, 0, 0),
                    UpdatedById = 1
                }
            };

            orgMuzeumNarodowe.Events = new List<ArtEvent>
            {
                new ArtEvent
                {
                    Name = "Boznańska. Kameralnie",
                    Description = "Wystawa poświęcona Oldze Boznańskiej, ukazująca jej życie i twórczość.",
                    ArtOrganizationId = 3,
                    Status = EventStatus.Published,
                    Category = EventCategory.Exhibition,
                    ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/boznanska.jpg",
                    CreatedAt = new DateTime(2024, 1, 11, 20, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 11, 20, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Łysogórski zwierzyniec",
                    Description = "Ekspozycja prezentująca ceramikę ze Spółdzielni 'Kamionka' z Łysej Góry, ukazującą bogactwo fauny i flory.",
                    ArtOrganizationId = 3,
                    Status = EventStatus.Published,
                    Category = EventCategory.Exhibition,
                    ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/lysogorski-zwierzyniec.jpg",
                    CreatedAt = new DateTime(2024, 1, 12, 21, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 12, 21, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Królestwo roślin i zwierząt – opowieść i trudy poznania",
                    Description = "Wystawa ukazująca relacje między człowiekiem a światem przyrody, poprzez sztukę i naukę.",
                    ArtOrganizationId = 3,
                    Status = EventStatus.Published,
                    Category = EventCategory.Exhibition,
                    ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/krolestwo-roslin-i-zwierzat.jpg",
                    CreatedAt = new DateTime(2024, 1, 13, 22, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 13, 22, 0, 0),
                    UpdatedById = 1
                }
            };

            orgMuzeumPowstania.Events = new List<ArtEvent>
            {
                new ArtEvent
                {
                    Name = "Rzeczywiste. 80 wyjątkowych przedmiotów z Powstania Warszawskiego",
                    Description = "Wystawa prezentująca unikalne przedmioty z czasów Powstania Warszawskiego, ukazujące codzienne życie powstańców.",
                    ArtOrganizationId = 4,
                    Status = EventStatus.Published,
                    Category = EventCategory.Exhibition,
                    ImageUrl = "https://www.1944.pl/media/cache/resolve/cover/uploads/media/default/0001/02/rzeczywiste.jpg",
                    CreatedAt = new DateTime(2024, 1, 14, 23, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 14, 23, 0, 0),
                    UpdatedById = 1
                },
                new ArtEvent
                {
                    Name = "Wystawa stała",
                    Description = "Stała ekspozycja muzeum, przenosząca zwiedzających w realia okupowanej Warszawy i czasów Powstania Warszawskiego z 1944 roku.",
                    ArtOrganizationId = 4,
                    Status = EventStatus.Published,
                    Category = EventCategory.Exhibition,
                    ImageUrl = "https://www.1944.pl/media/cache/resolve/cover/uploads/media/default/0001/02/wystawa-stala.jpg",
                    CreatedAt = new DateTime(2024, 1, 15, 0, 0, 0),
                    CreatedById = 1,
                    UpdatedAt = new DateTime(2024, 1, 15, 0, 0, 0),
                    UpdatedById = 1
                }
            };

            context.SaveChanges();
        }
    }
}

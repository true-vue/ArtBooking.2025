using Microsoft.Extensions.DependencyInjection;
using Business.Model.Entities.Organizations;
using Business.Model.Entities.Events;
using Business.Model.Entities.Events.Enums;
using Business.Application.Services.Users.Dtos;
using Business.Application.Services.Users;
using Business.Model.Entities.Users;
using Business.Application.Services.Organizations;
using Business.Application.Services.Events;
using Business.Application.DTOs.Organizations;
using Business.Application.DTOs.Events;

namespace Business.Application.Storage;

public class ArtBookingDbSeeder
{
    public static void SeedNow(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var organizationService = scope.ServiceProvider.GetRequiredService<IArtOrganizationService>();
        var eventService = scope.ServiceProvider.GetRequiredService<IArtEventService>();

        if (!userService.HasUsers())
        {
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
        if (!organizationService.HasOrganizations())
        {
            var orgBagatela = organizationService.CreateOrganization(new CreateArtOrganizationDto
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
                LogoUrl = "https://bagatela.pl/wp-content/themes/bagatela/images/logo.svg"
            });

            var orgMultikino = organizationService.CreateOrganization(new CreateArtOrganizationDto
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
                LogoUrl = "https://multikino.pl/-/jssmedia/multikino/img/logo-multikino.svg"
            });

            var orgMuzeumNarodowe = organizationService.CreateOrganization(new CreateArtOrganizationDto
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
                LogoUrl = "https://logowik.com/content/uploads/images/muzeum-narodowe-w-krakowie-new2995.logowik.com.webp"
            });

            var orgMuzeumPowstania = organizationService.CreateOrganization(new CreateArtOrganizationDto
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
                LogoUrl = "https://www.1944.pl/img/logo-color.svg"
            });

            // Create events for Bagatela
            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Szalone nożyczki",
                Description = "Kultowa komedia kryminalna, w której widzowie stają się detektywami.",
                Status = EventStatus.TicketSalesOpen,
                Category = EventCategory.Theater,
                ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/szalone-nozyczki.jpg"
            }, orgBagatela.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Pijacy",
                Description = "Spektakl na podstawie komedii Franciszka Bohomolca, ukazujący społeczne obyczaje XVIII wieku.",
                Status = EventStatus.TicketSalesOpen,
                Category = EventCategory.Theater,
                ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/pijacy.jpg"
            }, orgBagatela.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Bajki dla niegrzecznych",
                Description = "Muzyczne przedstawienie oparte na opowieściach Heinricha Hoffmanna, pełne czarnego humoru.",
                Status = EventStatus.Published,
                Category = EventCategory.Theater,
                ImageUrl = "https://bagatela.pl/wp-content/uploads/2023/11/bajki-dla-niegrzecznych.jpg"
            }, orgBagatela.ArtOrganizationId);

            // Create events for Multikino
            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Amator",
                Description = "Thriller z Ramim Malekiem w roli kryptologa CIA, który po stracie żony w zamachu terrorystycznym postanawia odnaleźć sprawców.",
                Status = EventStatus.TicketSalesOpen,
                Category = EventCategory.Film,
                ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/amator.jpg"
            }, orgMultikino.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Kaiju No. 8: Mission Recon",
                Description = "Japoński film animowany opowiadający o walce z potężnymi potworami zagrażającymi ludzkości.",
                Status = EventStatus.TicketSalesOpen,
                Category = EventCategory.Film,
                ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/kaiju-no-8.jpg"
            }, orgMultikino.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Surfer",
                Description = "Nadchodząca premiera filmu o pasji do surfingu i pokonywaniu własnych słabości.",
                Status = EventStatus.Published,
                Category = EventCategory.Film,
                ImageUrl = "https://multikino.pl/media/cache/resolve/film_poster/uploads/media/default/0001/02/surfer.jpg"
            }, orgMultikino.ArtOrganizationId);

            // Create events for Muzeum Narodowe
            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Boznańska. Kameralnie",
                Description = "Wystawa poświęcona Oldze Boznańskiej, ukazująca jej życie i twórczość.",
                Status = EventStatus.Published,
                Category = EventCategory.Exhibition,
                ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/boznanska.jpg"
            }, orgMuzeumNarodowe.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Łysogórski zwierzyniec",
                Description = "Ekspozycja prezentująca ceramikę ze Spółdzielni 'Kamionka' z Łysej Góry, ukazującą bogactwo fauny i flory.",
                Status = EventStatus.Published,
                Category = EventCategory.Exhibition,
                ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/lysogorski-zwierzyniec.jpg"
            }, orgMuzeumNarodowe.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Królestwo roślin i zwierząt – opowieść i trudy poznania",
                Description = "Wystawa ukazująca relacje między człowiekiem a światem przyrody, poprzez sztukę i naukę.",
                Status = EventStatus.Published,
                Category = EventCategory.Exhibition,
                ImageUrl = "https://mnk.pl/media/cache/resolve/cover/uploads/media/default/0001/02/krolestwo-roslin-i-zwierzat.jpg"
            }, orgMuzeumNarodowe.ArtOrganizationId);

            // Create events for Muzeum Powstania
            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Rzeczywiste. 80 wyjątkowych przedmiotów z Powstania Warszawskiego",
                Description = "Wystawa prezentująca unikalne przedmioty z czasów Powstania Warszawskiego, ukazujące codzienne życie powstańców.",
                Status = EventStatus.Published,
                Category = EventCategory.Exhibition,
                ImageUrl = "https://www.1944.pl/media/cache/resolve/cover/uploads/media/default/0001/02/rzeczywiste.jpg"
            }, orgMuzeumPowstania.ArtOrganizationId);

            eventService.CreateEvent(new CreateArtEventDto
            {
                Name = "Wystawa stała",
                Description = "Stała ekspozycja muzeum, przenosząca zwiedzających w realia okupowanej Warszawy i czasów Powstania Warszawskiego z 1944 roku.",
                Status = EventStatus.Published,
                Category = EventCategory.Exhibition,
                ImageUrl = "https://www.1944.pl/media/cache/resolve/cover/uploads/media/default/0001/02/wystawa-stala.jpg"
            }, orgMuzeumPowstania.ArtOrganizationId);
        }
    }
}

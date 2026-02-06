using ApiProjectCamp.WebUI.Dtos.NotificationDtos;
using ApiProjectCamp.WebUI.Dtos.ReservationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjectCamp.WebUI.ViewComponents.AdminLayoutNavbarViewComponent
{
    public class _NavbarNotificationAdminLayoutComponentPartiel : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _NavbarNotificationAdminLayoutComponentPartiel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("api");
                var responseMessage = await client.GetAsync("api/Reservations");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultReservationDto>>(jsonData);

                    // Son 10 rezervasyonu al - sadece beklemedeki rezervasyonları
                    var recentReservations = values
                        .Where(x => x.ReservationStatus == "Onay Bekliyor" || x.ReservationStatus == "Beklemede")
                        .OrderByDescending(x => x.ReservationDate)
                        .ThenByDescending(x => x.ReservationTime)
                        .Take(10)
                        .ToList();

                    return View(recentReservations);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste döndür
                Console.WriteLine($"Rezervasyon bildirimleri alınırken hata: {ex.Message}");
            }

            return View(new List<ResultReservationDto>());
        }
    }
}


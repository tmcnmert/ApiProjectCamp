using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.ViewComponents.HomePageViewComponents
{
    public class _HomePageStatisticsComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _HomePageStatisticsComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client1 = _httpClientFactory.CreateClient("api");
            var responseMessage1 = await client1.GetAsync("/api/Statistics/ProductCount/");
            var jsonData1 = await responseMessage1.Content.ReadAsStringAsync();
            ViewBag.v1 = jsonData1;

            var client2 = _httpClientFactory.CreateClient("api");
            var responseMessage2 = await client2.GetAsync("/api/Statistics/ReservationCount/");
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            ViewBag.v2 = jsonData2;

            var client3 = _httpClientFactory.CreateClient("api");
            var responseMessage3 = await client3.GetAsync("/api/Statistics/ChefCount/");
            var jsonData3 = await responseMessage3.Content.ReadAsStringAsync();
            ViewBag.v3 = jsonData3;

            var client4 = _httpClientFactory.CreateClient("api");
            var responseMessage4 = await client4.GetAsync("/api/Statistics/TotalGuestCount/");
            var jsonData4 = await responseMessage4.Content.ReadAsStringAsync();
            ViewBag.v4 = jsonData4;

            return View();
        }
    }
}

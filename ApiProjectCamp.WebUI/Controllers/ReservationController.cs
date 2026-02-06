using ApiProjectCamp.WebUI.Dtos.ReservationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ReservationList()
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.GetAsync("api/Reservations");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultReservationDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReservationDetail(int id)
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.GetAsync($"/api/Reservations/GetReservation?id={id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var reservation = JsonConvert.DeserializeObject<GetReservationByIdDto>(jsonData);
                return View(reservation);
            }

            return RedirectToAction("ReservationList");
        }

        private async Task<bool> UpdateReservationStatus(int id, string status)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("api");

                var getResponse = await client.GetAsync($"/api/Reservations/GetReservation?id={id}");
                if (!getResponse.IsSuccessStatusCode) return false;

                var reservationJson = await getResponse.Content.ReadAsStringAsync();
                var reservation = JsonConvert.DeserializeObject<UpdateReservationDto>(reservationJson);

                reservation.ReservationStatus = status;

                var jsonData = JsonConvert.SerializeObject(reservation);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync("/api/Reservations", stringContent);

                return responseMessage.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReservation(int id)
        {
            var success = await UpdateReservationStatus(id, "Onaylandı");

            if (success)
            {
                TempData["SuccessMessage"] = "Rezervasyon başarıyla onaylandı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rezervasyon onaylanırken bir hata oluştu.";
            }

            return RedirectToAction("ReservationList");
        }

        [HttpPost]
        public async Task<IActionResult> HoldReservation(int id)
        {
            var success = await UpdateReservationStatus(id, "Beklemede");

            if (success)
            {
                TempData["SuccessMessage"] = "Rezervasyon beklemeye alındı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rezervasyon bekletilirken bir hata oluştu.";
            }

            return RedirectToAction("ReservationList");
        }

        [HttpPost]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var success = await UpdateReservationStatus(id, "İptal Edildi");

            if (success)
            {
                TempData["SuccessMessage"] = "Rezervasyon iptal edildi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rezervasyon iptal edilirken bir hata oluştu.";
            }

            return RedirectToAction("ReservationList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.DeleteAsync($"/api/Reservations?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
                return RedirectToAction("ReservationList");
            }
            TempData["ErrorMessage"] = "Rezervasyon silinirken bir hata oluştu.";
            return RedirectToAction("ReservationList");
        }

    }
}
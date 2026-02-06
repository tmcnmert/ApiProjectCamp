using ApiProjectCamp.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static ApiProjectCamp.WebUI.Controllers.AIController;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _openAiApiKey;
        private readonly string _huggingFaceApiKey;
        public MessageController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _openAiApiKey = configuration["OPENAI_API_KEY"];
            _huggingFaceApiKey = configuration["HUGGINGFACE_API_KEY"];
        }


        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.GetAsync("api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient("api");
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("/api/Messages/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            return View();
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.DeleteAsync($"/api/Messages?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.GetAsync($"/api/Messages/GetMessage?id={id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var client = _httpClientFactory.CreateClient("api");
            var jsonData = JsonConvert.SerializeObject(updateMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("/api/Messages", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AnswerMessageWithOpenAI(int id, string prompt)
        {
            var client = _httpClientFactory.CreateClient("api");
            var responseMessage = await client.GetAsync($"/api/Messages/GetMessage?id={id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
            prompt = value.MessageDetails;

            using var client2 = new HttpClient();
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role="system",
                        content="Sen bir restoran için kullanıcıların göndermiş oldukları mesajları detaylı ve olabildiğince olumlu, müşteri memnuniyeti gözeten cevaplar veren bir yapay zeka aracısın. Amacımız kullanıcı tarafından girilen mesajlara en olumlu ve en mantıklı cevapları sunabilmek."
                    },
                    new
                    {
                        role="user",
                        content=prompt
                    }
                },
                temperature = 0.5
            };
            var response = await client2.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.answerAI = content;
            }
            else
            {
                ViewBag.answerAI = "Bir hata oluştu!" + response.StatusCode;
            }
            return View(value);
        }
        [HttpGet]
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _huggingFaceApiKey);
            try
            {
                var translateRequestBody = new
                {
                    inputs = createMessageDto.MessageDetails
                };
                var translateJson = System.Text.Json.JsonSerializer.Serialize(translateRequestBody);
                var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

                var translateResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
                var translateResponseString = await translateResponse.Content.ReadAsStringAsync();
                string englishText = createMessageDto.MessageDetails;
                if (translateResponseString.TrimStart().StartsWith("["))
                {
                    var translateDoc = JsonDocument.Parse(translateResponseString);
                    englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    //ViewBag.v = englishText;
                }
                var toxicityRequestBody = new
                {
                    inputs = englishText
                };
                var toxicJson = System.Text.Json.JsonSerializer.Serialize(toxicityRequestBody);
                var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
                var toxicResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/unitary/toxic-bert", toxicContent);
                var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();
                if (toxicResponseString.TrimStart().StartsWith("["))
                {
                    var toxicDoc = JsonDocument.Parse(toxicResponseString);
                    foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                    {
                        string label = item.GetProperty("label").GetString();
                        double score = item.GetProperty("score").GetDouble();
                        if (score > 0.5)
                        {
                            createMessageDto.Status = "Toksik Mesaj";
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(createMessageDto.Status))
                {
                    createMessageDto.Status = "Mesaj Alındı";
                }
            }
            catch
            {
                createMessageDto.Status = "Onay Bekliyor";
            }

            var client2 = _httpClientFactory.CreateClient("api");
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client2.PostAsync("/api/Messages/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageList");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var client = _httpClientFactory.CreateClient("api");

            var responseMessage = await client.GetAsync($"/api/Messages/GetMessage?id={id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);

                if (message.Status == "Yeni" || message.Status == "Mesaj Alındı" || !message.IsRead)
                {
                    await UpdateMessageStatus(id, "Okundu", true);
                    message.Status = "Okundu";
                    message.IsRead = true;
                }

                return View(message);
            }

            return RedirectToAction("MessageList");
        }
        private async Task<bool> UpdateMessageStatus(int id, string status, bool isRead)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("api");


                var getMessage = await client.GetAsync($"/api/Messages/GetMessage?id={id}");
                if (!getMessage.IsSuccessStatusCode) return false;

                var messageJson = await getMessage.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<UpdateMessageDto>(messageJson);


                message.Status = status;
                message.IsRead = isRead;

                var jsonData = JsonConvert.SerializeObject(message);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync("/api/Messages", stringContent);

                return responseMessage.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


        [HttpGet]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await UpdateMessageStatus(id, "Okundu", true);

            if (success)
            {
                TempData["SuccessMessage"] = "Mesaj okundu olarak işaretlendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Mesaj güncellenirken bir hata oluştu.";
            }

            return RedirectToAction("MessageDetail", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> SendReply(int id, string replyContent)
        {
            try
            {
                var success = await UpdateMessageStatus(id, "Yanıtlandı", true);

                if (success)
                {
                    TempData["SuccessMessage"] = "Yanıt başarıyla gönderildi ve mesaj 'Yanıtlandı' olarak işaretlendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Yanıt gönderilirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("MessageDetail", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> SendAIReply(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("api");
                var responseMessage = await client.GetAsync($"/api/Messages/GetMessage?id={id}");
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);

                // OpenAI ile yanıt oluştur
                using var client2 = new HttpClient();
                client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new
                        {
                            role="system",
                            content="Sen bir restoran için kullanıcıların göndermiş oldukları mesajları detaylı ve olabildiğince olumlu, müşteri memnuniyeti gözeten cevaplar veren bir yapay zeka aracısın. Amacımız kullanıcı tarafından girilen mesajlara en olumlu ve en mantıklı cevapları sunabilmek."
                        },
                        new
                        {
                            role="user",
                            content=message.MessageDetails
                        }
                    },
                    temperature = 0.5
                };

                var response = await client2.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                    var aiReply = result.choices[0].message.content;


                    await UpdateMessageStatus(id, "Yanıtlandı", true);

                    TempData["SuccessMessage"] = "AI yanıtı başarıyla oluşturuldu ve gönderildi.";
                    TempData["AIReply"] = aiReply;
                }
                else
                {
                    TempData["ErrorMessage"] = "AI yanıtı oluşturulurken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("MessageDetail", new { id = id });
        }


        [HttpGet]
        public async Task<IActionResult> ArchiveMessage(int id)
        {
            var success = await UpdateMessageStatus(id, "Arşivlendi", true);

            if (success)
            {
                TempData["SuccessMessage"] = "Mesaj arşivlendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Mesaj arşivlenirken bir hata oluştu.";
            }

            return RedirectToAction("MessageList");
        }


        [HttpGet]
        public async Task<IActionResult> MarkAsPending(int id)
        {
            var success = await UpdateMessageStatus(id, "Beklemede", true);

            if (success)
            {
                TempData["SuccessMessage"] = "Mesaj beklemede olarak işaretlendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Mesaj güncellenirken bir hata oluştu.";
            }

            return RedirectToAction("MessageDetail", new { id = id });
        }


        [HttpGet]
        public async Task<IActionResult> CloseMessage(int id)
        {
            var success = await UpdateMessageStatus(id, "Kapandı", true);

            if (success)
            {
                TempData["SuccessMessage"] = "Mesaj kapatıldı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Mesaj kapatılırken bir hata oluştu.";
            }

            return RedirectToAction("MessageList");
        }
    }
}
using System.Web;
using Common.DTO.Message;
using Common.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Adapters;
using Services.Communication;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Services.Storage;
using Services.Users; 

namespace Controlers.Controllers
{
    public class CommunicationController : Controller
    {
        private readonly ICommunicationService _communicationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IUserManagementService _userManagementService; 

        public CommunicationController(
            ICommunicationService communicationService,
            IBlobStorageService blobStorageService,
            IUserManagementService userManagementService)
        {
            _communicationService = communicationService;
            _blobStorageService = blobStorageService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Chat(string senderId, string receiverId)
        {
            var decodedSender = HttpUtility.UrlDecode(senderId);
            var decodedReceiver = HttpUtility.UrlDecode(receiverId);

            var messagesResponse = await _communicationService.GetByConversationAsync(decodedSender, decodedReceiver, 1, 50);
            var messages = new List<MessageResponseDto>();
            if (messagesResponse?.Object != null)
            {
                foreach (var msg in messagesResponse.Object)
                    messages.Add(await MessageAdapter.ConvertModelToResponseDto(msg));
            }

            ViewBag.CurrentUserId = decodedSender;
            ViewBag.ReceiverId = decodedReceiver;
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessages(
            [FromForm] string? content,
            [FromForm] string? senderId,
            [FromForm] string? receiverId,
            [FromForm] string? fromEmail,
            [FromForm] string? toEmail,
            IFormFile? attachment)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Treść wiadomości nie może być pusta.";
                return RedirectToAction("Index", "User");
            }

            string resolvedSenderId = null;
            if (!string.IsNullOrWhiteSpace(senderId))
            {
                resolvedSenderId = HttpUtility.UrlDecode(senderId);
            }
            else if (!string.IsNullOrWhiteSpace(fromEmail))
            {
                var userResp = await _userManagementService.GetUserByEmail(fromEmail.Trim());
                if (!userResp.Status || userResp.Object == null)
                {
                    TempData["Error"] = $"Nie znaleziono użytkownika o emailu '{fromEmail}'.";
                    return RedirectToAction("Index", "User");
                }
                resolvedSenderId = userResp.Object.Id;
            }

            string resolvedReceiverId = null;
            if (!string.IsNullOrWhiteSpace(receiverId))
            {
                resolvedReceiverId = HttpUtility.UrlDecode(receiverId);
            }
            else if (!string.IsNullOrWhiteSpace(toEmail))
            {
                var userResp = await _userManagementService.GetUserByEmail(toEmail.Trim());
                if (!userResp.Status || userResp.Object == null)
                {
                    TempData["Error"] = $"Nie znaleziono użytkownika o emailu '{toEmail}'.";
                    return RedirectToAction("Index", "User");
                }
                resolvedReceiverId = userResp.Object.Id;
            }

            if (string.IsNullOrWhiteSpace(resolvedSenderId) || string.IsNullOrWhiteSpace(resolvedReceiverId))
            {
                TempData["Error"] = "Musisz podać nadawcę i odbiorcę (jako id lub email).";
                return RedirectToAction("Index", "User");
            }

            var messageDto = new MessageRequestDto
            {
                SenderId = resolvedSenderId,
                ReciverId = resolvedReceiverId,
                Content = content
            };

            var msgModel = await MessageAdapter.ConvertRequestDtoToModel(messageDto);

            if (attachment != null && attachment.Length > 0 && attachment.ContentType.StartsWith("image"))
            {
                var blob = await _blobStorageService.UploadAsync(attachment);
                msgModel.AttachmentUrl = blob.ImageUri;
            }

            await _communicationService.SendMessagesAsync(resolvedSenderId, new[] { resolvedReceiverId }, msgModel);

            return await LoadUserFormViewAsync(resolvedSenderId);
        }
        
        private async Task<IActionResult> LoadUserFormViewAsync(string id)
        {
            // Pobierz użytkownika
            var userResponse = await _userManagementService.GetUserByIdAsync(id); 
            if (!userResponse.Status || userResponse.Object == null)
            {
                TempData["Error"] = "Nie znaleziono użytkownika.";
                return RedirectToAction("Index", "User");
            }

            // Pobierz wiadomości użytkownika
            var messagesResponse = await _communicationService.GetUserMessagesAsync(userResponse.Object.Id, null, null, 1, 50);
            var messages = messagesResponse.Status ? messagesResponse.Object : new List<Message>();

            var messagesDto = new List<MessageResponseDto>();
            foreach (var msg in messages)
            {
                // Konwersja modelu do DTO (bez zmian w adapterze)
                var messageDto = await MessageAdapter.ConvertModelToResponseDto(msg);

                // Pobierz email nadawcy
                var senderUser = await _userManagementService.GetUserByIdAsync(messageDto.SenderId);
                messageDto.SenderEmail = senderUser.Status && senderUser.Object != null
                    ? senderUser.Object.Email
                    : messageDto.SenderId; // fallback na ID jeśli brak użytkownika

                // Zamień AttachmentUrl na pełny URL z Bloba, jeśli istnieje
                if (!string.IsNullOrEmpty(messageDto.AttachmentUrl))
                {
                    messageDto.AttachmentUrl = await _blobStorageService.GetBlobUrl(messageDto.AttachmentUrl);
                }

                messagesDto.Add(messageDto);
            }

            Console.WriteLine($"InboxMessages count: {messagesDto.Count}");
            ViewBag.InboxMessages = messagesDto;

            // Użytkownik do formularza (nie lista, tylko pojedynczy user)
            var userDto = await UserAdapter.ConvertModelToResponseDto(userResponse.Object);
            return View("UserForm", userDto); 
        }


    }
}

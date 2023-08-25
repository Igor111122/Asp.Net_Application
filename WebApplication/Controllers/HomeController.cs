using Antlr.Runtime.Tree;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private static List<Message> allMessages = new List<Message>();


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(string text, string User)
        {
            var MessagesFilePath = System.IO.Path.GetFullPath(@"../Data/messages.txt");
            if (text == "") {
                return Json(new { success = false, message = "Введіть текст повідомлення" });
            }

            foreach (char c in User)
            {
                if (char.IsLetter(c))
                {
                    return Json(new { success = false, message = "В поле UserID можна вводити лише цифри" });
                }
            }

            if (User == "")
            {
                User = "1";
            }
            var message = new Message
            {
                UserId = int.Parse(User),
                Text = text,
                Date = DateTime.Now
            };


            var userMessageCounts = new Dictionary<int, int>();

            foreach (var message1 in allMessages)
            {
                if (!userMessageCounts.ContainsKey(message1.UserId))
                {
                    userMessageCounts[message1.UserId] = 1;
                }
                else
                {
                    userMessageCounts[message1.UserId]++;
                }
            }
            foreach (var message1 in userMessageCounts)
            {
                if (message1.Key == int.Parse(User)) {
                    if (message1.Value >= 10) {
                        var oldestMessageFromUser = allMessages.FirstOrDefault(m => m.UserId == int.Parse(User));
                        allMessages.Remove(oldestMessageFromUser);
                        allMessages.Add(message);
                        return Json(new { success = true, message = "Повідомлення надіслано успішно." });
                    }
                }
            }

            if (allMessages.Count >= 20)
            {
                allMessages.Remove(allMessages[0]);
                allMessages.Add(message);
                return Json(new { success = true, message = "Повідомлення надіслано успішно." });
            }
            else {
                allMessages.Add(message);
                return Json(new { success = true, message = "Повідомлення надіслано успішно." });
            }
            
        }

        public ActionResult Sort(string Type_Sort)
        {

            IEnumerable<Message> messages;

            if (Type_Sort== "Id") {
                messages = allMessages.OrderBy(m => m.UserId);
            }
            else {
                messages = allMessages.OrderBy(m => m.Date);
            }

            return PartialView("_MessageList", messages);
        }

        public ActionResult Show_User_massage(string UserID)
        {
            
            if (UserID == "") {
                return PartialView("_MessageList", allMessages);
            }
            IEnumerable<Message> messages;

            messages = allMessages.Where(item => item.UserId == Convert.ToInt32(UserID)).ToList();
            return PartialView("_MessageList", messages);
        }

        public ActionResult GetMessages()
        {

            var messages = allMessages.OrderByDescending(m => m.Date);
            return PartialView("_MessageList", messages);
        }
        /*
        private List<Message> LoadAllMessagesFromFile()
        {
            var MessagesFilePath = Directory.GetCurrentDirectory() + "../Data/messages.txt";
            var allMessages = new List<Message>();

            var lines = System.IO.File.ReadAllLines(MessagesFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    var message = new Message
                    {
                        UserId = int.Parse(parts[0]),
                        Text = parts[1],
                        Date = DateTime.Parse(parts[2])
                    };
                    allMessages.Add(message);
                }
            }

            return allMessages.OrderByDescending(m => m.Date).ToList();
        }
        */
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
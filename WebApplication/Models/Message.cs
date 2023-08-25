using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Message
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
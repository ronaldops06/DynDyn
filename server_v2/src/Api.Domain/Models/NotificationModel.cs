using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Models
{
    public class NotificationModel
    {
        public string Title { get; set; }
        public object Body { get; set; }
    }
}
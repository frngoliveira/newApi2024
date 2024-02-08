using System.ComponentModel.DataAnnotations.Schema;

namespace FRN.Domain.Notifications
{
    public class DomainNotification
    {
        public string Message { get; private set; }
        public DomainNotification(string message)
        {
            Message = message;
        }
    }
}

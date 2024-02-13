using FRN.Domain._2._1_Interface;
using FRN.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FRN.API.Controllers
{
    public class ApiController : ControllerBase
    {
        private readonly IDomainNotificationHandler _notifications;
        public ApiController(IDomainNotificationHandler notifications)
        {
            _notifications = notifications;
        }
        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();
        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }
        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Message)
            });
        }
        protected new IActionResult Response(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorInvalidModel(modelState);
            return Response();
        }
        protected void NotifyErrorInvalidModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage: error.Exception.Message;
                NotifyError(errorMsg);
            }
        }
        protected void NotifyModelStateError()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }
        protected void NotifyError(string messsage)
        {
            _notifications.Handle(new DomainNotification(messsage));
        }

    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.interprocess.api.SignalR.Models
{
    /// <summary>
    ///     Message
    /// </summary>
    public sealed class MessageModel
    {
        /// <summary>
        ///     Message UID
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     Message text
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        ///     Message received time UTC
        /// </summary>
        [Required]
        public DateTime ReceviedUtc { get; set; }

        [Required] public UserInfoModel User { get; set; }

        public MessageModel(string message, DateTime receivedUtc, UserInfoModel user)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            ReceviedUtc = receivedUtc;
            User = user;
        }
    }
}
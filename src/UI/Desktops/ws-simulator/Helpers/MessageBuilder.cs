using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws.simulator.Helpers
{
    public static class MessageBuilder
    {
        public static string BuildMessageForCurrentTime(string message, DateTime dt)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-------------");
            sb.Append(message);
            sb.Append(".");
            sb.Append("\n");
            sb.Append("Time:");
            sb.Append(dt);
            sb.Append("\n");
            sb.AppendLine("-------------");
            sb.Append("\n");
            return sb.ToString();
        }

        public static string BuildMessage(string message)
        {
            string dt = DateTime.Now.ToString(); 
            var sb = new StringBuilder();
            sb.AppendLine("-------------");
            sb.Append(message);
            sb.Append(".");
            sb.Append("\n");
            sb.Append("System Time:");
            sb.Append(dt);
            sb.Append("\n");
            sb.AppendLine("-------------");
            sb.Append("\n");
            return sb.ToString();
        }

        public static string BuildMessageFromException(string message)
        {
            string dt = DateTime.Now.ToString(); 
            var sb = new StringBuilder();
            sb.AppendLine("-------------");
            sb.Append(message);
            sb.Append("\n");
            sb.Append("Please configure");
            sb.Append("\n");
            sb.Append("Time:");
            sb.Append(dt);
            sb.Append("\n");
            sb.AppendLine("-------------");
            sb.Append("\n");
            return sb.ToString();
        }
    }
}

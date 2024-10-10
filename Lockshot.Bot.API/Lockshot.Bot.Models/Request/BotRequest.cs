using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockshot.Bot.Models.Request
{
    public class BotRequest
    {

        public BotType BotType { get; set; }

        public string? Message { get; set; }

    }
}

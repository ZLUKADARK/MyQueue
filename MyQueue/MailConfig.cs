using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue
{
    public class MailConfig
    {
        public string Domain { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.WPFClientChat
{
    public class CharMessage
    {
        //Id - користувача в чаті
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required string Text { get; set; }
        public required string Photo { get; set; }
    }
}

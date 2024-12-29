using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Polished_Version.MVVM.Model
{
    public class MessagesModel
    {
        public string MessageContent { get; set; }
        public int Message_Number { get; set; }
        public string Message_SenderName { get; set; }
        public int Message_SenderNumber { get; set; }
        public string Message_RecieverName { get; set; }
        public int Message_RecieverNumber { get; set; }
        public bool IsSender { get; set; }

        public string timePosted { get; set; }
    }
}

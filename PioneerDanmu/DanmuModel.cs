using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PioneerDanmu
{
    public class DanmuModel
    {
        public long ID { get; set; }

        public string AvatorUrl { get; set; }

        public string GroupCard { get; set; }

        public string NickName { get; set; }

        public string QQNumber { get; set; }

        public string DisplayName =>
            string.IsNullOrWhiteSpace(GroupCard) ? (string.IsNullOrWhiteSpace(NickName) ? QQNumber : NickName) : GroupCard;

        public IMessageBase[] EncodedContent { get; set; }

    }
}

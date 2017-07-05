using System;
using Structure.Enums;

namespace Structure.Entities
{
    public class Talk : Paragraph<TalkStyle>
    {
        public String Character { get; set; }
    }
}

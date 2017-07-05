using System;
using Structure.Enums;

namespace Structure.Entities
{
    public class Talk : Paragraph<eTalkStyle>
    {
        public String Character { get; set; }
    }
}

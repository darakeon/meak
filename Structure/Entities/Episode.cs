using System;
using System.Collections.Generic;
using Structure.Enums;

namespace Structure.Entities
{
    public class Episode
    {
        public Episode()
        {
            Season = new Season();

            ParagraphTypeList = new List<ParagraphType>();
            TalkList = new List<Talk>();
            TellerList = new List<Teller>();
        }





        public String ID { get; set; }

        public String Title { get; set; }

        public Season Season { get; set; }

        public IList<ParagraphType> ParagraphTypeList { get; set; }
        public IList<Talk> TalkList { get; set; }
        public IList<Teller> TellerList { get; set; }



        public override String ToString()
        {
            return ID;
        }

        public Int32 ParagraphCount { get { return ParagraphTypeList.Count; } }
    }
}

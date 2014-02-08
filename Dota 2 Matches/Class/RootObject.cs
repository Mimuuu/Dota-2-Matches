using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dota_2_Matches
{
    public class RootObject
    {
        public List<Started> started { get; set; }
        public List<Upcoming> upcoming { get; set; }
        public List<Ended> ended { get; set; }
    }
}

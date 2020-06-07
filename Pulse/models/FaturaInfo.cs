using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class FaturaInfo
    {
        public string nr { get; set; }
        public string price { get; set; }
        public string date { get; set; }

        public FaturaInfo(String nr, String price, String date)
        {
            this.nr = nr;
            this.price = price;
            this.date = date.Substring(0, 10);
        }
    }
}

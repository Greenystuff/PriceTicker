using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTicker.Models
{
    internal class PcGamer
    {

        private string name;

        private string webLink;

        public string getName()
        {
            return name;
        }

        public void setname(string Name)
        {
            this.name = Name;
        }

        public string getWebLink()
        {
            return webLink;
        }

        public void setWebLink(string WebLink)
        {
            this.webLink = WebLink;
        }

    }
}

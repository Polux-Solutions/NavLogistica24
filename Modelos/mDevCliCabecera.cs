using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NavLogistica24.Modelos
{
    public class mDevCliCabecera
    {
        public string accion { get; set; }
        public string almace { get; set; }
        public string cliente { get; set; }
        public string cliext { get; set; }
        public string co { get; set; }
        public string codext { get; set; }
        public string codigo { get; set; }
        public string fecdev { get; set; }
        public string fecha { get; set; }
        public string muelle { get; set; }
        public string numdoc { get; set; }
        public string propie { get; set; }
        public string recogi { get; set; }
        public string sitcab { get; set; }
        public string totbul { get; set; }
        public string vpl { get; set; }

        public ObservableCollection<mDevCliLineas> Lineas { get; set; }
    }
}

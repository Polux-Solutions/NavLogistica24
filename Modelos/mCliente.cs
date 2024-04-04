using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NavLogistica24.Modelos
{
    public class mCliente
    {
        public String accion { get; set; }
        public String codext { get; set; }
        public String codigo { get; set; }
        public String codpos { get; set; }
        public String codrut { get; set; }
        public String copais { get; set; }
        public String direcc { get; set; }
        public String fecha { get; set; }
        public String locali { get; set; }
        public String nombre { get; set; }
        public String nomcto { get; set; }
        public String numfax { get; set; }
        public String ordrut { get; set; }
        public String percon { get; set; }
        public String provin { get; set; }
        public String razsoc { get; set; }
        public String telefo { get; set; }
        public String tipcli { get; set; }
        public String tipcon { get; set; }
        public bool OK { get; set; }
        public String Error { get; set; }

        public void NOK(ref mCliente Cliente, string TextoError)
        {
            Cliente.OK = false;
            Cliente.Error = TextoError; 
        }
    }

    
}

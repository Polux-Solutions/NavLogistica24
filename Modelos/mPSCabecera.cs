using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace NavLogistica24.Modelos
{
    public class mPSCabecera
    {
        public String accion { get; set; }
        public String almace { get; set; }
        public String client { get; set; }
        public String cliext { get; set; }
        public String copais { get; set; }
        public String codpos { get; set; }
        public String codrut { get; set; }
        public String cross { get; set; }
        public String denemp { get; set; }
        public String dirent { get; set; }
        public String divped { get; set; }
        public String fecha { get; set; }
        public String fecser { get; set; }
        public String fectra { get; set; }
        public String guicar { get; set; }
        public String impadl { get; set; }
        public String indexp { get; set; }
        public String indnap { get; set; }
        public String indurg { get; set; }
        public String locali { get; set; }
        public String lotsec { get; set; }
        public String matric { get; set; }
        public String numfic { get; set; }
        public String numreg { get; set; }
        public String observ { get; set; }
        public String ordcar { get; set; }
        public String ordrut { get; set; }
        public String pedcom { get; set; }
        public String pedext { get; set; }
        public String pedido { get; set; }
        public String percon { get; set; }
        public String pesped { get; set; }
        public String propie { get; set; }
        public String provin { get; set; }
        public String sitped { get; set; }
        public String supedi { get; set; }
        public String telefo { get; set; }
        public String tipagr { get; set; }
        public String tipped { get; set; }
        public String volped { get; set; }

        public ObservableCollection<mPSLineas> Lineas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace NavLogistica24.Modelos
{
    public class mOCCabecera
    {
        public String accion { get; set; }
        public String albara { get; set; }
        public String almace { get; set; }
        public String fecasi { get; set; }
        public String fecha { get; set; }
        public String fecpre { get; set; }
        public String pedext { get; set; }
        public String pedido { get; set; }
        public String proext { get; set; }
        public String propie { get; set; }
        public String provee { get; set; }
        public String sitped { get; set; }
        public String tipped { get; set; }
        public String totlin { get; set; }

        public ObservableCollection<mOCLineas> Lineas { get; set; }
    }
}

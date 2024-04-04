using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavLogistica24.Modelos
{
    public class mDatos
    {
        public bool BucleInfinito { get; set; }
        public String Folder { get; set; }
        public Int32 Delay { get; set; }
        public String Log { get; set; }
        public String Version { get; set; }
        public String Server { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public String Database { get; set; }
        public String Company { get; set; }
        public String Estado { get; set; }
        public int Counter { get; set; }
        public int Errores { get; set; }
    }
}

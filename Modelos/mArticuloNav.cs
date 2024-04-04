using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NavLogistica24.Modelos
{
    public class mArticuloNav
    {
        public String No_ { get; set; }
        public String Description { get; set; }
        public Boolean Blocked { get; set; }
        public Decimal Kg_Saco { get; set; }
        public String Codigo_envase { get; set; }
        public Boolean Etq_Saco { get; set; }
        public String Cod_Formato { get; set; }
        public Boolean Etq_Palet { get; set; }
        public Decimal Sacos_Palet { get; set; }
        public Int32 Caducidad { get; set; }
        public String EAN13 { get; set; }
        public String Cod_Fmt_Inyector { get; set; }
        public Boolean Etq_Inyector { get; set; }
        public Boolean BigBag { get; set; }
        public Int32 Caducidad_Minima { get; set; }
        public Decimal Kg_Palet { get; set; }
        public String DUN14 { get; set; }
        public String Notas { get; set; }
        public String Cod_Etiq_Imprenta { get; set; }
        public Decimal Sacos_Caja { get; set; }
        public Decimal Kilos_caja { get; set; }
        public String Formula { get; set; }
        public String Descripcion_ampliada { get; set; }
        public Decimal Cajas_Pale { get; set; }
        public String Cod_Etiq_Imprenta_OLD { get; set; }
        public Boolean Etq_Caja { get; set; }
        public Boolean Visado { get; set; }
        public Boolean Ignorar { get; set; }
        public String Var_logistica { get; set; }
        public String Forma { get; set; }
        public String Medidas { get; set; }
        public String Colores { get; set; }
        public String Codigo_Fabricante { get; set; }
        public Int32 Caras_Palet_Etiquetar { get; set; }
        public String EtiquetaPiezaPath { get; set; }
        public String EtiquetaEmbalajePath { get; set; }
        public String EtiquetaEmbalajeManPath { get; set; }
        public Decimal Kg_Saco_SGA { get; set; }
        public Decimal Kg_Palet_SGA { get; set; }
        public Boolean Enviar_a_InLog { get; set; }
        public Boolean Enviar_Codbar_a_Inlog { get; set; }
    }
}

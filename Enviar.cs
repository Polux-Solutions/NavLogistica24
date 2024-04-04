using NavLogistica24.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NavLogistica24
{
    public class Enviar
    {
        /*
        public async Task <mDatos> Proveedores(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            String tt = $@"SELECT VE.[No_], VE.[Name], VE.[Search Name], VE.[Address], VE.[Country_Region Code], CU.[Name], 
                                  VE.[City], VE.[County], VE.[Post Code], VE.[Fax No_], VE.[Phone No_], VE.[Contact] 
                             FROM [{Datos.Company}$Vendor] VE
                             left join [{Datos.Company}$Country_Region] CU on CU.[Code] = VE.[Country_Region Code]
                             Where VE.[Enviar a InLog] = 1";


            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mProveedor Proveedor;
                    Soap WebServices = new Soap();

                    while (oRead.Read())
                    {
                        InlogID++;
                        string Error = "";
                        Proveedor = new mProveedor();

                        Proveedor.accion = "A";
                        Proveedor.codext = oRead.GetString(0);
                        //Proveedor.codigo = oRead.GetString(0);
                        Proveedor.nombre = oRead.GetString(1).Replace("&", "");
                        Proveedor.razsoc = oRead.GetString(1).Replace("&", "");
                        Proveedor.nomcto = oRead.GetString(2).Replace("&", "");
                        Proveedor.direcc = oRead.GetString(3);
                        Proveedor.codpais = oRead.GetString(4);
                        Proveedor.locali = oRead.GetString(6);
                        Proveedor.provin = oRead.GetString(7);
                        Proveedor.codpos = oRead.GetString(8);
                        Proveedor.numfax = oRead.GetString(9);
                        Proveedor.telefo = oRead.GetString(10);
                        Proveedor.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");

                        if (Proveedor.codext.Length > 15) Error = "El código del proveedor (codext) supera los 15 carácteres";
                        //if (Proveedor.codigo.Length > 6) Error = "El código del proveedor (codigo) supera los 6 carácteres";

                        if (Proveedor.nombre.Length > 40) Proveedor.nombre = Proveedor.nombre.Substring(0, 40);
                        if (Proveedor.codpais.Length > 40) Proveedor.codpais = Proveedor.codpais.Substring(0, 40);
                        if (Proveedor.locali.Length > 40) Proveedor.locali = Proveedor.locali.Substring(0, 40);
                        if (Proveedor.provin.Length > 40) Proveedor.provin = Proveedor.provin.Substring(0, 40);
                        if (Proveedor.razsoc.Length > 40) Proveedor.razsoc = Proveedor.razsoc.Substring(0, 40);
                        if (Proveedor.telefo.Length > 40) Proveedor.telefo = Proveedor.telefo.Substring(0, 40);
                        if (Proveedor.direcc.Length > 40) Proveedor.direcc = Proveedor.direcc.Substring(0, 40);
                        if (Proveedor.codpos.Length > 5) Proveedor.codpos = Proveedor.codpos.Substring(0,5);
                        if (Proveedor.nomcto.Length > 15) Proveedor.nomcto = Proveedor.nomcto.Substring(0, 15);
                        if (Proveedor.numfax.Length > 15) Proveedor.numfax = Proveedor.numfax.Substring(0, 14);
                        if (string.IsNullOrEmpty(Proveedor.numfax)) Proveedor.numfax = "0";
                        if (Proveedor.codpos == "") Proveedor.codpos = "00000";

                        Proveedor.numfax = Numeros_Telefono(Proveedor.numfax, 14);
                        Proveedor.telefo = Numeros_Telefono(Proveedor.telefo, 14);

                        String Respuesta = "";
                        if (Error == "")
                        {
                            Respuesta = await WebServices.Proveedores(Datos, Proveedor, InlogID);
                        }
                        else
                        {
                            Respuesta = Error;
                        }
                        Actualizar_Proveedor(Datos, ref Sqlclass, Proveedor.codext);
                        InLog(ref Datos, ref Sqlclass, 0, Proveedor.codext, 0, Proveedor.nombre, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> Articulos(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            String tt = $@"SELECT ART.[No_], ART.[Descripción ampliada], ART.[Cajas_Pale], ART.[Caducidad], ART.[Visado], ART.[Kilos_caja], 
                                  ART.[Sacos_Caja], ART.[Tip_ producto], IT.[Muestras], IT.[Es Lata], COALESCE(UOM.[Qty_ per Unit of Measure],0) 
                             FROM [{Datos.Company}$Articulos] ART
                             INNER Join [{Datos.Company}$Item] IT ON IT.[No_] = ART.[No_]
                             LEFT Join [{Datos.Company}$Item Unit of Measure] UOM on UOM.[Item No_] = IT.[No_] AND UOM.[Code] = 'KILOS'
                             WHERE ART.[Blocked] = 1
                               AND ART.[Enviar a InLog] = 1";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    while (oRead.Read())
                    {
                        InlogID++;
                        Actualizar_Articulos(Datos, ref Sqlclass, oRead.GetString(0));
                        InLog(ref Datos, ref Sqlclass, 1, oRead.GetString(0), 0, oRead.GetString(1), "Artículo bloqueado, no se envía a Inlog", InlogID);
                    }
                } 
                
                oRead.Close();
            }

             tt = $@"SELECT ART.[No_], ART.[Descripción ampliada], ART.[Cajas_Pale], ART.[Caducidad], ART.[Visado], ART.[Kilos_caja], 
                                  ART.[Sacos_Caja], ART.[Tip_ producto], IT.[Muestras], IT.[Es Lata], COALESCE(UOM.[Qty_ per Unit of Measure],0) 
                             FROM [{Datos.Company}$Articulos] ART
                             INNER Join [{Datos.Company}$Item] IT ON IT.[No_] = ART.[No_]
                             LEFT Join [{Datos.Company}$Item Unit of Measure] UOM on UOM.[Item No_] = IT.[No_] AND UOM.[Code] = 'KILOS'
                             WHERE ART.[Blocked] = 0
                               AND ( ART.[Tip_ producto]<>0 AND (ART.[Cajas_Pale] = 0 OR ART.[Kilos_caja] =0 OR ART.[Sacos_Caja] =0) ) 
                               AND ART.[Enviar a InLog] = 1";

            Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    while (oRead.Read())
                    {
                        InlogID++;
                        InLog(ref Datos, ref Sqlclass, 1, oRead.GetString(0), 0, oRead.GetString(1), "Error en cantidad en Cajas_Pale o Kilos_Caja o Sacos_Caja", InlogID);
                    }
                }

                oRead.Close();
            }

            tt = $@"SELECT ART.[No_], ART.[Descripción ampliada], ART.[Cajas_Pale], ART.[Caducidad], ART.[Visado], ART.[Kilos_caja], 
                                  ART.[Sacos_Caja], ART.[Tip_ producto], IT.[Muestras], IT.[Es Lata], COALESCE(UOM.[Qty_ per Unit of Measure],0) 
                             FROM [{Datos.Company}$Articulos] ART
                             INNER Join [{Datos.Company}$Item] IT ON IT.[No_] = ART.[No_]
                             LEFT Join [{Datos.Company}$Item Unit of Measure] UOM on UOM.[Item No_] = IT.[No_] AND UOM.[Code] = 'KILOS'
                             WHERE ART.[Blocked] = 0
                               AND ( ART.[Tip_ producto] =0 OR (ART.[Tip_ producto]<>0 AND (ART.[Cajas_Pale] > 0 and ART.[Kilos_caja] >0 and ART.[Sacos_Caja] >0) ) )
                               AND ART.[Enviar a InLog] = 1";

            Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mArticulo Articulo;
                    Soap WebServices = new Soap();

                    while (oRead.Read())
                    {
                        string Error = "";
                        InlogID++;

                        Articulo = new mArticulo();

                        Articulo.accion = "A";
                        Articulo.almace = "0"; 
                        Articulo.altcaj = "1";
                        Articulo.altpal = "01";
                        Articulo.anccaj = "1";
                        Articulo.ancpal = "01";
                        Articulo.artpro = oRead.GetString(0);
                        Articulo.artpv1 = "0";
                        Articulo.artpv2 = "0";
                        Articulo.artpvl = "0";
                        Articulo.caddev = "N";
                        Articulo.cajpal = oRead.GetDecimal(2).ToString("##0"); 
                        Articulo.capkud = "0";
                        Articulo.claabc = "A";
                        Articulo.codigo = "";
                        Articulo.denomi = oRead.GetString(1).Replace("&", "");
                        Articulo.ean128 = "N";
                        Articulo.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Articulo.famili = "01";
                        Articulo.forven = "U";
                        Articulo.gestns = "N";
                        Articulo.grprod = "01";
                        Articulo.idun14 = "N";
                        Articulo.indsus = "N"; //CAMPO CONFLICTIVO
                        Articulo.iean13 = "N";
                        if (oRead.GetInt32(3) > 0)
                        {
                            Articulo.clacad = "01";
                            Articulo.indcad = "S";
                        }
                        else
                        {
                            Articulo.clacad = "";
                            Articulo.indcad = "N";
                        }
                        Articulo.indpes = "N";
                        Articulo.inpesa = "N";
                        Articulo.insren = "N";
                        Articulo.insrsa = "N";
                        Articulo.larcaj = "1"; 
                        Articulo.loseob = "N";
                        Articulo.loteob = "S";
                        Articulo.lotpic = "N";
                        Articulo.lotsal = "N";
                        Articulo.manpal = "01";
                        Articulo.nsrlon = "0";
                        Articulo.percua = "0";
                        Articulo.pescaj = decimal.ToInt32(oRead.GetDecimal(5) * 1000).ToString("##0"); 
                        Articulo.pesvar = "N";
                        Articulo.prohab = "0";
                        Articulo.propie = "0";
                        Articulo.rkigun = "1"; 
                        Articulo.sitart = "AL"; 
                        Articulo.tipart = "N";
                        Articulo.tipent = "D";
                        Articulo.tipeti = "01";
                        Articulo.tiphue = "01";
                        Articulo.tippal = "01";
                        Articulo.tolera = "0";
                        Articulo.unicaj = decimal.ToInt32(oRead.GetDecimal(6)).ToString("##0");
                        Articulo.unidns = "N"; 
                        Articulo.unimed = "K";
                        Articulo.valdec = "0";
                        Articulo.varlog = "";
                        Articulo.vlogev = "0";
                        Articulo.vlogev = "0";
                        Articulo.vloofe = "0";
                        Articulo.vlogsu = "0";
                        Articulo.volcaj = "1"; // Obligatorio

                        if (oRead.GetInt32(7) == 0)  // VIMK
                        {
                            Articulo.clacad = "";
                            Articulo.cajpal = "100";
                            Articulo.famili = "03";
                            Articulo.pescaj = "1";
                            Articulo.unicaj = "1";
                            Articulo.indcad = "N";
                        }
                        if (oRead.GetByte(8) != 0) // Muestras
                        {
                            Articulo.famili = "04";
                        }

                        if (oRead.GetByte(9) != 0) // Es Lata
                        {
                            Decimal PesoUD = 0;

                            if (oRead.GetDecimal(10) != 0) PesoUD = 1 / oRead.GetDecimal(10);
                            Articulo.pescaj = decimal.ToInt32(oRead.GetDecimal(5) * 1000 * PesoUD ).ToString("##0");
                        }


                            if (Articulo.denomi.Length > 40) Articulo.denomi = Articulo.denomi.Substring(0, 40);
                        String Respuesta = "";
                        if (Error == "")
                        {
                            Respuesta = await WebServices.Articulos(Datos, Articulo, InlogID);
                        }
                        else
                        {
                            Respuesta = Error;
                        }
                        Actualizar_Articulos(Datos, ref Sqlclass, Articulo.artpro);
                        InLog(ref Datos, ref Sqlclass, 1, Articulo.artpro, 0, Articulo.denomi, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }

                oRead.Close();
            }

            return Datos;
        }

        public async Task<mDatos> CodigosBarras(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            String tt = $@"SELECT ART.[No_], ART.[Sacos_Caja], ART.[Var_logistica], ART.[DUN14]
                             FROM[{Datos.Company}$Articulos] ART
                             WHERE ART.[Blocked] = 0
                              AND ( ART.[Tip_ producto] =0 OR (ART.[Tip_ producto]<>0 AND (ART.[Cajas_Pale] > 0 and ART.[Kilos_caja] >0 and ART.[Sacos_Caja] >0 and ART.[Sacos_Caja] < 9000) ) )
                              AND ( (ART.[Var_logistica] <> '') OR (ART.[DUN14] <>'') )
                               AND ART.[Enviar Codbar a Inlog] = 1";
                               


            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mCodigoBarras CodBar;
                    Soap WebServices = new Soap();

                    while (oRead.Read())
                    {
                        string Error = "";
                        InlogID++;

                        string ean = "";
                        string tipo = "";

                        for (int i = 1; i <= 2; i++)
                        {
                            if (i==1) // EAN13
                            {
                                ean = oRead.GetString(2);
                                tipo = "EAN13";
                            }
                            if (i==2) // DUN14
                            {
                                ean = oRead.GetString(3);
                                tipo = "DUN14";
                                if (ean.Length == 13) ean = "0" + ean;
                                if (ean.Length < 14) ean = "";
                            }

                            if (ean != "")
                            {
                                CodBar = new mCodigoBarras();

                                CodBar.accion = "A";
                                CodBar.almace = "0";
                                CodBar.artpro = oRead.GetString(0);
                                CodBar.artpv1 = "0";
                                CodBar.artpv2 = "0";
                                CodBar.artpvl = "0";
                                CodBar.propie = "0";
                                CodBar.codbar = ean;
                                CodBar.tipcod = tipo; 
                                
                                if (decimal.ToInt32(oRead.GetDecimal(1)) == 0)
                                {
                                    CodBar.unicaj = "1";
                                }
                                else
                                {
                                    CodBar.unicaj = decimal.ToInt32(oRead.GetDecimal(1)).ToString("##0");
                                }

                                CodBar.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                CodBar.fecalt = DateTime.Now.ToString("yyyyMMddHHmmss");

                                String Respuesta = "";
                                if (Error == "")
                                {
                                    Respuesta = await WebServices.Codigos_Barras(Datos, CodBar, InlogID);
                                }
                                else
                                {
                                    Respuesta = Error;
                                }
                                InLog(ref Datos, ref Sqlclass, 2, CodBar.artpro, 0, ean, Respuesta, InlogID);
                            }
                        }
                        Actualizar_CodBar(Datos, ref Sqlclass, oRead.GetString(0));
                        
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }
        */
        public async Task<mDatos> Clientes(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);
            ObservableCollection <mCliente>Clientes = new ObservableCollection<mCliente>();

            String tt = $@"SELECT CU.[No_], CU.[Name], CU.[Search Name], CU.[Address], CU.[Country_Region Code], CR.[Name], CU.[City], CU.[County], CU.[Post Code], CU.[Fax No_], CU.[Phone No_] 
                             FROM [{Datos.Company}$Customer] CU
                             left join [{Datos.Company}$Country_Region] CR on CR.[Code] = CU.[Country_Region Code]
                             Where CU.[Enviar a InLog] = 1";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mCliente Cliente;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cliente = new mCliente();

                        Cliente.OK = true;
                        Cliente.accion = "A";
                        Cliente.codext = oRead.GetString(0);
                        Cliente.nombre = oRead.GetString(1).Replace("&", "");
                        Cliente.razsoc = oRead.GetString(1).Replace("&", "");
                        Cliente.nomcto = oRead.GetString(1).Replace("&", "");
                        Cliente.direcc = oRead.GetString(3);
                        Cliente.copais = oRead.GetString(4);
                        Cliente.locali = oRead.GetString(6);
                        Cliente.provin = oRead.GetString(7);
                        Cliente.codpos = oRead.GetString(8);
                        Cliente.numfax = Numeros_Telefono(oRead.GetString(9), 14);
                        Cliente.tipcli = "01";
                        Cliente.telefo = Numeros_Telefono(oRead.GetString(10), 40);
                        Cliente.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");

                        if (Cliente.codext.Length > 15) Cliente.NOK(ref Cliente, "El código del cliente (codext) supera los 15 carácteres");

                        if (Cliente.nombre.Length > 40) Cliente.nombre = Cliente.nombre.Substring(0, 40);
                        if (Cliente.copais.Length > 40) Cliente.copais = Cliente.copais.Substring(0, 40);
                        if (Cliente.locali.Length > 40) Cliente.locali = Cliente.locali.Substring(0, 40);
                        if (Cliente.provin.Length > 40) Cliente.provin = Cliente.provin.Substring(0, 40);
                        if (Cliente.razsoc.Length > 40) Cliente.razsoc = Cliente.razsoc.Substring(0, 40);
                        if (Cliente.telefo.Length > 40) Cliente.telefo = Cliente.telefo.Substring(0, 40);
                        if (Cliente.direcc.Length > 40) Cliente.direcc = Cliente.direcc.Substring(0, 40);
                        if (Cliente.codpos.Length > 7) Cliente.codpos = Cliente.codpos.Substring(0, 7);
                        if (String.IsNullOrEmpty(Cliente.nomcto)) Cliente.nomcto = Cliente.nombre;
                        if (Cliente.nomcto.Length > 15) Cliente.nomcto = Cliente.nomcto.Substring(0, 15);
                        if (Cliente.codpos == "") Cliente.codpos = "00000";

                        Clientes.Add(Cliente);
                    }
                }
                    
                oRead.Close();

                String Respuesta = "";

                    Soap WebServices = new Soap();
                    bool PrimerAlmacen = true;

                    foreach (mAlmacenes Almacen in Almacenes)
                    {
                    foreach (mCliente Cliente in Clientes)
                    {
                        if (Cliente.OK)
                        {
                            Respuesta = await WebServices.Clientes(Datos, Cliente, InlogID, Almacen);
                        }
                        else
                        {
                            Respuesta = Cliente.Error;
                        }

                        InLog(ref Datos, ref Sqlclass, 3, Cliente.codext, 0, Cliente.nombre, Respuesta, InlogID, string.Empty);

                        if (PrimerAlmacen) Datos.Counter++;
                    }

                    if (PrimerAlmacen)  PrimerAlmacen = false;
                }
            }

            return Datos;
        }

        /*
        public async Task<mDatos> OC(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select OC.[No_], OC.[Buy-from Vendor No_], coalesce(OC.[Order Date], getdate()), OC.[Tipo de pedido], count(*)
                             from[{Datos.Company}$Purchase Header] OC
                             inner join[{Datos.Company}$Purchase Line] OL on OL.[Document Type] = OC.[Document Type] and OL.[Document No_] = OC.[No_] 
                             where OC.[Document Type] = 1 
                              and OC.[Devolución] = 0
                              and OC.[Buy-from Vendor No_] <> ''
                              and OC.[Enviar a InLog] = 1
                              and OL.[Type] = 2
                              and OL.[Outstanding Quantity] > 0
                              and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                             group by OC.[No_], OC.[Buy-from Vendor No_], OC.[Order Date], OC.[Tipo de pedido]
                             having Count(*) >= 1
                             order by OC.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mOCCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mOCCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.pedext = oRead.GetString(0);
                        Cabecera.proext = oRead.GetString(1);
                        Cabecera.fecasi = oRead.GetDateTime(2).ToString("yyyyMMddHHmmss");
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.fecpre = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.propie = "0";
                        Cabecera.tipped = "MA";
                        Cabecera.totlin = oRead.GetInt32(4).ToString("##0");
                        Cabecera.sitped = "PE";


                        string tt2 = $@"select OL.[Line No_], OL.[No_], OL.[Outstanding Quantity], OL.[Kilos por envase], 
                                               OL.[Unit of Measure Code]
                                     from [{Datos.Company}$Purchase Line] OL 
                                     where OL.[Document Type] = 1 
                                      and OL.[Document No_] = '{Cabecera.pedext}' 
                                      and OL.[Type] = 2
                                      and OL.[Outstanding Quantity] > 0
                                      and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                                     order by OL.[Line No_]";


                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mOCLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mOCLineas iLinea = new mOCLineas();
                                mArticuloNav ArticuloNAV= null;
                                Funciones f = new Funciones();

                                iLinea.artpro = Item_Cross_Proveedor(Datos, ref Sqlclass, Cabecera.proext, oRead2.GetString(1));
                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    // Envases = Cantidad / [Kilos x Envases]
                                    decimal Envases = oRead2.GetDecimal(2);
                                    decimal KgSaco = oRead2.GetDecimal(3);
                                    if (KgSaco == 0) KgSaco = ArticuloNAV.Kg_Saco;
                                    if (KgSaco != 0) Envases = Envases / KgSaco;
                                    if (Envases % 1 != 0) Envases = Math.Truncate(Envases) + 1;

                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.canteo = Envases.ToString("##0"); // Kilos por envase
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.pedext = Cabecera.pedext;
                                    iLinea.sitlin = "PE";
                                    iLinea.tipfor = "U";
                                    iLinea.unicaj = ArticuloNAV.Sacos_Caja.ToString("##0");
                                    iLinea.codean = ArticuloNAV.Var_logistica;
                                    iLinea.feccad = DateTime.Now.ToString("yyyyMMdd");
                                    iLinea.codpal = "0";

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass, 1, Cabecera.pedext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.OC(Datos, Cabecera, InlogID);

                        Actualizar_Compras(Datos, ref Sqlclass, 1,Cabecera.pedext);
                        InLog(ref Datos, ref Sqlclass, 4, Cabecera.pedext, 0, Cabecera.proext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> Transfer_Entrada(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select TH.[No_], VE.[No_], TH.[Shipment Date], count(*)
                             from [{Datos.Company}$Transfer Header] TH
                             inner join [{Datos.Company}$Transfer Line] TL on TL.[Document No_] = TH.[No_]
                             inner join [{Datos.Company}$Location] ALM on ALM.[Code] = TH.[Transfer-from Code]
                             inner join [{Datos.Company}$Vendor] VE on VE.[No_] = ALM.[Proveedor]
                             where TH.[Transfer-to Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                               and TH.[Enviar a InLog] = 1
                               and TL.[Nº envases] >0
                            group by TH.[No_], VE.[No_], TH.[Shipment Date]
                            having Count(*) >= 1
                            order by TH.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mOCCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mOCCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.pedext = oRead.GetString(0);
                        Cabecera.proext = oRead.GetString(1);
                        Cabecera.fecasi = oRead.GetDateTime(2).ToString("yyyyMMddHHmmss");
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.fecpre = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.propie = "0";
                        Cabecera.tipped = "MA";
                        Cabecera.totlin = oRead.GetInt32(3).ToString("##0");
                        Cabecera.sitped = "PE";


                        string tt2 = $@"select TL.[Line No_], TL.[Item No_], TL.[Nº envases], TL.[Cajas por envase], 
                                               TL.[Unit of Measure Code]
                                     from [{Datos.Company}$Transfer Line] TL 
                                     where TL.[Document No_] = '{Cabecera.pedext}'
                                      and TL.[Quantity (Base)] - TL.[Qty_ Received (Base)] > 0
                                     order by TL.[Line No_]";


                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mOCLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mOCLineas iLinea = new mOCLineas();
                                mArticuloNav ArticuloNAV = null;
                                Funciones f = new Funciones();

                                iLinea.artpro = Item_Cross_Proveedor(Datos, ref Sqlclass, Cabecera.proext, oRead2.GetString(1));
                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    decimal unicaj =  oRead2.GetDecimal(3);
                                    if (unicaj == 0) unicaj = 1;
                                    unicaj = 1 / unicaj;

                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.canteo = oRead2.GetInt32(2).ToString("##0"); // Kilos por envase
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.pedext = Cabecera.pedext;
                                    iLinea.sitlin = "PE";
                                    iLinea.tipfor = "U";
                                    iLinea.unicaj = unicaj.ToString("##0");
                                    iLinea.codean = ArticuloNAV.Var_logistica;
                                    iLinea.feccad = DateTime.Now.ToString("yyyyMMdd");
                                    iLinea.codpal = "0";

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass, 1, Cabecera.pedext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.OC(Datos, Cabecera, InlogID);

                        Actualizar_Transferencia(Datos, ref Sqlclass,  Cabecera.pedext);
                        InLog(ref Datos, ref Sqlclass, 8, Cabecera.pedext, 0, Cabecera.proext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> PS(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select PS.[No_], PS.[Sell-to Customer No_], coalesce(PS.[Order Date], getdate()),
                             PS.[Ship-to Code], PS.[Ship-to Name], PS.[Ship-to Name 2], PS.[Ship-to Address], PS.[Ship-to Address 2],
                             PS.[Ship-to City], PS.[Ship-to Post Code], PS.[Ship-to County], PS.[Ship-to Country_Region Code], PS.[Ship-to Phone No_],
                             SSE.[Cliente Ecommerce],
                             count(*)
                             from[{Datos.Company}$Sales Header] PS
                             inner join [{Datos.Company}$Sales Line] PL on PL.[Document Type] = PS.[Document Type] and PL.[Document No_] = PS.[No_] 
                             Cross join [{Datos.Company}$Sales & Receivables Setup] SSE
                             where PS.[Document Type] = 1 
                              and PS.[Sell-to Customer No_] <> ''
                              and PS.[Enviar a InLog] = 1
                              and PL.[Type] = 2
                              and PL.[Outstanding Quantity] > 0
                              and PL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                             group by PS.[No_], PS.[Sell-to Customer No_], PS.[Order Date],
                                     PS.[Ship-to Code], PS.[Ship-to Name], PS.[Ship-to Name 2], PS.[Ship-to Address], PS.[Ship-to Address 2],
                                     PS.[Ship-to City], PS.[Ship-to Post Code], PS.[Ship-to County], PS.[Ship-to Country_Region Code], PS.[Ship-to Phone No_],
                                     SSE.[Cliente Ecommerce]
                             having Count(*) >= 1
                             order by PS.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mPSCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mPSCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.cliext = oRead.GetString(1);
                        Cabecera.copais = oRead.GetString(11);
                        Cabecera.codpos = oRead.GetString(9);
                        Cabecera.cross = "N";
                        Cabecera.dirent = oRead.GetString(6) + " " + oRead.GetString(7);
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.guicar = "0";
                        Cabecera.impadl = "N";
                        Cabecera.indexp = "N";
                        Cabecera.indnap = "N";
                        Cabecera.indurg = "0";
                        Cabecera.locali = oRead.GetString(8);
                        Cabecera.ordcar = "0";
                        Cabecera.pedext = oRead.GetString(0);
                        Cabecera.propie = "0";
                        Cabecera.provin = oRead.GetString(10);
                        Cabecera.sitped = "PE";
                        Cabecera.telefo = oRead.GetString(12);
                        Cabecera.tipped = "PV";
                        Cabecera.lotsec = "";

                        string ClienteEComm = oRead.GetString(13);
                        if (ClienteEComm.ToUpper() == Cabecera.cliext.ToUpper()) Cabecera.lotsec = "ECOMMERCE";

                        if (Cabecera.dirent.Length > 40) Cabecera.dirent = Cabecera.dirent.Substring(0, 40);
                        if (Cabecera.codpos.Length > 7) Cabecera.codpos = Cabecera.codpos.Substring(0, 7);
                        if (string.IsNullOrEmpty(Cabecera.codpos)) Cabecera.codpos = "00000";

                        string tt2 = $@"select PL.[Line No_], PL.[No_], PL.[Outstanding Quantity], PL.[Nº envases a enviar], PL.[Unit of Measure Code]
                                     from [{Datos.Company}$Sales Line] PL 
                                     where PL.[Document Type] = 1 
                                      and PL.[Document No_] = '{Cabecera.pedext}' 
                                      and PL.[Type] = 2
                                      and PL.[Outstanding Quantity] > 0
                                      and PL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                                     order by PL.[Line No_]";

                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mPSLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mPSLineas iLinea = new mPSLineas();

                                mArticuloNav ArticuloNAV = null;
                                Funciones f = new Funciones();

                                iLinea.artpro = oRead2.GetString(1);

                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.canped = oRead2.GetInt32(3).ToString("##0");
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.forfor = "N";
                                    iLinea.pedext = Cabecera.pedext;
                                    iLinea.sitlin = "PE";
                                    iLinea.tipfor = "U";
                                    iLinea.unicaj = ArticuloNAV.Sacos_Caja.ToString("##0");

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass, 1, Cabecera.pedext, oRead2.GetInt32(0), NoLine);
                                    //Actualizar_Lineas_PS(Datos, ref Sqlclass,  Cabecera.pedext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.PS(Datos, Cabecera, InlogID);

                        Actualizar_PS(Datos, ref Sqlclass, Cabecera.pedext);
                        InLog(ref Datos, ref Sqlclass, 5, Cabecera.pedext, 0, Cabecera.pedext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> Transfer_Salida(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select TH.[No_], CU.[No_], TH.[Shipment Date],
                             TH.[Transfer-to Code], TH.[Transfer-to Name], TH.[Transfer-to Name 2], TH.[Transfer-to Address], TH.[Transfer-to Address 2],
                             TH.[Transfer-to City], TH.[Transfer-to Post Code], TH.[Transfer-to County], TH.[Trsf_-to Country_Region Code],
                             SSE.[Almacén Ecommerce],
                             count(*)
                             from [{Datos.Company}$Transfer Header] TH
                             inner join [{Datos.Company}$Transfer Line] TL on TL.[Document No_] = TH.[No_]
                             inner join [{Datos.Company}$Location] ALM on ALM.[Code] = TH.[Transfer-to Code]
                             inner join [{Datos.Company}$Customer] CU on CU.[No_] = ALM.[Cliente]
                             Cross join [{Datos.Company}$Sales & Receivables Setup] SSE
                             where TH.[Transfer-from Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                               and TH.[Enviar a InLog] = 1
                               and TL.[Nº envases] >0
                            group by TH.[No_], CU.[No_], TH.[Shipment Date],
                                     TH.[Transfer-to Code], TH.[Transfer-to Name], TH.[Transfer-to Name 2], TH.[Transfer-to Address], TH.[Transfer-to Address 2],
                                     TH.[Transfer-to City], TH.[Transfer-to Post Code], TH.[Transfer-to County], TH.[Trsf_-to Country_Region Code],
                                     SSE.[Almacén Ecommerce] 
                            having Count(*) >= 1
                            order by TH.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mPSCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mPSCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.cliext = oRead.GetString(1);
                        Cabecera.copais = oRead.GetString(11);
                        Cabecera.codpos = oRead.GetString(9);
                        Cabecera.cross = "N";
                        Cabecera.dirent = oRead.GetString(6) + " " + oRead.GetString(7);
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.guicar = "0";
                        Cabecera.impadl = "N";
                        Cabecera.indexp = "N";
                        Cabecera.indnap = "N";
                        Cabecera.indurg = "0";
                        Cabecera.locali = oRead.GetString(8);
                        Cabecera.ordcar = "0";
                        Cabecera.pedext = oRead.GetString(0);
                        Cabecera.propie = "0";
                        Cabecera.provin = oRead.GetString(10);
                        Cabecera.sitped = "PE";
                        Cabecera.telefo = "";
                        Cabecera.tipped = "PV";
                        Cabecera.lotsec = "";

                        if (Cabecera.dirent.Length > 40) Cabecera.dirent = Cabecera.dirent.Substring(0, 40);
                        if (Cabecera.codpos == "") Cabecera.codpos = "00000";
                        if (Cabecera.codpos.Length > 7) Cabecera.codpos = Cabecera.codpos.Substring(0, 7);
                        if (string.IsNullOrEmpty(Cabecera.codpos)) Cabecera.codpos = "00000";
                        if (oRead.GetString(12) == oRead.GetString(3)) Cabecera.lotsec = "ECOMMERCE";

                        string tt2 = $@"select TL.[Line No_], TL.[Item No_], TL.[Nº envases], TL.[Unit of Measure Code]
                                     from [{Datos.Company}$Transfer Line] TL 
                                     where TL.[Document No_] = '{Cabecera.pedext}' 
                                      and TL.[Outstanding Quantity] > 0
                                     order by TL.[Line No_]";

                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mPSLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mPSLineas iLinea = new mPSLineas();

                                mArticuloNav ArticuloNAV = null;
                                Funciones f = new Funciones();

                                iLinea.artpro = oRead2.GetString(1);

                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.canped = oRead2.GetInt32(2).ToString("##0");
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.forfor = "N";
                                    iLinea.pedext = Cabecera.pedext;
                                    iLinea.sitlin = "PE";
                                    iLinea.tipfor = "U";
                                    iLinea.unicaj = ArticuloNAV.Sacos_Caja.ToString("##0");

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass, 1, Cabecera.pedext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.PS(Datos, Cabecera, InlogID);

                        Actualizar_Transferencia(Datos, ref Sqlclass, Cabecera.pedext);
                        InLog(ref Datos, ref Sqlclass, 9, Cabecera.pedext, 0, Cabecera.pedext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> DevPro(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select OC.[No_], OC.[Buy-from Vendor No_], coalesce(OC.[Order Date], getdate()), OC.[Tipo de pedido], count(*)
                             from[{Datos.Company}$Purchase Header] OC
                             inner join[{Datos.Company}$Purchase Line] OL on OL.[Document Type] = OC.[Document Type] and OL.[Document No_] = OC.[No_] 
                             where OC.[Document Type] = 1
                              and OC.[Devolución] = 1
                              and OC.[Buy-from Vendor No_] <> ''
                              and OC.[Enviar a InLog] = 1
                              and OL.[Type] = 2
                              and OL.[Outstanding Quantity] < 0
                              and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                             group by OC.[No_], OC.[Buy-from Vendor No_], OC.[Order Date], OC.[Tipo de pedido]
                             having Count(*) >= 1
                             order by OC.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mDevProCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mDevProCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.codext = oRead.GetString(0);
                        Cabecera.numdoc = oRead.GetString(0);
                        Cabecera.proext = oRead.GetString(1);
                        Cabecera.fecdev= oRead.GetDateTime(2).ToString("yyyyMMddHHmmss");
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.propie = "0";
                        Cabecera.sitcab = "PE";


                        string tt2 = $@"select OL.[Line No_], OL.[No_], OL.[Outstanding Quantity], OL.[Kilos por envase], 
                                               OL.[Unit of Measure Code]
                                     from [{Datos.Company}$Purchase Line] OL 
                                     where OL.[Document Type] = 1
                                      and OL.[Document No_] = '{Cabecera.codext}' 
                                      and OL.[Type] = 2
                                      and OL.[Outstanding Quantity] < 0
                                      and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                                     order by OL.[Line No_]";

                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mDevProLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mDevProLineas iLinea = new mDevProLineas();
                                mArticuloNav ArticuloNAV = null;
                                Funciones f = new Funciones();

                                iLinea.artpro = Item_Cross_Proveedor(Datos, ref Sqlclass, Cabecera.proext, oRead2.GetString(1));
                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    // Envases = Cantidad / [Kilos x Envases]
                                    decimal Envases = -oRead2.GetDecimal(2);
                                    decimal KgSaco = oRead2.GetDecimal(3);
                                    if (KgSaco == 0) KgSaco = ArticuloNAV.Kg_Saco;
                                    if (KgSaco != 0) Envases = Envases / KgSaco;
                                    if (Envases % 1 != 0) Envases = Math.Truncate(Envases) + 1;

                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.cantap = "0";
                                    iLinea.cantna = Envases.ToString("##0"); // Kilos por envase
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.fecdev = Cabecera.fecdev;
                                    iLinea.codext = Cabecera.codext;
                                    iLinea.sitlin = "PE";
                                    iLinea.feccad = DateTime.Now.ToString("yyyyMMdd");

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass,5, Cabecera.codext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.DevPro(Datos, Cabecera, InlogID);

                        Actualizar_Compras(Datos, ref Sqlclass, 1,Cabecera.codext);
                        InLog(ref Datos, ref Sqlclass, 6, Cabecera.codext, 0, Cabecera.proext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }

        public async Task<mDatos> DevCli(mDatos Datos)
        {
            Sql Sqlclass = new Sql();
            SqlDataReader oRead = null;
            SqlDataReader oRead2 = null;
            Int32 InlogID = Inlog_Id(Datos, ref Sqlclass);

            string tt = $@"select OC.[No_], OC.[Buy-from Vendor No_], coalesce(OC.[Order Date], getdate()), OC.[Tipo de pedido], count(*)
                             from[{Datos.Company}$Purchase Header] OC
                             inner join[{Datos.Company}$Purchase Line] OL on OL.[Document Type] = OC.[Document Type] and OL.[Document No_] = OC.[No_] 
                             where OC.[Document Type] = 1
                              and OC.[Devolución] = 1
                              and OC.[Buy-from Vendor No_] <> ''
                              and OC.[Enviar a InLog] = 1
                              and OL.[Type] = 2
                              and OL.[Outstanding Quantity] < 0
                              and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                             group by OC.[No_], OC.[Buy-from Vendor No_], OC.[Order Date], OC.[Tipo de pedido]
                             having Count(*) >= 1
                             order by OC.[No_]";

            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            Datos.Counter = 0;
            Datos.Errores = 0;
            if (Abrir)
            {
                if (oRead.HasRows)
                {
                    mDevProCabecera Cabecera;
                    Soap WebServices = new Soap();
                    Int32 NoLine = 0;

                    while (oRead.Read())
                    {
                        InlogID++;

                        Cabecera = new mDevProCabecera();
                        Cabecera.accion = "A";
                        Cabecera.almace = "0";
                        Cabecera.codext = oRead.GetString(0);
                        Cabecera.numdoc = oRead.GetString(0);
                        Cabecera.proext = oRead.GetString(1);
                        Cabecera.fecdev= oRead.GetDateTime(2).ToString("yyyyMMddHHmmss");
                        Cabecera.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Cabecera.propie = "0";
                        Cabecera.sitcab = "PE";


                        string tt2 = $@"select OL.[Line No_], OL.[No_], OL.[Outstanding Quantity], OL.[Kilos por envase], 
                                               OL.[Unit of Measure Code]
                                     from [{Datos.Company}$Purchase Line] OL 
                                     where OL.[Document Type] = 1
                                      and OL.[Document No_] = '{Cabecera.codext}' 
                                      and OL.[Type] = 2
                                      and OL.[Outstanding Quantity] < 0
                                      and OL.[Location Code] = (Select [Almacen SGA] from [{Datos.Company}$Warehouse Setup])
                                     order by OL.[Line No_]";

                        bool Abrir2 = Sqlclass.Crear_Datareader(ref Datos, ref oRead2, tt2);
                        if (Abrir2)
                        {
                            Cabecera.Lineas = new ObservableCollection<mDevProLineas>();
                            while (oRead2.Read())
                            {
                                NoLine++;
                                mDevProLineas iLinea = new mDevProLineas();
                                mArticuloNav ArticuloNAV = null;
                                Funciones f = new Funciones();

                                iLinea.artpro = Item_Cross_Proveedor(Datos, ref Sqlclass, Cabecera.proext, oRead2.GetString(1));
                                f.Articulo_NAV(Datos, ref Sqlclass, ref ArticuloNAV, iLinea.artpro);

                                if (ArticuloNAV != null)
                                {
                                    // Envases = Cantidad / [Kilos x Envases]
                                    decimal Envases = -oRead2.GetDecimal(2);
                                    decimal KgSaco = oRead2.GetDecimal(3);
                                    if (KgSaco == 0) KgSaco = ArticuloNAV.Kg_Saco;
                                    if (KgSaco != 0) Envases = Envases / KgSaco;
                                    if (Envases % 1 != 0) Envases = Math.Truncate(Envases) + 1;

                                    iLinea.accion = "A";
                                    iLinea.artpv1 = "0";
                                    iLinea.artpv2 = "0";
                                    iLinea.artpvl = "0";
                                    iLinea.cantap = "0";
                                    iLinea.cantna = Envases.ToString("##0"); // Kilos por envase
                                    iLinea.codlin = NoLine.ToString("##0");
                                    iLinea.fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    iLinea.fecdev = Cabecera.fecdev;
                                    iLinea.codext = Cabecera.codext;
                                    iLinea.sitlin = "PE";
                                    iLinea.feccad = DateTime.Now.ToString("yyyyMMdd");

                                    Cabecera.Lineas.Add(iLinea);

                                    Actualizar_Lineas_Mapeo(Datos, ref Sqlclass,5, Cabecera.codext, oRead2.GetInt32(0), NoLine);
                                }
                            }
                            oRead2.Close();
                        }

                        String Respuesta = await WebServices.DevPro(Datos, Cabecera, InlogID);

                        Actualizar_Compras(Datos, ref Sqlclass, 1,Cabecera.codext);
                        InLog(ref Datos, ref Sqlclass, 6, Cabecera.codext, 0, Cabecera.proext, Respuesta, InlogID);
                        Datos.Counter++;
                    }
                }
            }

            return Datos;
        }
        */

        private void InLog(ref mDatos Datos, ref Sql Sqlclass,  int xTipo, string xDocumentoNo, int xLineNo, string xDescription, string xRespuesta, Int32 xId, string xAlmacen)
        {
            int OK = 0;
            string TextoError;
            string tt = "";
            bool Dummy = false;

            if (xRespuesta.ToUpper() == "OK")
            {
                OK = 1;
                TextoError = "";
            }
            else
            {
                OK = 0;
                TextoError = xRespuesta;
                Datos.Errores++;
            }

            if (OK == 0)
            {
                tt = @$"DELETE FROM [{Datos.Company}$SGA Log]
                              WHERE [Tipo] = {xTipo}
                                AND [Document No_] = '{xDocumentoNo}'
                                AND CONVERT(date, [Hora modificacion]) = CONVERT(Date, getdate()) 
                                AND [Ok] = 0";
               Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
            }

            tt = $@"INSERT INTO [{Datos.Company}$SGA Log] ([Tipo], [Document No_], [Line No_], [Description], [Inlog ID], [Almacen], [Ok], [Error], [Hora modificacion]) 
                           Values ({xTipo},'{xDocumentoNo}',{xLineNo},'{xDescription}',{xId}, '{xAlmacen}', {OK}, '{TextoError}', Getdate())";

            Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Proveedor(mDatos Datos, ref Sql Sqlclass, string xProveedor)
        {
            String tt = $@"UPDATE [{Datos.Company}$Vendor] SET [Enviar a InLog] = 0 where [No_] = '{xProveedor}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Articulos(mDatos Datos, ref Sql Sqlclass, string xReferencia)
        {
            String tt = $@"UPDATE [{Datos.Company}$Articulos] SET [Enviar a InLog] = 0 where [No_] = '{xReferencia}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_CodBar(mDatos Datos, ref Sql Sqlclass, string xReferencia)
        {
            String tt = $@"UPDATE [{Datos.Company}$Articulos] SET [Enviar Codbar a Inlog] = 0 where [No_] = '{xReferencia}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Cliente(mDatos Datos, ref Sql Sqlclass, string xCliente)
        {
            String tt = $@"UPDATE [{Datos.Company}$Customer] SET [Enviar a InLog] = 0 where [No_] = '{xCliente}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Compras(mDatos Datos, ref Sql Sqlclass, Int32 xTipo,  string xOC)
        {
            String tt = $@"UPDATE [{Datos.Company}$Purchase Header] SET [Enviar a InLog] = 0 where [Document Type] = {xTipo} AND [No_] = '{xOC}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Transferencia(mDatos Datos, ref Sql Sqlclass, string xOC)
        {
            String tt = $@"UPDATE [{Datos.Company}$Transfer Header] SET [Enviar a InLog] = 0 where [No_] = '{xOC}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_PS(mDatos Datos, ref Sql Sqlclass, string xPS)
        {
            String tt = $@"UPDATE [{Datos.Company}$Sales Header] SET [Enviar a InLog] = 0 where [No_] = '{xPS}'";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private void Actualizar_Lineas_Mapeo(mDatos Datos, ref Sql Sqlclass, Int32 xTipo,   string xOC, Int32 xLineOC, Int32 xLineSGA)
        {
            string tt = $@"DELETE FROM [{Datos.Company}$SGA Mapeo Docs]
                                  WHERE [Document Type] = 0
                                    AND [Document No_] = '{xOC}'
                                    AND [Line No_] = {xLineOC}";

            bool Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);


            tt = $@"INSERT INTO [{Datos.Company}$SGA Mapeo Docs] ([Document Type], [Document No_], [Line No_], [InLog Line No_])
                         VALUES({xTipo}, '{xOC}', {xLineOC}, {xLineSGA})";

            Dummy = Sqlclass.Ejecutar_SQL(ref Datos, tt);
        }

        private string Numeros_Telefono(string tfn, int Largo)
        {
            string Resultado = "";

            for (int i = 0; i<= tfn.Length-1; i++ )
            {
                if (tfn.Substring(i, 1).All(Char.IsNumber)) Resultado += tfn.Substring(i, 1);
            }

            if (String.IsNullOrEmpty(Resultado)) Resultado = "0";
            if (Resultado.Length > Largo) Resultado = Resultado.Substring(0, Largo);

            return (Resultado);
        }

        private string Item_Cross_Proveedor(mDatos Datos, ref Sql Sqlclass, string xProveedor, string xReferencia)
        {
            string ReferenciaCruzada = xReferencia;
            SqlDataReader oRead = null;

            string tt = $@"SELECT [Cross-Reference No_] FROM [{Datos.Company}$Item Cross Reference]
                                WHERE [Item No_] = '{xReferencia}' 
                                  AND [Cross-Reference Type] = 2
                                  AND [Cross-Reference Type No_] = '{xProveedor}'
                                  AND [Cross-Reference No_] <>''";

            if ( Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt))
            {
                if (oRead.Read())
                    {
                        ReferenciaCruzada = oRead.GetString(0);
                }
                oRead.Close();
            }

            return (ReferenciaCruzada);
        }

        private Int32 Inlog_Id(mDatos Datos, ref Sql Sqlclass)
        {
            Int32 Id = 0;
            SqlDataReader oRead = null;

            String tt = $@"SELECT COALESCE(MAX([Inlog ID]),0) FROM [{Datos.Company}$SGA Log]";
            bool Abrir = Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt);

            if (oRead.Read()) Id = oRead.GetInt32(0);

            return (Id);
        }
    }
}

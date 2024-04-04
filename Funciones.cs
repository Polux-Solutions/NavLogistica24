using NavLogistica24.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace NavLogistica24
{
    public class Funciones
    {
        public String LastError = "";

        public void KillAll()
        {
            string MiProceso = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            Int32 i;

            i = MiProceso.IndexOf(".");
            if (i > 0) MiProceso = MiProceso.Substring(0, i - 1);

            foreach (Process pr in System.Diagnostics.Process.GetProcessesByName(MiProceso))
            {
                if (pr.Id != System.Diagnostics.Process.GetCurrentProcess().Id) pr.Kill();
            }
        }



        public string Menu(ref mDatos Datos)
        {
            Console.Clear();
            Console.WriteLine(" 1. Artículos");
            Console.WriteLine(" 2. Cód EAN");
            Console.WriteLine(" 3. Proveedores");
            Console.WriteLine(" 4. Clientes");
            Console.WriteLine("10. Órdenes de Compra");
            Console.WriteLine("11. Pedidos de Salida");
            Console.WriteLine("12. Devoluciones Proveedores");
            Console.WriteLine("13. Devoluciones Clientes");
            Console.WriteLine("99. Todo");
            Console.WriteLine(" 0. Salir");
            Console.WriteLine("-1. Todo Bucle");
            Console.Write("Pulse Opción: ");
            string Opc = Console.ReadLine();

            switch (Opc)
            {
                case "0": return("SALIR");
                case "1": return("ARTICULOS");
                case "2": return("EAN");
                case "3": return("PROVEEDORES");
                case "4": return("CLIENTES");
                case "10": return ("OC");
                case "11": return ("PS");
                case "12": return ("DEV-P");
                case "13": return ("DEV-C");
                case "99":return ("TODOS");
                case "-1": Datos.BucleInfinito = true;
                    return ("TODOS");
            }

            return ("ERROR");
        }


        public Boolean Leer_Parametros(ref mDatos Datos)
        {
            Boolean OK = true;

            try
            {
                Datos = new mDatos();
                Datos.Folder = System.Configuration.ConfigurationManager.AppSettings["FOLDER"];
                Datos.Log = System.Configuration.ConfigurationManager.AppSettings["LOG"];
                Datos.Version = System.Configuration.ConfigurationManager.AppSettings["VERSION"];
                Datos.User = System.Configuration.ConfigurationManager.AppSettings["USER"]; ;
                Datos.Password = System.Configuration.ConfigurationManager.AppSettings["PASSWORD"]; ;
                Datos.Company = System.Configuration.ConfigurationManager.AppSettings["COMPANY"]; ;
                Datos.Server = System.Configuration.ConfigurationManager.AppSettings["SERVER"]; ;
                Datos.Database = System.Configuration.ConfigurationManager.AppSettings["DATABASE"]; ;
                Datos.Delay = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELAY"]); 
            }
            catch (Exception ex)
            {
                LastError = $@"Error al leer parámetros iniciales: {ex.Message}";
                OK = false;
            }
            return (OK);
        }


        public Boolean Log(mDatos Datos, String Texto)
        {
            Boolean OK = true;
            System.IO.StreamWriter sr;

            try
            {
                sr = new System.IO.StreamWriter($@"{Datos.Folder}\{Datos.Log}", true);
                sr.WriteLine($@"{Datos.Version}  {DateTime.Now.ToString("dd.MM.yy HH:mm:ss")}   {Texto}");
                sr.Close();
            }
            catch (Exception ex)
            {
                OK = false;
                LastError = $@"Error Apertura Log: {ex.Message}";
            }

            return (OK);
        }

        public Boolean Guardar_xml(mDatos Datos, string FileName, string xml, string AlmacenCode)
        {
            Boolean OK = true;
            System.IO.StreamWriter sr;
            Funciones f = new Funciones();
            string Carpeta = $@"{Datos.Folder}\xml\{AlmacenCode}";

            if (! System.IO.Directory.Exists(Carpeta)) System.IO.Directory.CreateDirectory(Carpeta);

            FileName += $"_{DateTime.Now.ToString("dd_MM_yy HH_mm_ss")}.xml";
            try
            {
                sr = new System.IO.StreamWriter($@"{Datos.Folder}\xml\{AlmacenCode}\{FileName}", true);
                sr.Write(xml);
                sr.Close();
            }
            catch (Exception ex)
            {
                OK = false;
                f.Log(Datos, $@"Error Grabar xml: {ex.Message}");
                LastError = $@"Error Grabar xml: {ex.Message}";
            }

            return (OK);
        }


        public Boolean Articulo_NAV(mDatos Datos, ref Sql Sqlclass, ref mArticuloNav ArticuloNAV, string xCode)
        {
            Boolean OK = true;
            SqlDataReader oRead = null;

            string tt = $@"select [BigBag],[Blocked],[Caducidad],[Caducidad_Minima],[Cajas_Pale],[Caras_Palet_Etiquetar],[Cod_Etiq_Imprenta],
                                  [Cod_Etiq_Imprenta_OLD],[Cod_Fmt_Inyector],[Cod_Formato],[Código_envase],[Codigo_Fabricante],[Colores],
                                  [Descripción ampliada],[Description],[DUN14],[EAN13],[EtiquetaEmbalajeManPath],[EtiquetaEmbalajePath],[EtiquetaPiezaPath],
                                  [Etq_Caja],[Etq_Inyector],[Etq_Palet],[Etq_Saco],[Forma],[Fórmula],[Ignorar],[Kg_Palet],[Kg_Palet SGA],[Kg_Saco],
                                  [Kg_Saco SGA],[Kilos_caja],[Medidas],[No_],[Notas],[Sacos_Caja],[Sacos_Palet],[Var_logistica],[Visado]
                                  FROM [{Datos.Company}$Articulos] WHERE [No_] = '{xCode}'";
            if (Sqlclass.Crear_Datareader(ref Datos, ref oRead, tt))
            {
                if (oRead.Read())
                {
                    ArticuloNAV = new mArticuloNav();

                    try
                    {
                        ArticuloNAV.BigBag = ( oRead.GetByte(0) !=0);
                        ArticuloNAV.Blocked = (oRead.GetByte(1) != 0);
                        ArticuloNAV.Caducidad = oRead.GetInt32(2);
                        ArticuloNAV.Caducidad_Minima = oRead.GetInt32(3);
                        ArticuloNAV.Cajas_Pale = oRead.GetDecimal(4);
                        ArticuloNAV.Caras_Palet_Etiquetar = oRead.GetInt32(5);
                        ArticuloNAV.Cod_Etiq_Imprenta = oRead.GetString(6);
                        ArticuloNAV.Cod_Etiq_Imprenta_OLD = oRead.GetString(7);
                        ArticuloNAV.Cod_Fmt_Inyector = oRead.GetString(8);
                        ArticuloNAV.Cod_Formato = oRead.GetString(9);
                        ArticuloNAV.Codigo_envase = oRead.GetString(10);
                        ArticuloNAV.Codigo_Fabricante = oRead.GetString(11);
                        ArticuloNAV.Colores = oRead.GetString(12);
                        ArticuloNAV.Descripcion_ampliada = oRead.GetString(13);
                        ArticuloNAV.Description = oRead.GetString(14);
                        ArticuloNAV.DUN14 = oRead.GetString(15);
                        ArticuloNAV.EAN13 = oRead.GetString(16);
                        ArticuloNAV.EtiquetaEmbalajeManPath = oRead.GetString(17);
                        ArticuloNAV.EtiquetaEmbalajePath = oRead.GetString(18);
                        ArticuloNAV.EtiquetaPiezaPath = oRead.GetString(19);
                        ArticuloNAV.Etq_Caja = (oRead.GetByte(20) != 0);
                        ArticuloNAV.Etq_Inyector = (oRead.GetByte(21) != 0);
                        ArticuloNAV.Etq_Palet = (oRead.GetByte(22) != 0);
                        ArticuloNAV.Etq_Saco = (oRead.GetByte(23) != 0);//////
                        ArticuloNAV.Forma = oRead.GetString(24);
                        ArticuloNAV.Formula = oRead.GetString(25);
                        ArticuloNAV.Ignorar = (oRead.GetByte(26) != 0);
                        ArticuloNAV.Kg_Palet = oRead.GetDecimal(27);
                        ArticuloNAV.Kg_Palet_SGA = oRead.GetDecimal(28);
                        ArticuloNAV.Kg_Saco = oRead.GetDecimal(29);
                        ArticuloNAV.Kg_Saco_SGA = oRead.GetDecimal(30);
                        ArticuloNAV.Kilos_caja = oRead.GetDecimal(31);
                        ArticuloNAV.Medidas = oRead.GetString(32);
                        ArticuloNAV.No_ = oRead.GetString(33);
                        ArticuloNAV.Notas = oRead.GetString(34);
                        ArticuloNAV.Sacos_Caja = oRead.GetDecimal(35);
                        ArticuloNAV.Sacos_Palet = oRead.GetDecimal(36);
                        ArticuloNAV.Var_logistica = oRead.GetString(37);
                        ArticuloNAV.Visado = (oRead.GetByte(38) != 0);
                    }
                    catch (Exception ex)
                    {
                        OK = false;
                        Log(Datos, $@"Error Conversión Articulo {ex.Message}");
                    }
                }
            }

            return (OK);
        }
    }
}

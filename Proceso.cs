using NavLogistica24.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Net;

namespace NavLogistica24
{
    public class Proceso
    {
        public async Task Bucle(mDatos Datos, string Opcion)
        {
            ObservableCollection<mAlmacenes> Almacenes = null;

            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(Datos.Delay));
            do
            {
                Cargar_Almacenes(ref Datos, ref Almacenes);

                if (Opcion == "CLIENTES" || Opcion == "TODOS") await Clientes(Datos, Almacenes);
                if (Opcion == "PROVEEDORES" || Opcion == "TODOS") await Proveedores(Datos, Almacenes);
                if (Opcion == "ARTICULOS" || Opcion == "TODOS") await Articulos(Datos, Almacenes);
                if (Opcion == "EAN" || Opcion == "TODOS") await EAN(Datos, Almacenes);
                if (Opcion == "OC" || Opcion == "TODOS") await OC(Datos, Almacenes);
                if (Opcion == "PS" || Opcion == "TODOS") await PS(Datos, Almacenes);
                if (Opcion == "DEV-P" || Opcion == "TODOS") await Dev_P(Datos, Almacenes);
                if (Opcion == "DEV-C" || Opcion == "TODOS") await Dev_C(Datos, Almacenes);
                if (!Datos.BucleInfinito) break;
            }
            while (await timer.WaitForNextTickAsync());

        }

        private bool Cargar_Almacenes(ref mDatos Datos, ref ObservableCollection<mAlmacenes> Almacenes)
        {
            SqlDataReader Reader = null;
            Sql SqlNav = new Sql();
            string tt = string.Empty;
            bool sw9 = false;

            Almacenes = new ObservableCollection<mAlmacenes>();

            tt = $"SELECT [Code], [URL Inlog] FROM [{Datos.Company}$Location] WHERE [URL Inlog] <> ''";
            sw9 = SqlNav.Crear_Datareader(ref Datos, ref Reader, tt);
            if (sw9) sw9 = Reader.HasRows;

            if (sw9)
            {
                while (Reader.Read())
                {
                    mAlmacenes Location = new mAlmacenes();

                    Location.Codigo = Reader.GetString(0);
                    Location.URL = Reader.GetString(1);

                    Almacenes.Add(Location);
                }

                Reader.Close();
            }

            return sw9;
        }

        private async Task Clientes(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            Datos = await Transfer.Clientes(Datos, Almacenes);

            if (Datos.Counter > 0) f.Log(Datos, "", $@"Clientes Traspasados: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
        }

        private async Task Proveedores(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            Datos = await Transfer.Proveedores(Datos, Almacenes);

            if (Datos.Counter > 0) f.Log(Datos,"", $@"Proveedores Traspasados: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
        }

        private async Task Articulos(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            Datos = await Transfer.Articulos(Datos, Almacenes);

            if (Datos.Counter > 0) f.Log(Datos, "", $@"Articulos Traspasados: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
        }

        private async Task EAN(mDatos Datos,ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            Datos = await Transfer.CodigosBarras(Datos, Almacenes);

            if (Datos.Counter > 0) f.Log(Datos,"", $@"Códigos de Barras: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
        }

        private async Task OC(mDatos Datos, ObservableCollection<mAlmacenes>Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            foreach (mAlmacenes Almacen in Almacenes)
            {
                Datos = await Transfer.OC(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo, $@"Órdenes de Compra: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");

                Datos = await Transfer.Transfer_Entrada(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo, $@"Transfer Entrada: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
            }

        }

        private async Task PS(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            foreach (mAlmacenes Almacen in Almacenes)
            {
                Datos = await Transfer.PS(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo, $@"Pedidos de Salida: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");

                Datos = await Transfer.Transfer_Salida(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo, $@"Transfer Salida: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
            }
        }

        private async Task Dev_P(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            foreach (mAlmacenes Almacen in Almacenes)
            {
                Datos = await Transfer.DevPro(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo, $@"Dev. Proveedor: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
            }
        }

        private async Task Dev_C(mDatos Datos, ObservableCollection<mAlmacenes> Almacenes)
        {
            Funciones f = new Funciones();
            Enviar Transfer = new Enviar();

            foreach (mAlmacenes Almacen in Almacenes)
            {
                Datos = await Transfer.DevCli(Datos, Almacen);

                if (Datos.Counter > 0) f.Log(Datos, Almacen.Codigo,$@"Dev. Cliente: {(Datos.Counter - Datos.Errores).ToString("#,##0")}  Errores:{Datos.Errores.ToString("#,##0")}");
            }
        }

    }
}

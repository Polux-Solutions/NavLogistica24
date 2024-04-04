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

namespace NavLogistica24
{
    public class Sql
    {
        private SqlConnection SqlConn;

        private bool Abrir_BBDD(ref mDatos Datos)
        {
            bool Abrir = true;

            if (SqlConn == null) SqlConn = new SqlConnection();

            if (SqlConn.State != ConnectionState.Open)
            {
                try
                {
                    string Cadena = "Server= " + Datos.Server + ";database=" + Datos.Database + ";User Id=" + Datos.User + ";Password=" + Datos.Password + ";MultipleActiveResultSets=true;ConnectRetryCount=20;timeout=36000";

                    SqlConn = new SqlConnection(Cadena);
                    SqlConn.Open();
                }
                catch (Exception ex)
                {
                    Datos.Estado = "Error apertura conexión Base de Datos: " + ex.Message;
                    Abrir = false;
                }
            }

            return Abrir;
        }


        private bool Cerrar_BBDD(ref mDatos Datos)
        {
            bool Cerrar = true;

            try
            {
               if (SqlConn != null)
                {
                    if (SqlConn.State != ConnectionState.Open) SqlConn.Close();
                }
            }
            catch( Exception Ex)
            {
                Datos.Estado = "Error Cerrar conexión Base de Datos: " + Ex.Message;
                Cerrar = false;
            }

            return Cerrar;
        }

        public bool Crear_Datareader(ref mDatos Datos,  ref SqlDataReader oRead, string tt)
        {
            bool Crear = true;

            Crear =  Abrir_BBDD(ref Datos);
            
            if (Crear)
            {
                try
                {
                    SqlCommand oComm = new SqlCommand();
                    oComm.Connection = SqlConn;
                    oComm.CommandText = tt;
                    oRead = oComm.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Datos.Estado = "Error al crear datareader: " + ex.Message + "\n" + "Sql: "+ tt;
                    Crear = false;
                }
            }

            return Crear;
        }

        public bool Ejecutar_SQL(ref mDatos Datos,  string tt)
        {
            bool Ejecutar = true;

            Ejecutar = Abrir_BBDD(ref Datos);

            if (Ejecutar)
            {
                try
                {
                    SqlCommand oComm = new SqlCommand();
                    oComm.Connection = SqlConn;
                    oComm.CommandText = tt;
                    oComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Datos.Estado = "Error al ejecutar SQL: " + ex.Message + "\n" + " SQL: " + tt;
                    Ejecutar = false;
                }
            }

            return Ejecutar;
        }
    }
}

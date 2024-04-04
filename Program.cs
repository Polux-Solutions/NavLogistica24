// See https://aka.ms/new-console-template for more information
using NavLogistica24.Modelos;
using NavLogistica24;
using System.Diagnostics;
using System.Drawing.Text;

mDatos Datos = new mDatos();
Funciones f = new Funciones();

if (!f.Leer_Parametros(ref Datos))
{
    Console.WriteLine(f.LastError);
    return;
}


string Opcion = "";
if (Environment.GetCommandLineArgs().Length > 1)
{
    if (Environment.GetCommandLineArgs()[1].ToUpper() == "RUN")
    {
        Datos.BucleInfinito = true;
        Opcion = "TODOS";
    }
}

Proceso p = new Proceso();
f.KillAll();

if (Opcion == "") Opcion = f.Menu(ref Datos);
await p.Bucle(Datos, Opcion);







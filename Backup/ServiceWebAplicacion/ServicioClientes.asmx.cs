using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data;

namespace ServiceWebAplicacion
{
    
    /// <summary>
    /// Descripción breve de ServicioClientes //[WebService(Namespace = "http://suarpe.com/")]   //puedes cambiar esta direccion
    /// </summary>
    [WebService(Namespace = "http://localhost:5303/")]   //puedes cambiar esta direccion
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    //[System.Web.Script.Services.ScriptService]

    public class ServicioClientes : System.Web.Services.WebService
    {
        //hace referencia a la clase conexion, ahi esta la cadena de conexion y nuestros metodos
        Conexion con = new Conexion();

       
        [WebMethod]
        public string HelloWorld()
        {

            return "Hello World";
        }
         

        [WebMethod]
        public String LoginUsuario(string user, String password)
        {
            string msje = "";
            msje = con.InicioSesion(user,password);

            return msje;
        }


    }
}

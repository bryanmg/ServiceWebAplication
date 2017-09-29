using System;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;       

namespace ServiceWebAplicacion
{
    
    /// <summary>
    [WebService(Namespace = "http://colorganic-002-site5.itempurl.com/")]   //puedes cambiar esta direccion
    //[WebService(Namespace = "http://localhost/ServicioClientes.asmx")]   //puedes cambiar esta direccion
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    //[System.Web.Script.Services.ScriptService]

    public class ServicioClientes : System.Web.Services.WebService
    {
        //hace referencia a la clase conexion, ahi esta la cadena de conexion y nuestros metodos
        Conexion con = new Conexion();  

        [WebMethod]
        public String CatalogoJSONid()
        {
            string json = "";
            json = con.CatalogoJSONid();
            return json;
        }
        [WebMethod]
        public String CatalogoJSONdes()
        {
            string json = "";
            json = con.CatalogoJSONdes();
            return json;
        }
        
        [WebMethod]
        public string InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, String Image1, String Image2, String Image3, String Image4, String Image5, String Image6, Byte[] video)
        {
            string retorno = "";
            retorno = con.InsertarRegistro(catId, NumSafety, descripcion, latitud, longitud, usuario, Image1, Image2, Image3, Image4, Image5, Image6, video);

            return retorno;
        }

        [WebMethod]
        public String LoginUsuario(string user, String password)
        {
            string msj = "";
            msj = con.InicioSesion(user,password);

            return msj;
        }

        [WebMethod]
        public string CargaAlerta(string usu, string latitud, string longitud, string Image1, string Image2, String Image3)
        {
            string msje = "";
            msje = con.CargaAlerta(usu, latitud, longitud,  Image1,  Image2, Image3);

            return msje;
        }   

        [WebMethod]
        public string GetFile(String nameVideo, byte[] dataVideo)
        {                              
            try
            {                                  
                String arquivo = Server.MapPath("~/VideoData/") + nameVideo;  
                /*CREANDO ARCHIVO Y GUARDANDO EN DIRECTORIO*/
                File.WriteAllBytes(arquivo, dataVideo);   
            }
            catch (Exception ex)
            {     
                return ex.Message ;  
            }       
            return "1"; 
        }
    }
}

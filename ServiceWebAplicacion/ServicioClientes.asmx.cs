using System;
using System.IO;
using System.Web.Services;
using System.Text;
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
        public string InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, byte imgCount, byte vdCount/*, String nameVideo, byte[] dataVideo*/)
        {
            string retorno = "";   
            //GUARDANDO EL VIDEO      
            try{
                /*CREANDO ARCHIVO Y GUARDANDO EN DIRECTORIO*/       
                for (int i = (imgCount+1); i<=(imgCount+vdCount); i++) {
                    string nameVideo = usuario +"_"+ i+"_"+ Convert.ToString(DateTime.Now);
                    switch (i) {
                        case 1:
                            byte[] Video1 = Encoding.ASCII.GetBytes(Data1);
                            String arquivo1 = Server.MapPath("~/VideoData/") + nameVideo; //Nombre del Video
                            File.WriteAllBytes(arquivo1, Video1);   //se carga el video al servidor
                            Data1 = nameVideo; //para enviarlo a la BD
                            break;
                        case 2:
                            byte[] Video2 = Encoding.ASCII.GetBytes(Data2);
                            String arquivo2 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo2, Video2);
                            Data2 = nameVideo;
                            break;
                        case 3:
                            byte[] Video3 = Encoding.ASCII.GetBytes(Data3);
                            String arquivo3 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo3, Video3);
                            Data3 = nameVideo;
                            break;
                        case 4:
                            byte[] Video4 = Encoding.ASCII.GetBytes(Data4);
                            String arquivo4 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo4, Video4);
                            Data4 = nameVideo;
                            break;
                        case 5:
                            byte[] Video5 = Encoding.ASCII.GetBytes(Data5);
                            String arquivo5 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo5, Video5);
                            Data5 = nameVideo;
                            break;
                        case 6:
                            byte[] Video6 = Encoding.ASCII.GetBytes(Data6);
                            String arquivo6 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo6, Video6);
                            Data6 = nameVideo;
                            break;
                    }
                } 
                retorno = con.InsertarRegistro(catId, NumSafety, descripcion, latitud, longitud, usuario, Data1, Data2, Data3, Data4, Data5, Data6, imgCount, vdCount);
                //return retorno;
                if (retorno.Equals("1")) {
                    return retorno;
                }else{
                    for (int i = (imgCount + 1); i <= (imgCount + vdCount); i++)
                    {
                        //Si existe error elimino los videos del Servidor
                        string file;
                        switch (i)
                        {
                            case 1:
                                file = "~/VideoData/" + Data1;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                            case 2:
                                file = "~/VideoData/" + Data2;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                            case 3:
                                file = "~/VideoData/" + Data3;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                            case 4:
                                file = "~/VideoData/" + Data4;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                            case 5:
                                file = "~/VideoData/" + Data5;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                            case 6:
                                file = "~/VideoData/" + Data6;
                                if (File.Exists(file))
                                    File.Delete(file);
                                break;
                        }
                    }
                    return "0";
                }
            }
            catch (Exception ex){
                return ex.Message;
            }          
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

        /*[WebMethod]
        public string GetFile(String nameVideo, byte[] dataVideo)
        {                              
            try{                                  
                String arquivo = Server.MapPath("~/VideoData/") + nameVideo;  
                /*CREANDO ARCHIVO Y GUARDANDO EN DIRECTORIO*
                File.WriteAllBytes(arquivo, dataVideo);
                return "1";
            }
            catch (Exception ex) {     
                return ex.Message ;  
            }   
        }  */
    }
}

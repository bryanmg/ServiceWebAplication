using System;
using System.IO;
using System.Web.Services;
using System.Text;                
using System.Web;

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
        public string InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, byte imgCount, byte vdCount)
        {
            //Directory.CreateDirectory(Server.MapPath("~/VideoData/"));
            string retorno = "";   
            //GUARDANDO EL VIDEO      
            try{                 
                string pathToCreate = "~/VideoData/";
                if ( Directory.Exists(Server.MapPath(pathToCreate)) ){}
                else {
                    Directory.CreateDirectory(Server.MapPath(pathToCreate));
                }                          

                /*CREANDO ARCHIVO Y GUARDANDO EN DIRECTORIO*/
                
                for (int i = (imgCount+1); i<=(imgCount+vdCount); i++) {
                    //Random rnd = new Random();
                    string nameVideo = usuario.ToString() + "_" + i + "_" + DateTime.Now.ToString("dd-MM-yyyy H-mm-ss") + ".mp4";
                    //string nameVideo = usuario.ToString() +"_"+ i+"_"+Convert.ToString(rnd.Next(1,100))+".mp4";
                    switch (i) {
                        case 1:
                            byte[] Video1 = Encoding.ASCII.GetBytes(Data1);
                            String arquivo1 = Server.MapPath("~/VideoData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                            //File.WriteAllBytes(arquivo1, Video1); //se carga el video al servidor
                            //var path = Server.MapPath("~/App_Data/file.txt");
                            File.SetAttributes(arquivo1, FileAttributes.Normal);
                            File.WriteAllBytes(arquivo1, Video1); //se carga el video al servidor
                            /*using (StreamWriter _testData = new StreamWriter(Server.MapPath("~/VideoData/"), true))
                            {
                                
                                _testData.WriteLine(Video1); // Write the file.
                            }  */
                            
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
                                if (File.Exists(HttpContext.Current.Server.MapPath(file))){
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }                        
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
        public string CargaAlerta(string usu, string descripcion, string latitud, string longitud, String Image1, String Image2, String Image3)
        {
            string msje = "";
            msje = con.CargaAlerta(usu, descripcion, latitud, longitud,  Image1,  Image2, Image3);

            return msje;
        }

        [WebMethod]
        public String Find_QPhone(String date, String description, String Nosafety, int selection)
        {
            String data;
            data = con.Find_QPhone(date, description, Nosafety, selection);
            return data;
        }

        [WebMethod]
        public String Picture_Alerts(String id)
        {
            String data;
            data = con.Picture_Alerts(id);
            return data;
        }
    }
}

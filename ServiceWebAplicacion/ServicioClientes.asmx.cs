﻿using System;
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
    

    public class ServicioClientes : WebService
    {   
        //hace referencia a la clase conexion, ahi esta la cadena de conexion y nuestros metodos
        Conexion con = new Conexion();    
         
        [WebMethod]
        public String CatalogoJSON()
        {
            string json = "";
            json = con.CatalogoJSON();
            return json;
        }  
        
        [WebMethod]
        public string InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, byte[] Data1, byte[] Data2, byte[] Data3, byte[] Data4, byte[] Data5, byte[] Data6, int imgCount, int vdCount)
        {
            //Directory.CreateDirectory(Server.MapPath("~/VideoData/"));
            string retorno;
            int media = 0;
            if (imgCount > 0) { media = 1; }
            if (vdCount > 0) { media = 2; }
            if (imgCount > 0 && vdCount > 0) { media = 3; }                       

            //GUARDANDO EL VIDEO      
            try
            {      
                /*CREANDO ARCHIVO Y GUARDANDO EN DIRECTORIO*/

                for (int i = (imgCount+1); i<=(imgCount+vdCount); i++) {                
                    string nameVideo = usuario.ToString() + "_" + i + "_" + DateTime.Now.ToString("dd-MM-yyyy H-mm-ss") + ".mp4";
        
                    switch (i) {
                        case 1:
                            //byte[] Video1 = Encoding.ASCII.GetBytes(Data1);
                            String arquivo1 = Server.MapPath("~/VideoData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                            File.WriteAllBytes(arquivo1, Data1); //se carga el video al servidor  
                            //Data1 = nameVideo; //para enviarlo a la BD 
                            break;
                        case 2:
                            //byte[] Video2 = Encoding.ASCII.GetBytes(Data2);
                            String arquivo2 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo2, Data2);
                            //Data2 = nameVideo;                 
                            break;
                        case 3:
                            //byte[] Video3 = Encoding.ASCII.GetBytes(Data3);
                            String arquivo3 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo3, Data3);
                            //Data3 = nameVideo;
                            break;
                        case 4:
                            //byte[] Video4 = Encoding.ASCII.GetBytes(Data4);
                            String arquivo4 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo4, Data4);
                            //Data4 = nameVideo;
                            break;
                        case 5:
                            //byte[] Video5 = Encoding.ASCII.GetBytes(Data5);
                            String arquivo5 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo5, Data5);
                            //Data5 = nameVideo;
                            break;
                        case 6:
                            //byte[] Video6 = Encoding.ASCII.GetBytes(Data6);
                            String arquivo6 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo6, Data6);
                            //Data6 = nameVideo;
                            break;
                    }
                }
                retorno = con.InsertarRegistro(catId, NumSafety, descripcion, latitud, longitud, usuario, media, Convert.ToBase64String(Data1), Convert.ToBase64String(Data2), Convert.ToBase64String(Data3), Convert.ToBase64String(Data4), Convert.ToBase64String(Data5), Convert.ToBase64String(Data6), imgCount, vdCount);
                   
                if (Convert.ToInt32(retorno) == 1) {
                    return Convert.ToString(retorno);
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
                    return retorno;
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
        public string CargaAlerta(int usu, String descripcion, String latitud, String longitud, String Image1, String Image2, String Image3)
        {
            string res = "";
            int media = 0;
            if (Image1 != null) { media = 1; }
            res = con.CargaAlerta(usu, descripcion, latitud, longitud, media,  Image1,  Image2, Image3);
            return res;
        }

        [WebMethod]
        public String Find_QPhone(String date, String description, String Nosafety, int selection)
        {
            String data;
            data = con.Find_QPhone(date, description, Nosafety, selection);
            return data;
        }

        [WebMethod]
        public String Picture_Alerts(String id, int flag)
        {
            String data;
            data = con.Picture_Alerts(id, flag);
            return data;
        }
    }
}

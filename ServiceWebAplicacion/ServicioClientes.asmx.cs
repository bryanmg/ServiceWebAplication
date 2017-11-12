using System;
using System.IO;
using System.Web.Services;               
using System.Web;
using System.Net;        
using Newtonsoft.Json;           
using System.Data;
using System.Collections;

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
            string retorno;
            int media = 0;
            if (imgCount > 0) { media = 1; }
            if (vdCount > 0) { media = 2; }
            if (imgCount > 0 && vdCount > 0) { media = 3; }

            //GUARDANDO EL VIDEO      
            try
            {
                string newData1 = Convert.ToBase64String(Data1), newData2 = Convert.ToBase64String(Data2), newData3 = Convert.ToBase64String(Data3),
                    newData4 = Convert.ToBase64String(Data4), newData5 = Convert.ToBase64String(Data5), newData6 = Convert.ToBase64String(Data6);
                string NMImg_Aux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                /*CREANDO IMAGEN Y GUARDANDO EN DIRECTORIO*/
                for (int i = 0; i < imgCount; i++)
                {
                    string nameVideo = usuario.ToString() + "_" + i + "_" + NMImg_Aux  + ".jpg";    
                    switch (i)
                    {
                        case 0:
                            String arquivo1 = Server.MapPath("~/ImageData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                            File.WriteAllBytes(arquivo1, Data1); //se carga el video al servidor  
                            newData1 = nameVideo; //para enviarlo a la BD 
                            break;
                        case 1:
                            String arquivo2 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo2, Data2);
                            newData2 = nameVideo;
                            break;
                        case 2:
                            String arquivo3 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo3, Data3);
                            newData3 = nameVideo;
                            break;
                        case 3:
                            String arquivo4 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo4, Data4);
                            Convert.ToBase64String(Data1);
                            newData4 = nameVideo;
                            break;
                        case 4:
                            String arquivo5 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo5, Data5);
                            newData5 = nameVideo;
                            break;
                        case 5:
                            String arquivo6 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo6, Data6);
                            newData6 = nameVideo;
                            break;
                    }
                }

                string NMVid_Aux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                /*CREANDO VIDEO Y GUARDANDO EN DIRECTORIO*/
                for (int i = (imgCount + 1); i <= (imgCount + vdCount); i++)
                {
                    string nameVideo = usuario.ToString() + "_" + i + "_" + NMVid_Aux + ".mp4";
                    //string nameVideo = "pueba" + Convert.ToString(i) + ".mp4";
                    switch (i)
                    {
                        case 1:
                            String arquivo1 = Server.MapPath("~/VideoData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                            File.WriteAllBytes(arquivo1, Data1); //se carga el video al servidor  
                            newData1 = nameVideo; //para enviarlo a la BD 
                            break;
                        case 2:
                            String arquivo2 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo2, Data2);
                            newData2 = nameVideo;
                            break;
                        case 3:
                            String arquivo3 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo3, Data3);
                            newData3 = nameVideo;
                            break;
                        case 4:
                            String arquivo4 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo4, Data4);
                            Convert.ToBase64String(Data1);
                            newData4 = nameVideo;
                            break;
                        case 5:
                            String arquivo5 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo5, Data5);
                            newData5 = nameVideo;
                            break;
                        case 6:
                            String arquivo6 = Server.MapPath("~/VideoData/") + nameVideo;
                            File.WriteAllBytes(arquivo6, Data6);
                            newData6 = nameVideo;
                            break;
                    }
                }  

                retorno = con.InsertarRegistro(catId, NumSafety, descripcion, latitud, longitud, usuario, media, newData1, newData2, newData3, newData4, newData5, newData6, imgCount, vdCount);

                string file;
                if (Convert.ToInt32(retorno) == 1)
                {
                    return Convert.ToString(retorno);
                }
                else
                {
                    for (int i = (imgCount + 1); i <= (imgCount + vdCount); i++)
                    {
                        //Si existe error elimino los videos del Servidor          
                        switch (i)
                        {
                            case 1:
                                file = "~/VideoData/" + newData1;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 2:
                                file = "~/VideoData/" + newData2;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 3:
                                file = "~/VideoData/" + newData3;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 4:
                                file = "~/VideoData/" + newData4;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 5:
                                file = "~/VideoData/" + newData5;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 6:
                                file = "~/VideoData/" + newData6;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                        }
                    }
                    //ELIMINACION DE IMAGEN DEL SERVIDOR
                    for (int J = 0; J <= imgCount; J++)
                    {
                        switch (J)
                        {
                            case 1:
                                file = "~/ImageData/" + newData1;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 2:
                                file = "~/ImageData/" + newData2;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 3:
                                file = "~/ImageData/" + newData3;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 4:
                                file = "~/ImageData/" + newData4;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 5:
                                file = "~/ImageData/" + newData5;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                            case 6:
                                file = "~/ImageData/" + newData6;
                                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                                {
                                    File.Delete(HttpContext.Current.Server.MapPath(file));
                                }
                                break;
                        }
                    }
                    return retorno;
                }
            }
            catch (Exception ex)
            {
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
        public string CargaAlerta(int usu, String descripcion, String latitud, String longitud, byte[] Image1, byte[] Image2, byte[] Image3)
        {
            string res = "";
            int media = 0;
            if (Image1 != null) { media = 1; }

            string newData1 = Convert.ToBase64String(Image1), newData2 = Convert.ToBase64String(Image2), newData3 = Convert.ToBase64String(Image3);

            if (Image1 != null) {
                string nameAux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                string nameVideo = usu.ToString() + "_1_" + nameAux + ".jpg";
                String arquivo1 = Server.MapPath("~/ImageData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                File.WriteAllBytes(arquivo1, Image1); //se carga el video al servidor  
                newData1 = nameVideo; //para enviarlo a la BD 

                if (Image2 != null) {
                    nameVideo = usu.ToString() + "_2_" + nameAux + ".jpg";
                    String arquivo2 = Server.MapPath("~/ImageData/") + nameVideo;
                    File.WriteAllBytes(arquivo2, Image2);
                    newData2 = nameVideo;

                    if (Image3 != null) {
                        nameVideo = usu.ToString() + "_3_" + nameAux + ".jpg";
                        String arquivo3 = Server.MapPath("~/ImageData/") + nameVideo;
                        File.WriteAllBytes(arquivo3, Image3);
                        newData3 = nameVideo;
                    }
                }
            } 

            res = con.CargaAlerta(usu, descripcion, latitud, longitud, media, newData1, newData2, newData3);
            if (Convert.ToInt32(res) == 1)
            {
                return Convert.ToString(res);
            }
            else
            {
                if (newData1 != null)
                {
                    string file = "~/ImageData/" + newData1;
                    if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(file));
                    }

                    if (newData2 != null)
                    {
                        file = "~/ImageData/" + newData2;
                        if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(file));
                        }

                        if (newData3 != null)
                        {
                            file = "~/ImageData/" + newData3;
                            if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                            {
                                File.Delete(HttpContext.Current.Server.MapPath(file));
                            }
                        }
                    }
                }
                return res;
            }
            
        }

        [WebMethod]
        public String Find_QPhone(String date, String description, String Nosafety, int selection)
        {
            String data;
            data = con.Find_QPhone(date, description, Nosafety, selection);
            return data;
        }

        [WebMethod]
        public String[] Picture_Alerts(String id, int flag)
        {
            string[] imgs = new string[5];
            ArrayList list = new ArrayList();
            ArrayList binFile = new ArrayList();
             

            DataSet myDataSet = con.Picture_Alerts(id, flag);
                                                   
            DataTable firstTable = myDataSet.Tables[0];    
            for (int i = 0; i < firstTable.Rows.Count; i++)
            {
                using (var w = new WebClient())
                {
                    
                    try
                    {
                        binFile.Add(Convert.ToBase64String(w.DownloadData("http://colorganic-002-site5.itempurl.com/ImageData/" + firstTable.Rows[i]["IMIN_Imagene"].ToString())));

                        //string base64String = Convert.ToBase64String(binFile);
                        //list.Add(JsonConvert.SerializeObject(base64String));
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            for (int i = 0; i < binFile.Count; i++)
            {
                imgs[i] = JsonConvert.SerializeObject(binFile[i]);

            }
            return imgs;
        }    
    
    }
}

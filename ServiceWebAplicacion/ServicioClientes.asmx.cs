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
            //if (Image1 != null) { media = 1; }

            string newData1 = Convert.ToBase64String(Image1), newData2 = Convert.ToBase64String(Image2), newData3 = Convert.ToBase64String(Image3);

            if (Image1 != null) {
                string nameAux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                string nameVideo = usu.ToString() + "_1_" + nameAux + ".jpg";
                String arquivo1 = Server.MapPath("~/ImageData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                File.WriteAllBytes(arquivo1, Image1); //se carga el video al servidor  
                newData1 = nameVideo; //para enviarlo a la BD 
                media = 1;

                if (Image2 != null) {
                    nameVideo = usu.ToString() + "_2_" + nameAux + ".jpg";
                    String arquivo2 = Server.MapPath("~/ImageData/") + nameVideo;
                    File.WriteAllBytes(arquivo2, Image2);
                    newData2 = nameVideo;
                    media = 2;

                    if (Image3 != null) {
                        nameVideo = usu.ToString() + "_3_" + nameAux + ".jpg";
                        String arquivo3 = Server.MapPath("~/ImageData/") + nameVideo;
                        File.WriteAllBytes(arquivo3, Image3);
                        newData3 = nameVideo;
                        media = 3;
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
        public String Picture_Alerts(String id, int flag)
        {                                    
            ArrayList binFile = new ArrayList();
            DataSet myDataSet = con.Picture_Alerts(id, flag);
                                                   
            DataTable firstTable = myDataSet.Tables[0];    
            for (int i = 0; i < firstTable.Rows.Count; i++)
            {
                using (var w = new WebClient())
                {
                    
                    try
                    {
                        /*ERROR PRESENTADO EN ESTA SECCION PUES LA TABLA DE INCIDENTES RETORNABA EL NOMBRE DE LAS 
                         * IMAGENES EN UNA COLUMNA LLAMADA "IMIN_Imagene" Y LA TABLA DE ALERTAS EN UNA COLUMNA
                         * LLAMADA "Alert_Imagen"; CUANDO SE BUSCABA UNA ALERTA EL NOMBRE DE LA COLUMNA NO COINCIDIA
                         * ==CON LA SIGUIENTE VALIDACION DEBERIA QUEDAR RESUELTO==*/
                        if(flag == 0){
                            //BUSCA LA IMAGEN EN EL SERVIDOR Y LO DESCARGA
                            binFile.Add(Convert.ToBase64String(w.DownloadData("http://colorganic-002-site5.itempurl.com/ImageData/" + firstTable.Rows[i]["IMIN_Imagene"].ToString())));
                            //AHORA myDataSet TIENE EL ARRAY BINARIO CON LA MEDIA EN LUGAR DEL NOMBRE
                            myDataSet.Tables[0].Rows[i]["IMIN_Imagene"] = binFile[i]; 
                        }else if(flag == 1){
                            binFile.Add(Convert.ToBase64String(w.DownloadData("http://colorganic-002-site5.itempurl.com/ImageData/" + firstTable.Rows[i]["Alert_Imagen"].ToString())));
                            myDataSet.Tables[0].Rows[i]["Alert_Imagen"] = binFile[i]; 
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            }       
            string ret = JsonConvert.SerializeObject(myDataSet);
            return ret;
        }

        //METODO PARA RETORNAR AL MOVIL LA MEDIA DE UN INCIDENTE - VERSION 2
        [WebMethod]
        public String Picture_Alerts_V2(String id, int flag)
        {
            ArrayList binFile = new ArrayList();
            DataSet myDataSet = con.Picture_Alerts_V2(id, flag);

            DataTable firstTable = myDataSet.Tables[0];
            for (int i = 0; i < firstTable.Rows.Count; i++)
            {
                using (var w = new WebClient())
                {

                    try
                    {
                        if (flag == 0)
                        {
                            //BUSCA LA IMAGEN EN EL SERVIDOR Y LO DESCARGA
                            binFile.Add(Convert.ToBase64String(w.DownloadData("http://colorganic-002-site5.itempurl.com/ImageData/" + firstTable.Rows[i]["IMIN_Imagene"].ToString())));
                            //AHORA myDataSet TIENE EL ARRAY BINARIO CON LA MEDIA EN LUGAR DEL NOMBRE
                            myDataSet.Tables[0].Rows[i]["IMIN_Imagene"] = binFile[i];
                        }
                        else if (flag == 1)
                        {
                            binFile.Add(Convert.ToBase64String(w.DownloadData("http://colorganic-002-site5.itempurl.com/ImageData/" + firstTable.Rows[i]["Alert_Imagen"].ToString())));
                            myDataSet.Tables[0].Rows[i]["Alert_Imagen"] = binFile[i];
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            string ret = JsonConvert.SerializeObject(myDataSet);
            return ret;
        }

        //metodo para agregar/modifcar imagenes enviadas por el celular
        [WebMethod]
        public string Add_pictures(int id, int usu, int type, string description, byte[] data1, byte[] data2, byte[] data3, byte[] data4, byte[] data5, byte[] data6, int imgCount)//aregar array de array de bytes//
        {
            //CONDICIONES PARA CELULAR... 
            //1 alertas ---- 0 incidentes
            string res;
            //int media = 0;
            String arquivo1 = "";
            string nameData="";
            string name1 = "_", name2 = "_", name3 = "_", name4 = "_", name5 = "_", name6 = "_";
            string[] NamesImages = new string[6];
            
            string nameAux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
            int num = 0;
            string nameImg = (con.find_images_Name(type, id)).ToString();
            if (nameImg != "")
            {
                num = Convert.ToInt32(nameImg.Split('_')[1]);
            }

            //proceso, carga de imagenes al servidor
            if (data1 != null)
            {
                nameData = usu.ToString() + "_" + (num + 1) + "_" + nameAux + ".jpg";
                arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                File.WriteAllBytes(arquivo1, data1); 
                name1 = nameData; //para enviarlo a la BD 

                if (data2 != null)
                {
                    nameData = usu.ToString() + "_" + (num + 2) + "_" + nameAux + ".jpg";
                    arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                    File.WriteAllBytes(arquivo1, data2);
                    name2 = nameData; //para enviarlo a la BD 

                    if (data3 != null)
                    {
                        nameData = usu.ToString() + "_" + (num + 3) + "_" + nameAux + ".jpg";
                        arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                        File.WriteAllBytes(arquivo1, data3);
                        name3 = nameData; //para enviarlo a la BD 

                        if (data4 != null)
                        {
                            nameData = usu.ToString() + "_" + (num + 4) + "_" + nameAux + ".jpg";
                            arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                            File.WriteAllBytes(arquivo1, data4);
                            name4 = nameData; //para enviarlo a la BD 

                            if (data5 != null)
                            {
                                nameData = usu.ToString() + "_" + (num + 5) + "_" + nameAux + ".jpg";
                                arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                                File.WriteAllBytes(arquivo1, data5);
                                name5 = nameData; //para enviarlo a la BD 

                                if (data6 != null)
                                {
                                    nameData = usu.ToString() + "_" + (num + 6) + "_" + nameAux + ".jpg";
                                    arquivo1 = Server.MapPath("~/ImageData/") + nameData;
                                    File.WriteAllBytes(arquivo1, data6);
                                    name6 = nameData; //para enviarlo a la BD 
                                }
                            }
                        }
                    }
                }
            }
            
            res = con.Update_media(id, usu, type, imgCount, description, name1, name2, name3, name4, name5, name6);//aregar array de array de bytes --  
            return res;
        }

        //metodo para cargar mas imagenes enviadas por web
        [WebMethod]
        public string Pictures_WEB(int id, int usu, int number, int type, byte[] data1)
        {
            string res;
            String arquivo1 = "";
            string nameData = "";
            string nameAux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
            
            if (type == 1 || type == 2)// 1 == alertas ::::: 2 == incidentes(Imgs) :::::: 3 incidentes(Vids)
            {//incidentes - alertas
                nameData = usu.ToString() + "_" + number + "_" + nameAux + ".jpg";
                arquivo1 = Server.MapPath("~/ImageData/") + nameData; //Nombre del dato DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
            }
            else if (type == 3)//videos
            {
                nameData = usu.ToString() + "_" + number + "_" + nameAux + ".mp4";
                arquivo1 = Server.MapPath("~/VideoData/") + nameData; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
            }
            File.WriteAllBytes(arquivo1, data1); //se carga el archivo al servidor 
           
            res = con.MoreMedia(id, usu, type, nameData);//aregar array de array de bytes);
            return nameData;
        }

        //metodo para eliminar imagenes por web
        [WebMethod]
        public string DeletPic_WEB(int id, int usu, int type, string name)
        {
            string file;
            if (type == 1)//alertas
            {
                file = "~/ImageData/" + name;
                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(file));
                }
            }
            if (type == 2)//Incidentes_imagen
            {
                file = "~/ImageData/" + name; 
                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(file));
                }
            }
            if (type == 3)//Incidentes_Videos
            {
                file = "~/VideoData/" + name;
                if (File.Exists(HttpContext.Current.Server.MapPath(file)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(file));
                }
            }
            string res = "";
            res = con.DELIMG(id, usu, type, name);
            return res;
        }

        //Metodo para insertar un incidente sin Catalogo de incidentes y sin descripcion
        [WebMethod]
        public string InsertarRegistro_V2(String NumSafety, String latitud, String longitud, int usuario, byte[] Data1, byte[] Data2, byte[] Data3, byte[] Data4, byte[] Data5, byte[] Data6, int imgCount, int vdCount)
        {
            string retorno;
            int media = 0;
            if (imgCount > 0) { media = 1; }
            if (vdCount > 0) { media = 2; }
            if (imgCount > 0 && vdCount > 0) { media = 3; }
            int mediaExist = imgCount + vdCount;

            //GUARDANDO EL VIDEO      
            try
            {
                string newData1 = Convert.ToBase64String(Data1), newData2 = Convert.ToBase64String(Data2), newData3 = Convert.ToBase64String(Data3),
                    newData4 = Convert.ToBase64String(Data4), newData5 = Convert.ToBase64String(Data5), newData6 = Convert.ToBase64String(Data6);
                string NMImg_Aux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                /*CREANDO IMAGEN Y GUARDANDO EN DIRECTORIO*/
                for (int i = 0; i < imgCount; i++)
                {
                    string nameVideo = usuario.ToString() + "_" + i + "_" + NMImg_Aux + ".jpg";
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

                retorno = con.InsertarRegistro_V2( NumSafety, latitud, longitud, usuario, mediaExist, newData1, newData2, newData3, newData4, newData5, newData6, imgCount, vdCount);

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
        public string CargaAlerta_V2(int usu, String descripcion, String latitud, String longitud, byte[] Image1, byte[] Image2, byte[] Image3, byte[] Image4, byte[] Image5, byte[] Image6)
        {
            string res = "";
            int media = 0;
            //if (Image1 != null) { media = 1; }

            string newData1 = Convert.ToBase64String(Image1), newData2 = Convert.ToBase64String(Image2), newData3 = Convert.ToBase64String(Image3);
            string newData4 = Convert.ToBase64String(Image4), newData5 = Convert.ToBase64String(Image5), newData6 = Convert.ToBase64String(Image6);

            if (Image1 != null)
            {
                string nameAux = DateTime.Now.ToString("dd-MM-yyyy_H-mm-ss");
                string nameVideo = usu.ToString() + "_1_" + nameAux + ".jpg";
                String arquivo1 = Server.MapPath("~/ImageData/") + nameVideo; //Nombre del Video DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") 
                File.WriteAllBytes(arquivo1, Image1); //se carga el video al servidor  
                newData1 = nameVideo; //para enviarlo a la BD 
                media = 1;

                if (Image2 != null)
                {
                    nameVideo = usu.ToString() + "_2_" + nameAux + ".jpg";
                    String arquivo2 = Server.MapPath("~/ImageData/") + nameVideo;
                    File.WriteAllBytes(arquivo2, Image2);
                    newData2 = nameVideo;
                    media = 2;

                    if (Image3 != null)
                    {
                        nameVideo = usu.ToString() + "_3_" + nameAux + ".jpg";
                        String arquivo3 = Server.MapPath("~/ImageData/") + nameVideo;
                        File.WriteAllBytes(arquivo3, Image3);
                        newData3 = nameVideo;
                        media = 3;

                        if (Image4 != null)
                        {
                            nameVideo = usu.ToString() + "_4_" + nameAux + ".jpg";
                            String arquivo4 = Server.MapPath("~/ImageData/") + nameVideo;
                            File.WriteAllBytes(arquivo4, Image4);
                            newData4 = nameVideo;
                            media = 4;

                            if (Image5 != null)
                            {
                                nameVideo = usu.ToString() + "_5_" + nameAux + ".jpg";
                                String arquivo5 = Server.MapPath("~/ImageData/") + nameVideo;
                                File.WriteAllBytes(arquivo5, Image5);
                                newData5 = nameVideo;
                                media = 5;

                                if (Image6 != null)
                                {
                                    nameVideo = usu.ToString() + "_6_" + nameAux + ".jpg";
                                    String arquivo6 = Server.MapPath("~/ImageData/") + nameVideo;
                                    File.WriteAllBytes(arquivo6, Image6);
                                    newData6 = nameVideo;
                                    media = 6;
                                }
                            }
                        }
                    }
                }
            }

            res = con.CargaAlerta_V2(usu, descripcion, latitud, longitud, media, newData1, newData2, newData3, newData4, newData5, newData6);
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
    }

}


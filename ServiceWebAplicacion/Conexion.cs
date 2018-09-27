using System;       
using System.Data.SqlClient;
using System.Data;         
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServiceWebAplicacion
{

    public class Conexion
    {               
        SqlConnection con;

        public Conexion()
        {
            if (con == null)
                
        }

        public void Abrir()
        {
            if (con.State == ConnectionState.Closed) con.Open();
        }

        public void Cerrar()
        {
            if (con.State == ConnectionState.Open) con.Close();
        }

        // METODOS
        public String InicioSesion(String nic, String clav)
        {
            String msje = "";
            SqlCommand cmd;
            try
            {
                Abrir();
                cmd = new SqlCommand("InicioSesion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", nic);
                cmd.Parameters.AddWithValue("@clave", clav);
                cmd.Parameters.Add("@msje", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                msje = cmd.Parameters["@msje"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msje;
        }

        public string InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, int media, String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, int imgCount, int vdCount)
        {
            SqlCommand cmd;
            int res = 1;
            string msj = "";
            try
            {
                Abrir();

                cmd = new SqlCommand("Carga_datos", con);//LLAMAMOS AL ESTORED PROCEDURE Y CREAMOS LA CONEXION EN LA BD
                cmd.CommandType = CommandType.StoredProcedure;
                //ASIGNAMOS LOS VALORES QUE LE MANDAREMOS AL METODO EN LA BASE DE DATOS
                cmd.Parameters.AddWithValue("@catId", catId);
                cmd.Parameters.AddWithValue("@NumSafety", NumSafety);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@latitud", latitud);
                cmd.Parameters.AddWithValue("@longitud", longitud);
                cmd.Parameters.AddWithValue("@USU_id", usuario);
                cmd.Parameters.AddWithValue("@media", media);
                //INSERTAMOS TODAS LAS IMAGENES AUNQUE ESTAS SEAN =NULL
                cmd.Parameters.AddWithValue("@Data1", Data1);
                cmd.Parameters.AddWithValue("@Data2", Data2);
                cmd.Parameters.AddWithValue("@Data3", Data3);
                cmd.Parameters.AddWithValue("@Data4", Data4);
                cmd.Parameters.AddWithValue("@Data5", Data5);
                cmd.Parameters.AddWithValue("@Data6", Data6);
                cmd.Parameters.AddWithValue("@imgCount", imgCount);
                cmd.Parameters.AddWithValue("@vdCount", vdCount);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;


                res = cmd.ExecuteNonQuery();//EJECUTAMOS EL METODO Y CACHAMOS EL RESULTADO
                msj = cmd.Parameters["@msj"].Value.ToString();

                //obtieneAutoSerial();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msj;
        }

        public String CatalogoJSON()//metodo para obtener el catalogo de tipos de incidentes y sus descripciones
        {
            var json = "";
            SqlCommand cmd;
            DataSet myDataSet = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                con.Open();
                string sql = "SELECT CAT_Id, CAT_Descripcion FROM CAT_Incidentes_Safety";
                cmd = new SqlCommand(sql, con);

                da.SelectCommand = cmd;
                da.Fill(myDataSet);
                json = JsonConvert.SerializeObject(myDataSet); //se usa la libreria Newtonsoft.Json 
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return json;
        }

        //Carga una alerta 
        public string CargaAlerta(int usu, String descripcion, String latitud, String longitud, int media, String Image1, String Image2, String Image3)
        {
            SqlCommand cmd;
            string msj = "";
            int res;
            try
            {
                Abrir();

                cmd = new SqlCommand("CargaAlerta", con);//LLAMAMOS AL ESTORED PROCEDURE Y CREAMOS LA CONEXION EN LA BD
                cmd.CommandType = CommandType.StoredProcedure;
                //ASIGNAMOS LOS VALORES QUE LE MANDAREMOS AL METODO EN LA BASE DE DATOS
                cmd.Parameters.AddWithValue("@usu", usu);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@latitud", latitud);
                cmd.Parameters.AddWithValue("@longitud", longitud);
                cmd.Parameters.AddWithValue("@media", media);
                //INSERTAMOS TODAS LAS IMAGENES AUNQUE ESTAS SEAN =NULL
                cmd.Parameters.AddWithValue("@Image1", Image1);
                cmd.Parameters.AddWithValue("@Image2", Image2);
                cmd.Parameters.AddWithValue("@Image3", Image3);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                //EJECUTAMOS EL METODO Y CACHAMOS EL RESULTADO
                res = cmd.ExecuteNonQuery();
                msj = cmd.Parameters["@msj"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msj;

        }

        //retorna consultas desde celular
        public String Find_QPhone(String date, String description, String Nosafety, int selection)//metodo para obtener el catalogo de tipos de incidentes y sus descripciones
        {
            DataSet myDataSet = new DataSet();
            string jsonVa;
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("Query_phone", con);
                //ASIGNAMOS LOS VALORES QUE LE MANDAREMOS AL METODO EN LA BASE DE DATOS
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@Nosafety", Nosafety);
                cmd.Parameters.AddWithValue("@selection", selection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(myDataSet);

                jsonVa = JsonConvert.SerializeObject(myDataSet); //se usa la libreria Newtonsoft.Json 
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jsonVa;
        }
        
        //Retorna imagenes de alertas
        public DataSet Picture_Alerts(String id, int flag)
        {
            DataSet myDataSet = new DataSet();
            SqlCommand cmd;
            List<String> Users = new List<String>();
            try
            {
                cmd = new SqlCommand("Return_Picture", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(myDataSet);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myDataSet;
        }

        //Retorna imagenes de alertas version 2
        public DataSet Picture_Alerts_V2(string id, int flag)
        {
            DataSet myDataSet = new DataSet();
            SqlCommand cmd;
            List<String> Users = new List<String>();
            
            try
            {
                cmd = new SqlCommand("Return_Picture_V2", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(myDataSet);
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myDataSet;
        }

        //carga nuevas imagenes al registro desde el celular
        public string Update_media(int id, int usu, int type, int imgCount, string description, string data1, string data2, string data3, string data4, string data5, string data6)//
        {
            SqlCommand cmd;
            string msje;
            try
            {
                Abrir();
                cmd = new SqlCommand("Update_Pictures", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@usu", usu);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@imgCount", imgCount);
                cmd.Parameters.AddWithValue("@description", description);
                //INSERTAMOS TODAS LAS IMAGENES AUNQUE ESTAS SEAN =NULL
                cmd.Parameters.AddWithValue("@Image1", data1);
                cmd.Parameters.AddWithValue("@Image2", data2);
                cmd.Parameters.AddWithValue("@Image3", data3);
                cmd.Parameters.AddWithValue("@Image4", data4);
                cmd.Parameters.AddWithValue("@Image5", data5);
                cmd.Parameters.AddWithValue("@Image6", data6);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                msje = cmd.Parameters["@msj"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msje;
        }

        //metodo que busca la ultima imagen insertada de un registro
        //apoyo para el metodo anterior
        public string find_images_Name(int type, int id)
        {
            SqlCommand cmd;
            string name;
            try
            {
                Abrir();
                cmd = new SqlCommand("consulta_media_name", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                int res = cmd.ExecuteNonQuery();
                name = cmd.Parameters["@name"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return name;
        }
        //carga nuevas imagenes de la WEB al registro
        public string MoreMedia(int id, int usu, int type, string name)
        {
            SqlCommand cmd;
            string msje;
            try
            {
                Abrir();
                cmd = new SqlCommand("Insert_Media_Web", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usu", usu);
                cmd.Parameters.AddWithValue("@tipo", type);
                cmd.Parameters.AddWithValue("@nombre", name);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.Add("@msj", SqlDbType.Int).Direction = ParameterDirection.Output;
                int res = cmd.ExecuteNonQuery();
                msje = cmd.Parameters["@msj"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msje;
        }

        //elimina imagenes de la WEB 
        public string DELIMG(int id, int usu, int type, string name)
        {
            SqlCommand cmd;
            string msje;
            try
            {
                Abrir();
                cmd = new SqlCommand("DelImg_Web", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@usu", usu);
                cmd.Parameters.AddWithValue("@tipo", type);
                cmd.Parameters.AddWithValue("@nombre", name);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                
                int res = cmd.ExecuteNonQuery();
                msje = cmd.Parameters["@msj"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msje;
        }

        //Inserta un registro nuevo a incidentes ignorando en CatId y la descripcion
        public string InsertarRegistro_V2(String NumSafety, String latitud, String longitud, int usuario, int mediaExist, String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, int imgCount, int vdCount)
        {
            SqlCommand cmd;
            int res = 1;
            string msj = "";
            try
            {
                Abrir();

                cmd = new SqlCommand("Carga_datos_V2", con);//LLAMAMOS AL ESTORED PROCEDURE Y CREAMOS LA CONEXION EN LA BD
                cmd.CommandType = CommandType.StoredProcedure;
                //ASIGNAMOS LOS VALORES QUE LE MANDAREMOS AL METODO EN LA BASE DE DATOS
                cmd.Parameters.AddWithValue("@NumSafety", NumSafety);
                cmd.Parameters.AddWithValue("@latitud", latitud);
                cmd.Parameters.AddWithValue("@longitud", longitud);
                cmd.Parameters.AddWithValue("@USU_id", usuario);
                cmd.Parameters.AddWithValue("@media", mediaExist);
                //INSERTAMOS TODAS LAS IMAGENES AUNQUE ESTAS SEAN =NULL
                cmd.Parameters.AddWithValue("@Data1", Data1);
                cmd.Parameters.AddWithValue("@Data2", Data2);
                cmd.Parameters.AddWithValue("@Data3", Data3);
                cmd.Parameters.AddWithValue("@Data4", Data4);
                cmd.Parameters.AddWithValue("@Data5", Data5);
                cmd.Parameters.AddWithValue("@Data6", Data6);
                cmd.Parameters.AddWithValue("@imgCount", imgCount);
                cmd.Parameters.AddWithValue("@vdCount", vdCount);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                
                res = cmd.ExecuteNonQuery();//EJECUTAMOS EL METODO Y CACHAMOS EL RESULTADO
                msj = cmd.Parameters["@msj"].Value.ToString();

                //obtieneAutoSerial();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msj;
        }

        //Carga una alerta para version 2
        public string CargaAlerta_V2(int usu, String descripcion, String latitud, String longitud, int media, String Image1, String Image2, String Image3, String Image4, String Image5, String Image6)
        {
            SqlCommand cmd;
            string msj = "";
            int res;
            try
            {
                Abrir();

                cmd = new SqlCommand("CargaAlerta_V2", con);//LLAMAMOS AL ESTORED PROCEDURE Y CREAMOS LA CONEXION EN LA BD
                cmd.CommandType = CommandType.StoredProcedure;
                //ASIGNAMOS LOS VALORES QUE LE MANDAREMOS AL METODO EN LA BASE DE DATOS
                cmd.Parameters.AddWithValue("@usu", usu);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@latitud", latitud);
                cmd.Parameters.AddWithValue("@longitud", longitud);
                cmd.Parameters.AddWithValue("@media", media);
                //INSERTAMOS TODAS LAS IMAGENES AUNQUE ESTAS SEAN =NULL
                cmd.Parameters.AddWithValue("@Image1", Image1);
                cmd.Parameters.AddWithValue("@Image2", Image2);
                cmd.Parameters.AddWithValue("@Image3", Image3);
                cmd.Parameters.AddWithValue("@Image4", Image4);
                cmd.Parameters.AddWithValue("@Image5", Image5);
                cmd.Parameters.AddWithValue("@Image6", Image6);
                cmd.Parameters.Add("@msj", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                //EJECUTAMOS EL METODO Y CACHAMOS EL RESULTADO
                res = cmd.ExecuteNonQuery();
                msj = cmd.Parameters["@msj"].Value.ToString();
                Cerrar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msj;

        }
    }    
}
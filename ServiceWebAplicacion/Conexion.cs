using System;       
using System.Data.SqlClient;
using System.Data;                
using System.Web.Script.Serialization;  
using System.Collections;  
using Newtonsoft.Json;

                                                                                     
namespace ServiceWebAplicacion
{

    public class Conexion
    {
        SqlConnection con;

        public Conexion()
        {
            if (con == null)
                //  con = new SqlConnection("Server=SQL5031.SmarterASP.NET;DataBase=DB_9B853E_Servicios;User Id=DB_9B853E_Servicios_admin;password=Leirdadacrca2017");
                con = new SqlConnection("Server=SQL5017.SmarterASP.NET;DataBase=DB_9B853E_sspcolima;User Id=DB_9B853E_sspcolima_admin;password=Leirdadacrca2017");
            //Data Source=SQL5017.SmarterASP.NET;Initial Catalog=DB_9B853E_sspcolima;User Id=DB_9B853E_sspcolima_admin;Password=Leirdadacrca2017;" providerName="System.Data.SqlClient
            //con = new SqlConnection("Server=LEONCIO1;DataBase=EJEMPLO;User Id=sa;password=1");
            //con = new SqlConnection("Data Source=.;DataBase=ejemplo;Integrated Security=true");
            //private static string cadenaConexion = @"Data Source=SQL5007.Smarterasp.net;Initial Catalog=DB_9B853E_REPUVE;User Id=DB_9B853E_REPUVE_admin;Password=Leirdadacrca2014;";
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

        public int InsertarRegistro(String catId, String NumSafety, String descripcion, String latitud, String longitud, int usuario, String Data1, String Data2, String Data3, String Data4, String Data5, String Data6, int imgCount, int vdCount)
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
            return res;
        }
        public String CatalogoJSONid()//metodo para obtener el catalogo de tipos de incidentes y sus ID
        {
            var jsonID = "";
            SqlCommand cmd;
            try {
                con.Open();
                string sql = "SELECT CAT_Id, CAT_Descripcion FROM CAT_Incidentes_Safety";
                cmd = new SqlCommand(sql, con);
                //comodin para poder llegar al dataset
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                myAdapter.SelectCommand = cmd;

                DataSet myDataSet = new DataSet();
                myAdapter.Fill(myDataSet, "CAT_Descripcion");
                //estos contendran las columnas ID y DES. del DataSet 
                ArrayList myArrayID = new ArrayList();

                foreach (DataRow dtRow in myDataSet.Tables["CAT_Descripcion"].Rows)
                {
                    myArrayID.Add(dtRow[0]);//obtenemos los ID del XML
                }
                var jsonSerialiser = new JavaScriptSerializer();
                //serializamos los ArrayList obtenidos el DataSet
                jsonID = jsonSerialiser.Serialize(myArrayID);

                con.Close();
            }
            catch (Exception ex) {
                throw ex;
            }

            return jsonID;
        }
        public String CatalogoJSONdes()//metodo para obtener el catalogo de tipos de incidentes y sus descripciones
        {
            var jsonDES = "";
            SqlCommand cmd;
            try
            {
                con.Open();
                string sql = "SELECT CAT_Id, CAT_Descripcion FROM CAT_Incidentes_Safety";
                cmd = new SqlCommand(sql, con);
                //comodin para poder llegar al dataset
                SqlDataAdapter myAdapter = new SqlDataAdapter();
                myAdapter.SelectCommand = cmd;

                DataSet myDataSet = new DataSet();
                myAdapter.Fill(myDataSet, "CAT_Descripcion");
                //estos contendran las columnas ID y DES. del DataSet 
                ArrayList myArrayDES = new ArrayList();

                foreach (DataRow dtRow in myDataSet.Tables["CAT_Descripcion"].Rows)
                {
                    myArrayDES.Add(dtRow[1]);//obtenemos las DESCRIPCIONES del XML
                }
                var jsonSerialiser2 = new JavaScriptSerializer();
                //serializamos los ArrayList obtenidos el DataSet
                jsonDES = jsonSerialiser2.Serialize(myArrayDES);

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonDES;
        }

        public string CargaAlerta(int usu, String descripcion, String latitud, String longitud, String Image1, String Image2, String Image3)
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
        public String Picture_Alerts(String id, int flag)
        {
            DataSet myDataSet = new DataSet();
            string data;
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("Return_Picture", con);                           
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(myDataSet);

                data = JsonConvert.SerializeObject(myDataSet); //se usa la libreria Newtonsoft.Json 
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
    }
}
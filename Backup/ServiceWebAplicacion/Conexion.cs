using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ServiceWebAplicacion
{
   public class Conexion
    {
       SqlConnection con;

       public Conexion()
        {
            if (con == null)
             con = new SqlConnection("Server=LEONCIO1;DataBase=EJEMPLO;User Id=sa;password=1");
             //con = new SqlConnection("Data Source=.;DataBase=ejemplo;Integrated Security=true");
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
                cmd.Parameters.Add("@msje", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
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
    }
}
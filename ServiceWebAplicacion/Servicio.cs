using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO;

namespace ServicioWebAplicacion
{
    public class Servicio
    {
        public int Id { get; set; }
        public string ServicioDes { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Foto { get; set; }

        public Servicio()
        {
            this.Id = 0;
            this.ServicioDes = "";
            this.Latitud = "";
            this.Longitud = "";
            this.Foto = "";
        }

        public Servicio(int id, string servicio, string latitud, string longitud, string foto)
        {
            this.Id = id;
            this.ServicioDes = servicio;
            this.Latitud = latitud;
            this.Longitud = longitud;
            this.Foto = foto;
        }
    }
}
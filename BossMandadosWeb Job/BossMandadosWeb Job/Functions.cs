using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using System.Data.SqlClient;

namespace BossMandadosWeb_Job
{
    public class Functions
    {
        public static void AsignarMandados()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["mandadosdb"].ToString();
            //Obtener mandados sin mandadero, estado 1
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT id FROM manboos_mandados WHERE estado=1", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int m_id = reader.GetInt32(reader.GetOrdinal("id"));
                        }
                    }
                }
            }
            //Obtener mandaderos disponibles, estado 0
            List<Mandadero> mandaderos = new List<Mandadero>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT repartidor,latitud,longitud,rating,efectivo FROM manboos_repartidores WHERE estado=0", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int r_id = reader.GetInt32(reader.GetOrdinal("repartidor"));
                            float r_latitud = reader.GetFloat(reader.GetOrdinal("latitud"));
                            float r_longitud = reader.GetFloat(reader.GetOrdinal("longitud"));
                            float r_rating = reader.GetFloat(reader.GetOrdinal("rating"));
                            float r_efectivo = reader.GetFloat(reader.GetOrdinal("efectivo"));
                            //Mandadero aux = new Mandadero(r_efectivo);
                            //mandaderos.Add(aux);
                        }
                    }
                }
            }
            
            //Cambiar mandados a estado 2
            //Mensaje
            Console.WriteLine("Proceso Terminado Exitosamente");
        }

        public int Resultado(List<Mandadero> mandaderos, double tRango, double Tiempo)
        {
            int ind = -1;
            double best = 100000000;
            double homo = 100000000;
            for (int i = 0; i < mandaderos.Count; i++)
            {
                if (best > mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandando)
                    best = mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandando;
            }
            for (int i = 0; i < mandaderos.Count; i++)
            {
                if (mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandando <= best + tRango)
                {
                    if (((mandaderos[i].GananciaDia + mandaderos[i].Ganancia) /
                        (mandaderos[i].HorasTrabajo + mandaderos[i].TiempoAMandando + Tiempo) - mandaderos[i].Sueldo()) < homo)
                    {
                        homo = ((mandaderos[i].GananciaDia + mandaderos[i].Ganancia) /
                            (mandaderos[i].HorasTrabajo + mandaderos[i].TiempoAMandando + Tiempo) - mandaderos[i].Sueldo());
                        ind = i;
                    }
                }
            }
            return ind;
        }
    }
}

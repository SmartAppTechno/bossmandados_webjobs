using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Xml;

namespace BossMandadosWeb_Job
{
    public class Functions
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["mandadosdb"].ToString();
        public static SqlConnection connection = new SqlConnection(connectionString);

        public static void AsignarMandados()
        {    
            //Obtener mandaderos
            List<Mandadero> mandaderos = new List<Mandadero>();
            using (SqlCommand cmd = new SqlCommand("SELECT repartidor,latitud,longitud,rating,efectivo,hora_inicio FROM manboos_repartidores", connection))
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
                            DateTime r_hora = reader.GetDateTime(reader.GetOrdinal("hora_inicio"));
                            Mandadero aux = new Mandadero();
                            aux.Identificador = r_id;
                            aux.Latitud = r_latitud;
                            aux.Longitud = r_longitud;
                            aux.GananciaDia = r_efectivo;
                            DateTime ahora = DateTime.Now;
                            aux.HorasTrabajo = (ahora - r_hora).TotalHours;
                            aux.Rating = r_rating;
                            aux.TiempoDisponible = 0;
                            mandaderos.Add(aux);
                        }
                    }
                }
            }
            //Obtener mandados en proceso, estado 3
            using (SqlCommand cmd = new SqlCommand("SELECT id,repartidor FROM manboos_mandados WHERE estado=3", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int m_id = reader.GetInt32(reader.GetOrdinal("id"));
                            int m_repartidor = reader.GetInt32(reader.GetOrdinal("repartidor"));
                            //Obtener ubicacion del último punto
                            float latitud = GetUltimaLatitud(m_id);
                            float longitud = GetUltimaLongitud(m_id);
                            foreach(Mandadero temp in mandaderos)
                            {
                                if(temp.Identificador == m_repartidor)
                                {
                                    double tiempo = GetTiempo(temp.Latitud, temp.Longitud, latitud, longitud);
                                    temp.TiempoDisponible = tiempo;
                                }
                            }
                        }
                    }
                }
            }
            //Obtener mandados sin mandadero, estado 1
            using (SqlCommand cmd = new SqlCommand("SELECT id,total FROM manboos_mandados WHERE estado=1", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int m_id = reader.GetInt32(reader.GetOrdinal("id"));
                            float m_ganancia = Convert.ToSingle(reader.GetFloat(reader.GetOrdinal("total")) * 0.35);
                            foreach (Mandadero temp in mandaderos)
                            {
                                temp.GananciaMandado = m_ganancia;
                                float latitud = GetPrimeraLatitud(m_id);
                                float longitud = GetPrimeraLongitud(m_id);
                                temp.TiempoAMandado = GetTiempo(temp.Latitud, temp.Longitud, latitud, longitud);
                            }
                            //Buscar Mandadero
                            double rango = 0.08333333333;
                            double tiempo = 0;
                            int mandadero = BuscarMandadero(mandaderos,rango,tiempo);
                            //Cambiar mandados a estado 2
                            AsignarMandadero(mandadero, m_id);
                        }
                    }
                }
            }
            //Mensaje
            Console.WriteLine("Proceso Terminado Exitosamente");
        }

        public static void AsignarMandadero(int mandadero,int mandado)
        {
            SqlCommand cmd = new SqlCommand("UPDATE manboss_mandados SET repartidor = @mandadero where id= @mandado_id", connection);
            cmd.Parameters.AddWithValue("@mandadero", mandadero);
            cmd.Parameters.AddWithValue("@mandado_id", mandado);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public static float GetUltimaLatitud(int id_mandado)
        {
            try
            {
                string sql = @"select latitud from manboss_mandados_rutas where mandado=@id_mandado order by id DESC limit 1";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id_mandado", id_mandado);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (float)reader["latitud"];
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        public static float GetUltimaLongitud(int id_mandado)
        {
            try
            {
                string sql = @"select longitud from manboss_mandados_rutas where mandado=@id_mandado order by id DESC limit 1";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id_mandado", id_mandado);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (float)reader["longitud"];
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        public static float GetPrimeraLatitud(int id_mandado)
        {
            try
            {
                string sql = @"select latitud from manboss_mandados_rutas where mandado=@id_mandado order by id ASC limit 1";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id_mandado", id_mandado);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (float)reader["latitud"];
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        public static float GetPrimeraLongitud(int id_mandado)
        {
            try
            {
                string sql = @"select longitud from manboss_mandados_rutas where mandado=@id_mandado order by id ASC limit 1";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id_mandado", id_mandado);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (float)reader["longitud"];
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        public static double GetTiempo(float olat,float olong, float dlat, float dlong)
        {
            var R = 6371d;
            var dLat = Deg2Rad(dlat - olat); 
            var dLon = Deg2Rad(dlong - olong);
            var a = Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) + Math.Cos(Deg2Rad(olat)) * Math.Cos(Deg2Rad(dlat)) * Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d);
            var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
            var d = R * c;
            return d / 60; //60km por hora
        }

        public static double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180d);
        }

        public static int BuscarMandadero(List<Mandadero> mandaderos, double tRango, double Tiempo)
        {
            int ind = -1;
            double best = 100000000;
            double homo = 100000000;
            for (int i = 0; i < mandaderos.Count; i++)
            {
                if (best > mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandado)
                    best = mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandado;
            }
            for (int i = 0; i < mandaderos.Count; i++)
            {
                if (mandaderos[i].TiempoDisponible + mandaderos[i].TiempoAMandado <= best + tRango)
                {
                    if (((mandaderos[i].GananciaDia + mandaderos[i].GananciaMandado) /
                        (mandaderos[i].HorasTrabajo + mandaderos[i].TiempoAMandado + Tiempo) - mandaderos[i].Sueldo()) < homo)
                    {
                        homo = ((mandaderos[i].GananciaDia + mandaderos[i].GananciaMandado) /
                            (mandaderos[i].HorasTrabajo + mandaderos[i].TiempoAMandado + Tiempo) - mandaderos[i].Sueldo());
                        ind = i;
                    }
                }
            }
            return ind;
        }
    }
}

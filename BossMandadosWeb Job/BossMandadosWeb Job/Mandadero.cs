namespace BossMandadosWeb_Job
{
    public class Mandadero
    {
        float _Latitud;
        float _Longitud;
        int _Identificador;
        double _GananciaDia;
        double _HorasTrabajo;
        double _Rating;
        double _TiempoDisponible;
        double _GananciaMandado;
        double _TiempoAMandado;

        public Mandadero()
        {
            
        }

        public int Sueldo()
        {
            return 0;
        }

        public int Identificador { get => _Identificador; set => _Identificador = value; }
        public float Latitud { get => _Latitud; set => _Latitud = value; }
        public float Longitud { get => _Longitud; set => _Longitud = value; }
        public double GananciaDia { get => _GananciaDia; set => _GananciaDia = value; }
        public double HorasTrabajo { get => _HorasTrabajo; set => _HorasTrabajo = value; }
        public double Rating { get => _Rating; set => _Rating = value; }
        public double TiempoDisponible { get => _TiempoDisponible; set => _TiempoDisponible = value; }
        public double GananciaMandado { get => _GananciaMandado; set => _GananciaMandado = value; }
        public double TiempoAMandado { get => _TiempoAMandado; set => _TiempoAMandado = value; }
    }
}

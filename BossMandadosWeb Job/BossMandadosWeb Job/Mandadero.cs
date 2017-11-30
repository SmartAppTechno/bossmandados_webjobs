using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossMandadosWeb_Job
{
    public class Mandadero
    {
        double _GananciaDia;
        double _HorasTrabajo;
        double _TiempoDisponible;
        double _TiempoAMandando;
        double _Raiting;
        double _Ganancia;

        public double Sueldo()
        {
            return 0;
        }
        
        public Mandadero(double GananciaDia, double TiempoDisponible, double TiempoAMandando, double Raiting, double Ganancia)
        {
            _GananciaDia = GananciaDia;
            _HorasTrabajo = HorasTrabajo;
            _TiempoDisponible = TiempoDisponible;
            _TiempoAMandando = TiempoAMandando;
            _Raiting = Raiting;
            _Ganancia = Ganancia;
        }

        public double GananciaDia { get => _GananciaDia; set => _GananciaDia = value; }
        public double HorasTrabajo { get => _HorasTrabajo; set => _HorasTrabajo = value; }
        public double TiempoDisponible { get => _TiempoDisponible; set => _TiempoDisponible = value; }
        public double TiempoAMandando { get => _TiempoAMandando; set => _TiempoAMandando = value; }
        public double Raiting { get => _Raiting; set => _Raiting = value; }
        public double Ganancia { get => _Ganancia; set => _Ganancia = value; }
    }
}

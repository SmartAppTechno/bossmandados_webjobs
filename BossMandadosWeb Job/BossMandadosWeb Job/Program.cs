using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace BossMandadosWeb_Job
{
    class Program
    {
        static void Main()
        {
            Functions.AsignarMandados();
        }
    }
}

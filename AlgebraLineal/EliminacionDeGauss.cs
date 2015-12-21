using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraLineal
{
    public interface EliminacionDeGauss
    {
        EcuacionLineal AplicacionDeSuma(EcuacionLineal r1, EcuacionLineal r2, double razon);
        EcuacionLineal CambiaAUno(EcuacionLineal r1, double razon);
        
    }
}

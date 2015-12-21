using AlgebraLineal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 2;//2 infinito y -3 incongruente
            SistemaDeEcuaciones se = new SistemaDeEcuaciones(){
                new EcuacionLineal(){
                    new TerminoLineal("x",1),
                    new TerminoLineal("y",1),
                    new TerminoLineal("z",-1),
                    new TerminoLineal("_",1)
                },
                new EcuacionLineal(){
                    new TerminoLineal("x",2),
                    new TerminoLineal("y",3),
                    new TerminoLineal("z",a),
                    new TerminoLineal("_",3)
                },
                new EcuacionLineal(){
                    new TerminoLineal("x",1),
                    new TerminoLineal("y",a),
                    new TerminoLineal("z",3),
                    new TerminoLineal("_",2)
                }
            };
            Console.WriteLine("La ecuación inicial es:");
            Console.WriteLine(se.ToString());

            //Paso 1 mismos términos
            var s1 = EliminacionDeGaussLinq.IgualaTerminos(se);
            //Paso 2 orden
            var _s2 = new SistemaDeEcuaciones(s1.Select(x => x.ordena()).ToList()).ordena();
            List<string> l = (from s in _s2[0] select s.Literal).ToList();
            var __s2 = new SistemaDeEcuaciones(s1.Select(x => x.ordena(l)).ToList()).ordena();
            //Paso 3 Reduccion
            var s2 = Reduccion.re(__s2, l);

            //Paso 4 Resultados
            switch (EliminacionDeGaussLinq.EvaluaNumeroDeResultados(s2))
            {
                case 1:
                    Console.WriteLine("Existen una infinidad de Resultados");
                    Console.WriteLine(s2.ToString());
                    Console.WriteLine("Una solución específica es:");
                    SistemaDeEcuaciones sde = new SistemaDeEcuaciones();
                    foreach (EcuacionLineal el in s2)
                    {
                        EcuacionLineal e = new EcuacionLineal();
                        foreach (TerminoLineal tl in el)
                            e.Add(new TerminoLineal(tl.Literal, tl.Coeficiente));
                        sde.Add(e);
                    }
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoInfinito(sde,5.3))
                        Console.WriteLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    SistemaDeEcuaciones sde1 = new SistemaDeEcuaciones();
                    foreach (EcuacionLineal el in s2)
                    {
                        EcuacionLineal e = new EcuacionLineal();
                        foreach (TerminoLineal tl in el)
                            e.Add(new TerminoLineal(tl.Literal, tl.Coeficiente));
                        sde1.Add(e);
                    }
                    Console.WriteLine("Otra solución específica es:");
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoInfinito(sde1, -1))
                        Console.WriteLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    break;
                case 0:
                    Console.WriteLine("Existe sólo un Resultado");
                    Console.WriteLine(s2.ToString());
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoUnico(s2))
                        Console.WriteLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    break;
                case -1:
                    Console.WriteLine("La ecuación es incongruente");
                    Console.WriteLine(s2.ToString());
                    break;
                default:
                    Console.WriteLine("La ecuación no se evaluó correctamente");
                    Console.WriteLine(s2.ToString());
                    break;
            }
            Console.ReadKey();

        }
    }
}

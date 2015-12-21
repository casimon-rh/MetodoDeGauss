using AlgebraLineal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraLinealConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SistemaDeEcuaciones se = new SistemaDeEcuaciones();
            int i = 0, j = 0;
            char confirmacion = 's';
            do
            {
                Console.WriteLine("A continuacion se le solicita los detalles de su ecuacion:" + (i + 1));
                EcuacionLineal el = new EcuacionLineal();
                j = 0;
                do
                {
                    TerminoLineal tl = new TerminoLineal();
                    Console.WriteLine("Especifique el coeficiente del termino:" + (j + 1));
                    tl.Coeficiente = int.Parse(Console.ReadLine());
                    Console.WriteLine("Especifique la literal del termino:" + (j + 1) + "\n(Para indicar el termino constante, especifique '_')");
                    tl.Literal = Console.ReadLine();

                    Console.WriteLine("¿Desea especificar otro término para la ecuacion" + (i + 1)+"(s/n)?");
                    confirmacion = Console.ReadKey().KeyChar;
                    el.Add(tl);
                    j++;
                } while (confirmacion != 'n');
                if (el.Where(x => x.Literal == "_").Count() == 0)
                    el.Add(new TerminoLineal("_", 0));
                se.Add(el);
                i++;
                Console.WriteLine("¿Desea especificar otra ecuacion (s/n)?");
                confirmacion = Console.ReadKey().KeyChar;
            }
            while (confirmacion != 'n');
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
                    //foreach (string s in Reduccion.PresentaInfinidadDeResultados(s2))
                    //    Console.WriteLine(s);
                    break;
                case 0:
                    Console.WriteLine("Existe sólo un Resultado");
                    Console.WriteLine(s2.ToString());
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoUnico(s2))
                        Console.WriteLine(t.Literal + "=" + t.Valor);
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

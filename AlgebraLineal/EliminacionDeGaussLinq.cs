using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraLineal
{
    public static class EliminacionDeGaussLinq
    {
        public static EcuacionLineal AplicacionDeSuma(EcuacionLineal r1, EcuacionLineal r2, double razon)
        {
            return new EcuacionLineal((from _r1 in r1
                                       join _r2 in r2 on (_r1.Literal) equals (_r2.Literal)
                                       select new TerminoLineal(
                                           _r1.Literal,
                                           (_r1.Coeficiente * razon) + _r2.Coeficiente)
                    ).ToList());
        }

        public static EcuacionLineal CambiaAUno(EcuacionLineal r1, double razon)
        {
            return new EcuacionLineal((from _r1 in r1
                                       select new TerminoLineal(
                                           _r1.Literal,
                                           (_r1.Coeficiente * (1 / razon)))
                    ).ToList());
        }
        public static EcuacionLineal ordena(this EcuacionLineal el)
        {
            var ell = new EcuacionLineal((from e in el
                                          where (e.Coeficiente != 0 && e.Literal != "_")
                                            && (e.Literal != "_")
                                          orderby Math.Abs(e.Coeficiente) ascending
                                          select e).ToList());
            foreach (var elem in el.Where(x => x.Coeficiente == 0 && x.Literal != "_").Select(x => x).ToList())
                ell.Add(elem);
            foreach (var elem in el.Where(x => x.Literal == "_").Select(x => x).ToList())
                ell.Add(elem);
            return ell;
        }
        public static SistemaDeEcuaciones ordena(this SistemaDeEcuaciones sis)
        {
            return new SistemaDeEcuaciones((from s in sis orderby Math.Abs(s[0].Coeficiente) ascending select s).ToList());
        }
        public static EcuacionLineal ordena(this EcuacionLineal sis, List<string> orden)
        {
            EcuacionLineal s = new EcuacionLineal();
            foreach (var o in orden)
                s.Add(new TerminoLineal(o, sis.obtieneCoeficiente(o)));
            return s;
        }
        public static SistemaDeEcuaciones IgualaTerminos(this SistemaDeEcuaciones sis)
        {
            SistemaDeEcuaciones se = new SistemaDeEcuaciones();
            EcuacionLineal el = new EcuacionLineal();
            for (int i = 0; i < sis.Count; i++)
            {
                el = Libera(el);
                for (int j = 0; j < sis[i].Count; j++)
                {
                    if (el.Select(x => x.Literal).ToList().Contains(sis[i][j].Literal))
                        el.AcumulaCoeficientePorTermino(sis[i][j].Literal, sis[i][j].Coeficiente);
                    else
                        el.Add(new TerminoLineal(sis[i][j].Literal, sis[i][j].Coeficiente));
                }
                for (int j = sis[i].Count - 1; j >= 0; j--)
                {
                    if (el.Select(x => x.Literal).ToList().Contains(sis[i][j].Literal))
                        el.CambiaCoeficientePorTermino(sis[i][j].Literal, sis[i][j].Coeficiente);
                    else
                        el.Add(new TerminoLineal(sis[i][j].Literal, sis[i][j].Coeficiente));
                }
                se.Add(el);
            }
            return se;
        }
        public static EcuacionLineal Libera(EcuacionLineal el)
        {
            return new EcuacionLineal((from t in el select new TerminoLineal(t.Literal, 0)).ToList());
        }
        public static int EvaluaNumeroDeResultados(SistemaDeEcuaciones sis)
        {
            if (sis.Where(e => e.Where(t => t.Coeficiente == 0).Count() == e.Count).Select(x => x).Count() > 0)
                return 1;//Cuando hay una infinidad de Resultados
            else if (sis.Where(e => e.Where(t => t.Coeficiente == 0).Count() == e.Where(t => t.Literal != "_").Count()).Select(x => x).Count() > 0)
                return -1;//Cuando es Incosistente
            else if (sis.Select(e => e).LastOrDefault().Select(t => t).LastOrDefault().Coeficiente != 0
                        && sis.Count() == sis[0].Count() - 1)
                return 0;//Cuando existe solo un resultado
            else return 1;//Algo falló
        }
    }
    public static class Reduccion
    {
        public static SistemaDeEcuaciones re(SistemaDeEcuaciones s2, List<string> l)
        {
            int e = 0;

            for (int i = 0; i < s2.Count; i++)
            {
                if (s2[i][e].Literal != "_")
                {
                    if (s2[i][e].Coeficiente == 0)
                        continue;
                    if (s2[i][e].Coeficiente < 0)
                        s2[i].InvierteSignos();
                    if (s2[i][e].Coeficiente == 1)
                    {
                        for (int j = 0; j < s2.Count; j++)
                            if (i < j && s2[j].obtieneCoeficiente(s2[i][e].Literal) != 0)
                                s2[j] = EliminacionDeGaussLinq.AplicacionDeSuma(s2[i], s2[j], -s2[j].obtieneCoeficiente(s2[i][e].Literal)).ordena(l);
                    }
                    else
                    {
                        s2[i] = EliminacionDeGaussLinq.CambiaAUno(s2[i], s2[i][e].Coeficiente);
                        for (int j = 0; j < s2.Count; j++)
                            if (i < j && s2[j].obtieneCoeficiente(s2[i][e].Literal) != 0)
                                s2[j] = EliminacionDeGaussLinq.AplicacionDeSuma(s2[i], s2[j], -s2[j].obtieneCoeficiente(s2[i][e].Literal)).ordena(l);
                    }
                    e++;
                }
            }
            return s2;
        }
        public static List<TerminoLineal> PresentaResultadoUnico(SistemaDeEcuaciones sis)
        {
            var lista = new List<TerminoLineal>();
            for (int i = sis.Count - 1; i >= 0; i--)
            {
                TerminoLineal a = new TerminoLineal();
                double suma = 0;
                int j = 0;
                foreach (TerminoLineal t in sis[i])
                {
                    foreach (TerminoLineal tt in lista)
                    {
                        if (t.Literal == tt.Literal)
                            t.Valor = tt.Valor;
                    }
                    if (t.Literal == "_")
                    {
                        suma += t.Coeficiente;
                    }
                    else if (t.Coeficiente != 1)
                    {
                        suma += (-t.Coeficiente) * t.Valor;
                    }
                    else if (t.Coeficiente == 1 && t.Literal != "_" && t.Valor == 0 && i == j)
                    {
                        a.Literal = t.Literal;
                    }
                    j++;
                }
                a.Valor = suma;
                lista.Add(a);
            }
            return lista;
        }
        public static List<TerminoLineal> PresentaResultadoInfinito(SistemaDeEcuaciones sis1, double parametro)
        {
            SistemaDeEcuaciones sis = sis1;
            var lista = new List<TerminoLineal>();
            bool paso = false;
            for (int i = sis.Count - 1; i >= 0; i--)
            {
                if (sis[i].Where(x => x.Coeficiente == 0).Count() == sis[i].Count)
                    continue;
                TerminoLineal a = new TerminoLineal();
                double suma = 0;

                for (int j = sis[i].Count - 1; j >= 0; j--)
                {
                    foreach (TerminoLineal tt in lista)
                    {
                        if (sis[i][j].Literal == tt.Literal)
                            sis[i][j].Valor = tt.Valor;
                    }
                    if (sis[i][j].Coeficiente != 0
                        && i != j
                        && sis[i][j].Literal != "_"
                        && sis[i][j].Valor == 0)
                    {
                        sis[i][j].Valor = parametro;
                        if (!paso)
                        {
                            lista.Add(sis[i][j]);
                            paso = true;
                        }
                    }

                }
                int jj = 0;
                foreach (TerminoLineal t in sis[i])
                {
                    foreach (TerminoLineal tt in lista)
                    {
                        if (t.Literal == tt.Literal)
                            t.Valor = tt.Valor;
                    }
                    if (t.Literal == "_")
                    {
                        suma += t.Coeficiente;
                    }
                    else if (t.Coeficiente != 1 || i != jj)
                    {
                        suma += (-t.Coeficiente) * t.Valor;
                    }
                    else if (t.Coeficiente == 1 && t.Literal != "_" && t.Valor == 0)
                    {
                        a.Literal = t.Literal;
                    }
                    jj++;
                }
                if (a.Literal != null)
                {
                    a.Valor = suma;
                    lista.Add(a);
                }
            }
            return lista;
        }
    }
}

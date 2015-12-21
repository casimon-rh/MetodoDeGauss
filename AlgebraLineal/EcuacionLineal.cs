using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraLineal
{
    public class SistemaDeEcuaciones : List<EcuacionLineal>
    {
        public override string ToString()
        {
            string s = "";
            foreach (EcuacionLineal e in this)
                s += e.ToString() + Environment.NewLine;
            return s;
        }
        public SistemaDeEcuaciones(List<EcuacionLineal> lista)
        {
            foreach (var elem in lista)
                this.Add(elem);
        }
        public SistemaDeEcuaciones() { }
    }

    public class EcuacionLineal : List<TerminoLineal>
    {
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < this.Count; i++)
            {
                if (i == 0)
                    s += this[i].Coeficiente.ToString("0.00") + this[i].Literal + " ";
                else if (this[i].Literal == "_")
                    s += "=" + this[i].Coeficiente.ToString("0.00");
                else
                    if (this[i].Coeficiente >= 0)
                        s += "+" + this[i].Coeficiente.ToString("0.00") + this[i].Literal + " ";
                    else
                        s += this[i].Coeficiente.ToString("0.00") + this[i].Literal + " ";
            }
            return s;
        }
        public void CambiaCoeficientePorTermino(string s, double coe)
        {
            for (int i = 0; i < this.Count; i++)
                if (this[i].Literal == s)
                    this[i].Coeficiente = coe;
        }
        public void AcumulaCoeficientePorTermino(string s, double coe)
        {
            for (int i = 0; i < this.Count; i++)
                if (this[i].Literal == s)
                    this[i].Coeficiente += coe;
        }
        public void InvierteSignos()
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Coeficiente = -this[i].Coeficiente;
        }
        public double obtieneCoeficiente(string literal)
        {
            return (from e in this where e.Literal == literal select e.Coeficiente).FirstOrDefault();
        }
        public EcuacionLineal() { }
        public EcuacionLineal(List<TerminoLineal> lista)
        {
            foreach (var e in lista)
                this.Add(e);
        }
    }

    public class TerminoLineal
    {
        private double valor;

        public double Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        private double _coeficiente;

        public double Coeficiente
        {
            get { return _coeficiente; }
            set { _coeficiente = value; }
        }
        private string _literal;

        public string Literal
        {
            get { return _literal; }
            set { _literal = value; }
        }
        public TerminoLineal(string literal, double coeficiente)
        {
            this._literal = literal;
            this._coeficiente = coeficiente;
            this.valor = 0;
        }
        public TerminoLineal(string literal, double coeficiente, double _valor)
        {
            this._literal = literal;
            this._coeficiente = coeficiente;
            this.valor = _valor;
        }
        public TerminoLineal() { }
    }
}

using AlgebraLineal;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlgebraLinealWPF.Window
{
    /// <summary>
    /// Lógica de interacción para MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            this.Loaded += MainView_Loaded;
            
        }
        public DataTable dt { get; set; }
        void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            Config con = new Config();
            Sistema.DataContext = this;
            con.ShowDialog();
            dt = new DataTable();
            for (int i = 0; i < (con._num??0); i++)
                dt.Columns.Add("x" + (i + 1));
            dt.Columns.Add("_");
            Sistema.ItemsSource = dt.DefaultView;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = (Sistema.ItemsSource as DataView).Table;
            SistemaDeEcuaciones se = new SistemaDeEcuaciones();
            foreach (DataRow dr in dt.Rows)
            {
                EcuacionLineal el = new EcuacionLineal();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    el.Add(new TerminoLineal(dt.Columns[i].Caption, Convert.ToDouble(dr[i].ToString())));
                }
                se.Add(el);
            }
            StringBuilder sb = new StringBuilder();
            
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
                    sb.AppendLine("Existen una infinidad de Resultados");
                    sb.AppendLine(s2.ToString());
                    sb.AppendLine("Una solución específica es:");
                    SistemaDeEcuaciones sde = new SistemaDeEcuaciones();
                    foreach (EcuacionLineal el in s2)
                    {
                        EcuacionLineal ee = new EcuacionLineal();
                        foreach (TerminoLineal tl in el)
                            ee.Add(new TerminoLineal(tl.Literal, tl.Coeficiente));
                        sde.Add(ee);
                    }
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoInfinito(sde, 5.3))
                        sb.AppendLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    SistemaDeEcuaciones sde1 = new SistemaDeEcuaciones();
                    foreach (EcuacionLineal el in s2)
                    {
                        EcuacionLineal ee = new EcuacionLineal();
                        foreach (TerminoLineal tl in el)
                            ee.Add(new TerminoLineal(tl.Literal, tl.Coeficiente));
                        sde1.Add(ee);
                    }
                    sb.AppendLine("Otra solución específica es:");
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoInfinito(sde1, -1))
                        sb.AppendLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    break;
                case 0:
                    sb.AppendLine("Existe sólo un Resultado");
                    sb.AppendLine(s2.ToString());
                    foreach (TerminoLineal t in Reduccion.PresentaResultadoUnico(s2))
                        sb.AppendLine(t.Literal + "=" + t.Valor.ToString("0.00"));
                    break;
                case -1:
                    sb.AppendLine("La ecuación es incongruente");
                    sb.AppendLine(s2.ToString());
                    break;
                default:
                    sb.AppendLine("La ecuación no se evaluó correctamente");
                    sb.AppendLine(s2.ToString());
                    break;
            }
            Result.Text = sb.ToString();
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            Config con = new Config();
            con.ShowDialog();
            dt = new DataTable();
            for (int i = 0; i < (con._num ?? 0); i++)
                dt.Columns.Add("x" + (i + 1));
            dt.Columns.Add("_");
            Sistema.ItemsSource = dt.DefaultView;

        }
    }
}

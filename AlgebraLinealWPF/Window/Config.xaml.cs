using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : MetroWindow
    {
        public Config()
        {
            InitializeComponent();
            Ok.Click += Ok_Click;
            this.Closing += Config_Closing;
        }
        public int? _num { get; set; }

        void Config_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _num = (int?)Num.Value;
            if (Num.Value == null)
            {
                MessageBox.Show("Especifique el numero de variables","Reduccion de Gauss",MessageBoxButton.OK);
                e.Cancel = true;
            }
            else
                return;
        }

        void Ok_Click(object sender, RoutedEventArgs e)
        {
            _num = (int?)Num.Value;
            if (Num.Value == null)
                MessageBox.Show("Especifique el numero de variables", "Reduccion de Gauss", MessageBoxButton.OK);
            else
                this.Hide();
        }
    }
}

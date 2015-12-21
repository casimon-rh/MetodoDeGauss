using MahApps.Metro.Controls;
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
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : MetroWindow
    {
        public Menu()
        {
            InitializeComponent();
            this.Gauss.Click += Gauss_Click;
            this.Matriz.Click += Matriz_Click;
        }

        void Matriz_Click(object sender, RoutedEventArgs e)
        {

            Matrices mat = new Matrices();
            mat.ShowDialog(); ;
        }

        void Gauss_Click(object sender, RoutedEventArgs e)
        {
            MainView mv = new MainView();
            mv.ShowDialog();
        }

    }
}

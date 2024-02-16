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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MontlyReport.xaml
    /// </summary>
    public partial class MontlyReport : UserControl
    {
        public MontlyReport()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(string), typeof(MontlyReport), new PropertyMetadata(null));

        public string DataSource
        {
            get { return (string)GetValue(DataSourceProperty); }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }
    }
}

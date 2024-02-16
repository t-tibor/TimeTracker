using System.Text;
using System.Windows;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (Exception ex = e.Exception; ex != null; ex = ex.InnerException)
            {
                stringBuilder.Append(ex.Message);
            }

            MessageBox.Show("An unhandled exception just occurred: " + stringBuilder, "Exception report", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }

}

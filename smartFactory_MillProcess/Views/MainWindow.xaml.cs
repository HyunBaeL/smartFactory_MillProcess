using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using smartFactory_MillProcess.ViewModels;

namespace smartFactory_MillProcess.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public LoginViewModel LoginVM { get; private set; }
        public MainViewModel MainVM { get; private set; }
        public MachineViewModel MachineVM { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            LoginVM = new LoginViewModel();
            this.DataContext = LoginVM;

            MainVM = new MainViewModel();
            MachineVM = new MachineViewModel();

            // LoginUser에 ViewModel 넘겨줌
            MainFrame.Navigate(new LoginUser(LoginVM));


        }

        public void Navigate(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
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
using System.Linq;

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

        public EmployeeViewModel EmployeeVM { get; private set; }

        private Stack<Page> NavigationHistory = new Stack<Page>();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            LoginVM = new LoginViewModel();
            //this.DataContext = LoginVM;

            MainVM = new MainViewModel();
            DataContext = MainVM;
            MachineVM = new MachineViewModel();
            EmployeeVM = new EmployeeViewModel();

            // LoginUser에 ViewModel 넘겨줌
            MainFrame.Navigate(new LoginUser(LoginVM));


        }

        public void Navigate(Page page)
        {
            if (MainFrame.Content is Page currentPage)
            {
                NavigationHistory.Push(currentPage);
            }
            MainFrame.Navigate(page);
        }

        public void GoBack()
        {
            if (NavigationHistory.Count > 0)
            {
                var lastPage = NavigationHistory.Pop();

                // 로그인 페이지면 뒤로가기 금지
                if (lastPage is LoginUser)
                {
                    return;
                }
                else
                {
                    MainFrame.Navigate(lastPage);
                }
            }
        }

        public bool CanGoBack()
        {
            return NavigationHistory.Count > 0 && !(NavigationHistory.Peek() is LoginUser);
        }

        public void Logout()
        {
            NavigationHistory.Clear();

            // Frame 내부의 내비게이션 히스토리 제거
            var navService = MainFrame.NavigationService;

            MainVM.IsMenuOpen = false;
          
            MainFrame.Navigate(new LoginUser(LoginVM));
        }
    }
}
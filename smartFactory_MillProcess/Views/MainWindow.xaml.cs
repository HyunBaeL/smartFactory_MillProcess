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
        public FurnaceViewModel FurnaceVM { get; private set; }
        public RollingMachineViewModel RollingMachineVM { get; private set; }

        public FurnaceViewModel furnaceVM { get; private set; }
        public RollingMachineViewModel rollingMachineVM { get; private set; }

        

        private Stack<Page> NavigationHistory = new Stack<Page>();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // 윈도우 상태를 Normal로 바꾸고
            this.WindowState = WindowState.Normal;

            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            this.Width = screenWidth * 0.95;   // 화면 너비의 95%
            this.Height = screenHeight * 0.95; // 화면 높이의 95%
            // 화면 크기만큼 수동으로 크기 설정
            var screen = System.Windows.SystemParameters.WorkArea; // 작업 표시줄 제외한 영역

            LoginVM = new LoginViewModel();
            EmployeeVM = new EmployeeViewModel();
            //furnaceVM = new FurnaceViewModel();
            //rollingMachineVM = new RollingMachineViewModel();

            MainVM = new MainViewModel(LoginVM, EmployeeVM);
            DataContext = MainVM;
            MachineVM = new MachineViewModel();
            //EmployeeVM = new EmployeeViewModel();
            FurnaceVM = new FurnaceViewModel();
            RollingMachineVM = new RollingMachineViewModel();

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

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // 윈도우 드래그 이동
            }
        }
    }
}
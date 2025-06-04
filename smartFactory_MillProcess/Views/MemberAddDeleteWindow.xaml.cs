using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using ScottPlot;
using ScottPlot.WPF;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using smartFactory_MillProcess.ViewModels;
using System.Reflection.Emit;

namespace smartFactory_MillProcess.Views
{
    /// <summary>
    /// MemberAddDeletePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MemberAddDeleteWindow : Window
    {
        public MemberAddDeleteViewModel MVDV;
        public MemberAddDeleteWindow()
        {
            InitializeComponent();
             
            
            
        }

        public Window Owner { get; internal set; }
    }
}

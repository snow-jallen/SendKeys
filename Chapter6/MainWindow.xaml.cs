using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chapter6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Processes = Process.GetProcesses()
                .Select(p => p.ProcessName)
                .Where(p=>p != "svchost")
                .OrderBy(p=>p)
                .ToList();
            DataContext = this;
        }

        public IEnumerable<String> Processes { get; set; }
        public string SelectedProcess { get; set; }
        public string Text { get; set; }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        private ICommand sendText;
        public ICommand SendText => sendText ?? (sendText = new RelayCommand(
            () =>
            {
                Process p = Process.GetProcessesByName(SelectedProcess).FirstOrDefault();
                if (p != null)
                {
                    IntPtr h = p.MainWindowHandle;
                    SetForegroundWindow(h);
                    SendKeys.SendWait(Text);
                }
            }));        
    }
}

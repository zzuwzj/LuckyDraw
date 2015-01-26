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
using System.Windows.Threading;
using SMELuckyDraw.Logic;
using SMELuckyDraw.Model;

namespace SMELuckyDraw
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawLogic _logic = DrawLogic.Instance();
        private DispatcherTimer timer = new DispatcherTimer();
        private int finalValue = 0;
        private string finalValueDesc = "";

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            _logic.Init();
        }

        //timer tick
        void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                lbWinner.Content = finalValueDesc;
                timer.Stop();
                buttonStart.IsEnabled = true;
            }
        }

        //start button handler
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (!_logic.IsAbleToDraw())
            {
                showCannotDrawMsg();
                return;
            }

            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            lbWinner.Content = "Winner";
            timer.Stop();
        }

        //stop button handler
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            buttonStop.IsEnabled = false;
            //Random random = new Random();
            //finalValue = random.Next(100000);
            Candidate cdt = _logic.DoDraw();
            if (cdt != null)
            {
                finalValueDesc = cdt.Id + "   " + cdt.Name;
                finalValue = Convert.ToInt32(cdt.Id.Substring(1)); //remove first char
                numberGroupMain.TurnStop(finalValue);//stop
                timer.Start();
            }
            else
            {
                showCannotDrawMsg();
                numberGroupMain.TurnStop(finalValue);//stop
                timer.Stop();
            }
        }

        private void showCannotDrawMsg()
        {
            lbMsg.Content = "No candidate left!";
        }

        private void btnRest_Click(object sender, RoutedEventArgs e)
        {
            _logic.ResetApp();
        }

        private void btnSunny_Click(object sender, RoutedEventArgs e)
        {
            SunnyWindow sw = new SunnyWindow();
            sw.Show();
        }
    }
}

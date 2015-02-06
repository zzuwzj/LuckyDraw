using SMELuckyDraw.Logic;
using SMELuckyDraw.Model;
using SMELuckyDraw.Util;
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
using System.Windows.Threading;

namespace SMELuckyDraw
{
    /// <summary>
    /// SunnyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SunnyWindow : Window
    {
        private DrawLogic _logic = DrawLogic.Instance();
        private DispatcherTimer timer = new DispatcherTimer();
        private int _counter = 0;
        private bool isRunning = false;
        private readonly string DEFAULT_LABEL = "Wish you lucky!";
        private readonly string DEFAULT_LABEL_CONGRAT = "Contratulations!";

        public SunnyWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);

            lbCount.Content = _logic.GetExceptionCount();
        }

        //timer tick
        void timer_Tick(object sender, EventArgs e)
        {
            Candidate cdt = _logic.FreeDraw();
            if (cdt != null)
            {
                lbName.Content = cdt.Id + "   " + cdt.Name;
            }

            _counter++;
            if (_counter % 10 == 0)
            {
                int count = nameGroupMain.AddRandomName();
                if (count >= 30)
                {
                    timer.Stop();
                    isRunning = false;
                    lbName.Content = DEFAULT_LABEL_CONGRAT;
                }
                lbCount.Content = _logic.GetExceptionCount();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //nameGroupMain.TurnStart();
            timer.Start();
            isRunning = true;
            LogHelper.INFO("");
            LogHelper.INFO("=============================");
            LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
            LogHelper.INFO("Start lucky draw");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lbName.Content = DEFAULT_LABEL_CONGRAT;
            isRunning = false;
            LogHelper.INFO("End lucky draw");
            LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
            LogHelper.INFO("=============================\r\n");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (isRunning)
                {
                    timer.Stop();
                    lbName.Content = DEFAULT_LABEL_CONGRAT;
                    LogHelper.INFO("End lucky draw");
                    LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
                    LogHelper.INFO("=============================\r\n");
                }
                else
                {
                    timer.Start();
                    LogHelper.INFO("=============================");
                    LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
                    LogHelper.INFO("Start lucky draw");
                }
                isRunning = !isRunning;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            nameGroupMain.Clear();
            lbName.Content = DEFAULT_LABEL;
        }

        private void btnClear_GotFocus(object sender, RoutedEventArgs e)
        {
            btnBlank.Focus();
        }
    }
}

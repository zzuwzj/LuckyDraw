using SMELuckyDraw.Logic;
using SMELuckyDraw.Model;
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
        bool isRunning = false;

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
                }
                lbCount.Content = _logic.GetExceptionCount();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //nameGroupMain.TurnStart();
            timer.Start();
            isRunning = true;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
            isRunning = !isRunning;
        }
    }
}

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
        private readonly int DEFAULT_MAX_COUNT = 20;

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
                if (count >= DEFAULT_MAX_COUNT)
                {
                    timer.Stop();
                    isRunning = false;
                    lbName.Content = DEFAULT_LABEL_CONGRAT;
                    LogHelper.INFO("End lucky prize draw");
                    LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
                    LogHelper.SEPARATE();
                    LogHelper.SEPARATE();
                    LogHelper.NEWLINE();
                }
                lbCount.Content = _logic.GetExceptionCount();
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //nameGroupMain.TurnStart();
            if (nameGroupMain.GetCount() >= DEFAULT_MAX_COUNT)
            {
                nameGroupMain.Clear();
            }
            timer.Start();
            isRunning = true;
            // LogHelper.INFO("");
            // LogHelper.INFO("==========================================");
            LogHelper.NEWLINE();
            LogHelper.SEPARATE();
            LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
            LogHelper.INFO("Start lucky prize draw");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            lbName.Content = DEFAULT_LABEL_CONGRAT;
            isRunning = false;
            LogHelper.INFO("End lucky prize draw");
            LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
            // LogHelper.INFO("=========================================\r\n");
            LogHelper.SEPARATE();
            LogHelper.SEPARATE();
            LogHelper.NEWLINE();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (isRunning)
                {
                    timer.Stop();
                    lbName.Content = DEFAULT_LABEL_CONGRAT;
                    LogHelper.INFO("End lucky prize draw");
                    LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
                    // LogHelper.INFO("=========================================\r\n");
                    LogHelper.SEPARATE();
                    LogHelper.SEPARATE();
                    LogHelper.NEWLINE();
                }
                else
                {
                    // clear if full
                    if (nameGroupMain.GetCount() >= DEFAULT_MAX_COUNT)
                    {
                        nameGroupMain.Clear();
                    }
                    timer.Start();
                    // LogHelper.INFO("=====================================");
                    LogHelper.NEWLINE();
                    LogHelper.SEPARATE();
                    LogHelper.INFO("Total Winners: " + _logic.GetExceptionCount());
                    LogHelper.INFO("Start lucky prize draw");
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

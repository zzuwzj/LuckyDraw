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
using SMELuckyDraw.Util;
using System.Windows.Media.Animation;

namespace SMELuckyDraw
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawLogic _logic = DrawLogic.Instance();
        private DispatcherTimer winnerTimer = new DispatcherTimer();
        private DispatcherTimer stopTimer = new DispatcherTimer();
        private static int delayStopFrom = 4;
        private int stopCounter = delayStopFrom;
        private int finalValue = 0;
        private string finalValueDesc = "";
        private DrawStatus drawStatus = DrawStatus.STOPPED;
        private Dictionary<string, string> dictIdChar = new Dictionary<string, string>();
        private int lastNumberItem = 7;
        private bool isIUser = false;
        private bool isAllTurnStopped = false;

        // animation
        Storyboard story = new Storyboard();
        DoubleAnimation anim1 = new DoubleAnimation();
        DoubleAnimation anim2 = new DoubleAnimation();

        enum DrawStatus
        {
            STOPPED = 0,
            RUNNING,
            STOPPING
        }

        public MainWindow()
        {
            LogHelper.DEBUG("Init main window.");
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            winnerTimer.Interval = TimeSpan.FromSeconds(1);
            winnerTimer.Tick += new EventHandler(timer_Tick);

            stopTimer.Interval = TimeSpan.FromMilliseconds(1000);
            stopTimer.Tick += new EventHandler(stopTimer_Tick);

            _logic.Init();

            // Init Id char map for i, c & d user
            dictIdChar.Add("i", "0");
            dictIdChar.Add("c", "1");
            dictIdChar.Add("d", "2");
        }

        //stop timer tick, delay the stop
        void stopTimer_Tick(object sender, EventArgs e)
        {
            stopCounter++;

            //get the number
            int idx = stopCounter % 8;
            //exchange 1 & 6
            if (idx == 1)
            {
                TurnStopAt(6);
            }
            else if (idx == 6)
            {
                TurnStopAt(1);
            }
            else
            {
                TurnStopAt(idx);
            }
            //int value = (int)(finalValue / Math.Pow(10, 7 - idx));
            //Console.WriteLine("value:" + value);
            //numberGroupMain.TurnStopAt(idx, value);

            Console.WriteLine("stopCounter:" + stopCounter);
            if (stopCounter >= 9) // take care of this number!!!
            {
                stopTimer.Stop();
                isAllTurnStopped = true;
            }
        }

        //timer tick
        void timer_Tick(object sender, EventArgs e)
        {
            if (isAllTurnStopped && numberGroupMain.IsStoped())
            {
                stopTimer.Stop();
                lbWinner.Content = finalValueDesc;
                winnerTimer.Stop();
                buttonStart.IsEnabled = true;

                //hide last number for i-user
                if (isIUser)
                {
                    numberGroupMain.HideNumberAt(lastNumberItem, true);
                }

                ShowAnimation();

                drawStatus = DrawStatus.STOPPED;
                isAllTurnStopped = false;
            }
        }

        private void ShowAnimation()
        {
            anim1.From = 1;
            anim1.To = 80;
            anim1.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            Storyboard.SetTargetName(anim1, lbWinner.Name);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Label.FontSizeProperty));

            story.Children.Add(anim1);

            anim2.From = 80;
            anim2.To = 48;
            anim2.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            anim2.BeginTime = TimeSpan.FromMilliseconds(300);
            Storyboard.SetTargetName(anim2, lbWinner.Name);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(Label.FontSizeProperty));

            story.Children.Add(anim2);

            story.Begin(this, true);
        }

        private void DoDraw()
        {
            if (drawStatus == DrawStatus.STOPPING)
            {
                return;
            }
            else if (drawStatus == DrawStatus.STOPPED) // start
            {
                StartDraw();
            }
            else // stop
            {
                SelectWinner();
            }
        }

        //start button handler
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            DoDraw();
        }

        //stop button handler
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            SelectWinner();
        }

        private void StartDraw()
        {
            if (!_logic.IsAbleToDraw())
            {
                showCannotDrawMsg();
                return;
            }

            numberGroupMain.HideNumberAt(lastNumberItem, false);
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            lbWinner.Content = "";
            winnerTimer.Stop();
            stopCounter = delayStopFrom;

            drawStatus = DrawStatus.RUNNING;
        }

        private void SelectWinner()
        {
            buttonStop.IsEnabled = false;
            //Random random = new Random();
            //finalValue = random.Next(100000);
            Candidate cdt = _logic.DoDraw();
            if (cdt != null)
            {
                finalValueDesc = cdt.Id.Trim() + "   " + cdt.Name.Trim();
                string winnerId = cdt.Id.Trim();

                //i-user only has 7 chars
                isIUser = false;
                if (winnerId.Length == 7)
                {
                    isIUser = true;
                    winnerId += "0";
                }

                //change user char to number
                string idChar = winnerId.Substring(0, 1).ToLower();
                winnerId = dictIdChar[idChar] + winnerId.Substring(1);

                finalValue = Convert.ToInt32(winnerId); //remove first char
                //numberGroupMain.TurnStop(finalValue);//stop
                //stop the 2, 3 & 4 number immediatly
                //get the number
                TurnStopAt(2);
                TurnStopAt(3);
                TurnStopAt(4);

                stopTimer.Start();
                winnerTimer.Start();

                drawStatus = DrawStatus.STOPPING;
            }
            else
            {
                showCannotDrawMsg();
                numberGroupMain.TurnStop(finalValue);//stop
                winnerTimer.Stop();

                drawStatus = DrawStatus.STOPPED;
            }
        }

        private void TurnStopAt(int idx)
        {
            int value = (int)(finalValue / Math.Pow(10, 7 - idx));
            Console.WriteLine("idx: " + idx + "  value: " + value);
            numberGroupMain.TurnStopAt(idx, value);
        }

        private void showCannotDrawMsg()
        {
            lbMsg.Content = "No candidate left! Please reset and try again.";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoDraw();
            }
        }

        private void btnSunny_Click(object sender, RoutedEventArgs e)
        {
            SunnyWindow sw = new SunnyWindow();
            sw.Show();
        }

        private void btnRest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to reset?", "SYSTEM MESSAGE", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _logic.ResetApp();
                lbMsg.Content = "Please press ENTER to start!";
            }
        }

        private void btnSunny_GotFocus(object sender, RoutedEventArgs e)
        {
            btnBlank.Focus();
        }

        private void btnRest_GotFocus(object sender, RoutedEventArgs e)
        {
            btnBlank.Focus();
        }
    }
}

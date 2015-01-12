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
        private DrawLogic _logic = new DrawLogic();
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

        //监视转动是否停止，如果停止，显示价格
        void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                // textBoxFinalPrice.Text = "￥" + finalValue.ToString("F2");//显示最终金额
                lbWinner.Content = finalValueDesc;
                timer.Stop();
                buttonStart.IsEnabled = true;
            }
        }

        //开始按钮点击
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
            //textBoxFinalPrice.Text = "";
            lbWinner.Content = "Winner";
            timer.Stop();
        }

        //停止按钮点击
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
                numberGroupMain.TurnStop(finalValue);//使数字组停止
                timer.Start();
            }
            else
            {
                showCannotDrawMsg();
                numberGroupMain.TurnStop(finalValue);//使数字组停止
                timer.Stop();
            }
        }

        private void showCannotDrawMsg()
        {
            lbMsg.Content = "No candidate left!";
        }
    }
}

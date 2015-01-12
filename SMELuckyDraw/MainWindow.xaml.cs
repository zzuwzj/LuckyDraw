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

namespace SMELuckyDraw
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private int finalValue = 0;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
        }

        //监视转动是否停止，如果停止，显示价格
        void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                textBoxFinalPrice.Text = "￥" + finalValue.ToString("F2");//显示最终金额
                timer.Stop();
                buttonStart.IsEnabled = true;
            }
        }

        //开始按钮点击
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            textBoxFinalPrice.Text = "";
            timer.Stop();
        }

        //停止按钮点击
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            buttonStop.IsEnabled = false;
            Random random = new Random();
            finalValue = random.Next(100000);
            numberGroupMain.TurnStop(finalValue);//使数字组停止
            timer.Start();
        }
    }
}

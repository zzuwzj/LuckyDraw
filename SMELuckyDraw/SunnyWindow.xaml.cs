using SMELuckyDraw.Logic;
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

        public SunnyWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }

        //timer tick
        void timer_Tick(object sender, EventArgs e)
        {
            _counter++;
            if (_counter % 5 == 0)
            {
                int count = nameGroupMain.AddRandomName();
                if (count >= 30)
                {
                    timer.Stop();
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //nameGroupMain.TurnStart();
            timer.Start();
        }
    }
}

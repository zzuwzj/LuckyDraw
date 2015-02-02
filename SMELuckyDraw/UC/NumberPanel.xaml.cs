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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMELuckyDraw.UC
{
    /// <summary>
    /// NumberPanel.xaml 的交互逻辑
    /// </summary>
    public partial class NumberPanel : UserControl
    {
        //基础周期(秒)
        private readonly int BASE_PERIOD = 10;
        //Height
        private readonly int ITEM_HEIGHT = 150;
        private readonly int HALF_ITEM_HEIGHT = 75;

        /// <summary>
        /// 滚动速度（个/秒）
        /// </summary>
        private double _Speed = 1;
        /// <summary>
        /// 转动速度
        /// </summary>
        public double Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        Storyboard storyboard1 = new Storyboard();
        Storyboard storyboard2 = new Storyboard();
        DoubleAnimation animation1 = new DoubleAnimation();
        DoubleAnimation animation2 = new DoubleAnimation();

        //字符列表
        private string[] numberList1 = new string[] { "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        private string[] numberList2 = new string[] { "D", "I", "C", "D", "I", "C", "D", "I", "C", "D", "C", "I", "C", "D", "C", "D", "I", "C", "D", "I", "C", "I", "C", "D", "C", "I", "C", "D", "I", "C" };

        public NumberPanel(int listIndex)
        {
            InitializeComponent();
            Init(listIndex);
        }

        private void Init(int listIndex)
        {
            if (listIndex == 2)
            {
                foreach (string i in numberList2)
                {
                    NumberItem item = new NumberItem();
                    item.NumberValue = i;
                    stackPanelMain.Children.Add(item);
                }
            }
            else
            {
                foreach (string i in numberList1)
                {
                    NumberItem item = new NumberItem();
                    item.NumberValue = i;
                    stackPanelMain.Children.Add(item);
                }
            }
        }

        //开始转动
        public void TurnStart()
        {
            if (Speed <= 0)
            {
                return;
            }

            animation1.From = -1 * HALF_ITEM_HEIGHT;
            animation1.To = -1 * ITEM_HEIGHT * 10 - HALF_ITEM_HEIGHT;
            animation1.Duration = new Duration(TimeSpan.FromSeconds(BASE_PERIOD));
            animation1.SpeedRatio = Speed;
            animation1.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(animation1, stackPanelMain.Name);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.TopProperty));
            storyboard1.Children.Add(animation1);


            storyboard1.Begin(this, true);
        }

        //使转动停止在某个数字上
        public void TurnStopAt(int number, double specifiedDuration = 0)
        {
            if (Speed <= 0)
            {
                return;
            }
            double fromTop = (double)stackPanelMain.GetValue(Canvas.TopProperty);
            double toTop = -1 * ITEM_HEIGHT * (((number + 22) % 10) + 18) - HALF_ITEM_HEIGHT;

            if (fromTop - toTop > ITEM_HEIGHT * 10)
            {
                fromTop -= ITEM_HEIGHT * 10;
            }

            animation2.From = fromTop;
            animation2.To = toTop;
            double numberCount = (fromTop - toTop) / ITEM_HEIGHT;
            double duration = specifiedDuration;
            if (specifiedDuration  == 0)
            {
                duration = BASE_PERIOD * numberCount / 10 + 2;
            }
            animation2.Duration = new Duration(TimeSpan.FromSeconds(duration));
            animation2.SpeedRatio = 4;
            animation2.DecelerationRatio = 1;
            Storyboard.SetTargetName(animation2, stackPanelMain.Name);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.TopProperty));
            storyboard2.Children.Add(animation2);

            storyboard1.Stop(this);

            storyboard2.Begin(this, true);
        }


        /// <summary>
        /// 判断转动是否停止
        /// </summary>
        public bool IsStopped()
        {
            bool isStopped = storyboard2.GetCurrentState(this) != ClockState.Active;
            return isStopped;
        }

        public void HideNumber(bool bHide)
        {
            foreach (NumberItem item in stackPanelMain.Children)
            {
                item.hideNumber(bHide);
            }
        }
    }

}

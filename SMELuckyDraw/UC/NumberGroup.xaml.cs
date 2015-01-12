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

namespace SMELuckyDraw.UC
{
    /// <summary>
    /// NumberGroup.xaml 的交互逻辑
    /// </summary>
    public partial class NumberGroup : UserControl
    {
        /// <summary>
        /// 最终数字
        /// </summary>
        public int FinalValue { get; set; }

        private List<NumberPanel> listNumber = new List<NumberPanel>();

        public NumberGroup()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            double baseSpeed = 8;//基础速度
            double stepSpeed = 0.1;//累加速度
            double randomSpeed = 3;//随机速度范围
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                NumberPanel number = new NumberPanel();
                number.Speed = baseSpeed + (stepSpeed * i) + random.NextDouble() * randomSpeed;
                stackPanelMain.Children.Add(number);
                listNumber.Add(number);
            }
        }

        public void TurnStart()
        {
            foreach (var item in listNumber)
            {
                item.TurnStart();
            }
        }

        public void TurnStop(int number)
        {
            for (int i = 0; i < 6; i++)
            {
                int value = (int)(number / Math.Pow(10, 5 - i));
                var item = listNumber[i];
                item.TurnStopAt(value);
            }
        }

        /// <summary>
        /// 判断停止状态
        /// </summary>
        /// <returns></returns>
        public bool IsStoped()
        {
            foreach (var item in listNumber)
            {
                if (!item.IsStopped())
                {
                    return false;
                }
            }
            return true;
        }
    }
}

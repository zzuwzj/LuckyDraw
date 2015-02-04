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
    /// NameItem.xaml 的交互逻辑
    /// </summary>
    public partial class NameItem : UserControl
    {
        private string _NameValue;
        /// <summary>
        /// 数字值
        /// </summary>
        public string NameValue
        {
            get
            {
                return _NameValue;
            }
            set
            {
                lbNameText.Content = value.ToString();
                _NameValue = value;
            }
        }

        Storyboard storyBoard = new Storyboard();
        DoubleAnimation anim1 = new DoubleAnimation(); //font size animation
        DoubleAnimation anim2 = new DoubleAnimation();

        public NameItem()
        {
            InitializeComponent();
        }

        public void TurnStart()
        {
            anim1.From = 1;
            anim1.To = 40;
            anim1.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            //anim1.SpeedRatio = Speed;
            //anim1.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(anim1, lbNameText.Name);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Label.FontSizeProperty));
            storyBoard.Children.Add(anim1);

            anim2.From = 40;
            anim2.To = 30;
            anim2.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            anim2.BeginTime = TimeSpan.FromMilliseconds(300);
            //anim1.SpeedRatio = Speed;
            //anim2.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(anim2, lbNameText.Name);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(Label.FontSizeProperty));
            storyBoard.Children.Add(anim2);

            storyBoard.Begin(this, true);
        }
    }
}

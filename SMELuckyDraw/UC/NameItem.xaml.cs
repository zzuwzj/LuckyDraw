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
                textBlockNameText.Text = value.ToString();
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
            anim1.From = 5;
            anim1.To = 20;
            anim1.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            //anim1.SpeedRatio = Speed;
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(anim1, textBlockNameText.Name);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(TextBlock.FontSizeProperty));
            storyBoard.Children.Add(anim1);

            anim2.From = 0.5;
            anim2.To = 1;
            anim2.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            //anim1.SpeedRatio = Speed;
            anim2.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(anim2, textBlockNameText.Name);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(TextBlock.OpacityProperty));
            storyBoard.Children.Add(anim2);

            storyBoard.Begin(this, true);
        }
    }
}

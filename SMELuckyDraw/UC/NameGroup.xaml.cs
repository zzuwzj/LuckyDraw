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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMELuckyDraw.UC
{
    /// <summary>
    /// NameGroup.xaml 的交互逻辑
    /// </summary>
    public partial class NameGroup : UserControl
    {
        private List<NameItem> listName = new List<NameItem>();
        private DrawLogic _logic = DrawLogic.Instance();

        public NameGroup()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //Random random = new Random();
            //for (int i = 0; i < 60; i++)
            //{
            //    NameItem name = new NameItem();
            //    wrapPanelMain.Children.Add(name);
            //    listName.Add(name);
            //}
        }

        public int AddRandomName()
        {
            if (listName.Count < 30)
            {
                Candidate cdt = _logic.DoDraw();
                if (cdt != null)
                {
                    NameItem name = new NameItem();
                    name.NameValue = cdt.Name;
                    wrapPanelMain.Children.Add(name);
                    listName.Add(name);
                }
            }
            return listName.Count;
        }

        public void TurnStart()
        {
            foreach (NameItem item in listName)
            {
                Candidate cdt = _logic.FreeDraw();
                if (cdt != null)
                {
                    item.NameValue = cdt.Name;
                }
                else
                {
                    //
                }
                item.TurnStart();
            }
        }

        public void TurnStop(int number)
        {
            for (int i = 0; i < 6; i++)
            {
                int value = (int)(number / Math.Pow(10, 5 - i));
                var item = listName[i];
            }
        }
    }
}

using Mirai_CSharp.Models;
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

namespace PioneerDanmu
{
    /// <summary>
    /// Interaction logic for DanmuItem.xaml
    /// </summary>
    public partial class DanmuItem : UserControl
    {
        public DanmuItem()
        {
            InitializeComponent();
        }


        public DanmuItem(DanmuModel model)
        {
            InitializeComponent();
            avatorImg.Source = new BitmapImage(new Uri(model.AvatorUrl));
            nameText.Text = model.DisplayName;

            foreach (var item in model.EncodedContent)
            {
                if(item is PlainMessage message)
                {
                    contentText.Inlines.Add(new Run(message.Message));
                }
                else if(item is ImageMessage m)
                {
                    contentText.Inlines.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri(m.Url)),
                        MaxHeight = 200
                    });
                }
                else if(item is FaceMessage m1)
                {
                    contentText.Inlines.Add(new Run($"[/{m1.Name}]"));
                }
                else if(item is SourceMessage)
                { 
                }
                else
                {
                    contentText.Inlines.Add(new Run("[暂不支持的消息类型]"));
                }

            }

            
        }
    }
}

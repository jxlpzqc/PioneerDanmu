using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PioneerDanmu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public void SendDanmu(DanmuModel danmu)
        {
            foreach (var item in Windows)
            {
                if(item is DanmuWindow window)
                {
                    window.CurrentManager.AddDanmu(danmu);
                }

            }
        }
    }
}

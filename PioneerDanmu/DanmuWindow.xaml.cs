using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PioneerDanmu
{
    /// <summary>
    /// Interaction logic for DanmuWindow.xaml
    /// </summary>
    public partial class DanmuWindow : Window
    {
        public DanmuWindow()
        {
            InitializeComponent();
            manager.DanmuAdded += (r) =>
            {
                CheckAndAdd(r);
            };
            this.Loaded += (s, e) =>
            {
                SetHitTestInvisible();
            };
        }

        #region P/Invoke Part


        public const int WS_EX_TRANSPARENT = 0x20;
        
        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }


        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        #endregion


        private void SetHitTestInvisible()
        {
            var handle = new WindowInteropHelper(this).Handle;
            var style = GetWindowLongPtr(handle, (int)GWL.GWL_EXSTYLE).ToInt32();
            SetWindowLong(handle, (int)GWL.GWL_EXSTYLE, (uint)(style | WS_EX_TRANSPARENT));

        }

        private async Task CheckAndAdd(int row)
        {
            if (!isBusy[row]&&manager.HasDanmu(row))
            {
                isBusy[row] = true;

                var danmu = manager.GetDanmu(row);
                var danmuItem = new DanmuItem(danmu);
                danmuItem.SetValue(Canvas.TopProperty, GetTop(row));
                danmuItem.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.LowQuality);
                canvas.Children.Add(danmuItem);

                danmuItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                danmuItem.Arrange(new Rect(0, 0, danmuItem.DesiredSize.Width, danmuItem.DesiredSize.Height));
                var width = danmuItem.ActualWidth;
                var screenWidth = canvas.ActualWidth;

                var animation = new DoubleAnimation(screenWidth, -width, new Duration(TimeSpan.FromMilliseconds(passingTime)));
                


                var speed = (screenWidth + width) / passingTime;
        
                danmuItem.BeginAnimation(Canvas.LeftProperty, animation);

                var finishEndedTime = (int)(width / speed);
                await Task.Delay(finishEndedTime);
                isBusy[row] = false;
                CheckAndAdd(row);

                await Task.Delay(passingTime - finishEndedTime);

                canvas.Children.Remove(danmuItem);
            }

        }

        private const int passingTime = 10000;

        private bool[] isBusy = new bool[rowNum]; 

        private const int rowNum = 5;

        private DanmuManager manager = new DanmuManager();

        public DanmuManager CurrentManager => manager;

        private double GetTop(int row)
        {
            return row * this.ActualHeight / rowNum;
        }


    }
}

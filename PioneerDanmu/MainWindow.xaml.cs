﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PioneerDanmu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //前往
            if(Screen.AllScreens.Length < 2)
            {
                System.Windows.MessageBox.Show("程序检测到只连接了一个显示器，弹幕将在主屏上运行。如连接显示器，请重新启动应用程序。");
               
            }
            else
            {
                window.WindowState = WindowState.Normal;
                var screen = Screen.AllScreens[1].Bounds;
                window.Left = screen.X;
                window.Top = screen.Y;
                window.Width = screen.Width;
                window.Height = screen.Height;
            }
            window.Show();

            MessageHandler.Configure();

        }

        private DanmuWindow window = new DanmuWindow();
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            window.CurrentManager.AddDanmu(new DanmuModel
            {
                GroupCard = "测试",
                EncodedContent = new Mirai_CSharp.Models.IMessageBase[]
                {
                    new Mirai_CSharp.Models.PlainMessage("这是一条测试弹幕")
                },
                AvatorUrl = "https://pic1.zhimg.com/v2-383e1a20fc7503a0d0a74e67b0112b68_im.jpg"
            });

        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            window.Show();
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            window.Hide();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            window.Opacity = e.NewValue / 100;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }

   
}

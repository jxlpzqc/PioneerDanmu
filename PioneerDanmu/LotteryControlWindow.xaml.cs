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

namespace PioneerDanmu
{
    /// <summary>
    /// Interaction logic for LotteryControlWindow.xaml
    /// </summary>
    public partial class LotteryControlWindow : Window
    {

        public event EventHandler Toggled;

        private LotteryRect rect;
        private QuestionModel question;

        public LotteryControlWindow(LotteryRect rect,QuestionModel question)
        {
            this.rect = rect;
            this.question = question;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rect.Start(question.Content, question.RightAnswer, question.Seconds);

            Task.Run(async () =>
            {
                await Task.Delay(question.Seconds * 1000);
                Dispatcher.Invoke(() =>
                {
                    btn.IsEnabled = true;
                });
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            rect.Finish();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rect.ToggleLottery();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for LotteryRect.xaml
    /// </summary>
    public partial class LotteryRect : UserControl
    {
        public LotteryRect()
        {
            InitializeComponent();
            timer.Elapsed += Timer_Elapsed;
            Reset();
        }

        public void Reset()
        {
            cancelFlag = false;
            senders.Clear();
            this.Visibility = Visibility.Collapsed;
            questionGrid.Visibility = Visibility.Visible;
            lotterySp.Visibility = Visibility.Collapsed;
            rightAvatorSp.Children.Clear();
        }

        private string content;
        private string rightAnswer;
        private Task timeTask;
        private bool cancelFlag = false;

        class Sender
        {
            public string QQNumber { get; set; }
            public string NickName { get; set; }
            public string AvatorUrl =>
                $"https://qlogo4.store.qq.com/qzone/{QQNumber}/{QQNumber}/100";

        }

        private List<Sender> senders = new List<Sender>();

        private Sender selectedUser;

        public void Start(string content, string rightAnswer, int seconds)
        {
            Reset();
            this.Visibility = Visibility.Visible;
            this.content = content;
            this.rightAnswer = rightAnswer;

            this.contentText.Text = content;

            MessageHandler.MessageReceived += MessageHandler_MessageReceived;

            MessageHandler.SendMessage("【回答正确加入抽奖】\n" +
                content +
                "\n加油！😝😝");

            Task.Run(async () =>
            {
                for (int i = seconds; i >= 0; i--)
                {
                    if (cancelFlag)
                    {
                        cancelFlag = false;
                        return;
                    }

                    Dispatcher.Invoke(() =>
                    {
                        timeText.Text = i.ToString();
                    });

                    await Task.Delay(1000);
                }
                Dispatcher.Invoke(() =>
                {
                    EndAnswer();
                });
            });


        }

        private void AddSender(long qqnumber,string nickname)
        {
            var sender = new Sender
            {
                QQNumber = qqnumber.ToString(),
                NickName = nickname
            };
            senders.Add(sender);
            
            if (rightAvatorSp.Children.Count < 8) 
            { 
                rightAvatorSp.Children.Add(new Image()
                {
                    Source = new BitmapImage(new Uri(sender.AvatorUrl)),
                    Height = 30,
                    Width = 30,
                    Margin = new Thickness(4, 0, 4, 0)

                });
            }
        }

        private void MessageHandler_MessageReceived(Mirai_CSharp.Models.IGroupMessageEventArgs obj)
        {
            if (string.Join("", obj.Chain.Skip(1).Select(u => u.ToString())) == rightAnswer)
            {
                if (senders.Any(u => u.QQNumber == obj.Sender.Id.ToString())) return;
                AddSender(obj.Sender.Id, obj.Sender.Name);

                Dispatcher.Invoke(() =>
                {
                    rightNumText.Text = senders.Count.ToString();
                });
            }

        }

        public void EndAnswer()
        {
            MessageHandler.MessageReceived -= MessageHandler_MessageReceived;

            questionGrid.Visibility = Visibility.Collapsed;
            lotterySp.Visibility = Visibility.Visible;

        }

        private Timer timer = new Timer(100);

        public void StartLottery()
        {
            timer.Start();
        }

        public void PauseLottery()
        {
            timer.Stop();
            MessageHandler.SendMessage($"【恭喜！】\n恭喜 @{selectedUser.NickName} (QQ账号：{selectedUser.QQNumber})中奖！");

        }


        public void ToggleLottery()
        {
            if (timer.Enabled) PauseLottery();
            else StartLottery();
        }


        public void Finish()
        {
            timer.Stop();
            cancelFlag = true;
            MessageHandler.MessageReceived -= MessageHandler_MessageReceived;

            this.Visibility = Visibility.Collapsed;
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!senders.Any())
            {
                Dispatcher.Invoke(() =>
                {
                    nameText.Text = "Sorry，无人回答正确";
                });
                return;
            }

            var r = new Random().Next(0, senders.Count);
            selectedUser = senders[r];
            Dispatcher.Invoke(() =>
            {
                nameText.Text = selectedUser.NickName;
                avatorImg.Source = new BitmapImage(new Uri(selectedUser.AvatorUrl));
            });


        }
    }
}

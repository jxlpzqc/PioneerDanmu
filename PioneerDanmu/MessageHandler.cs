using Mirai_CSharp;
using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PioneerDanmu
{
    class MessageHandler
    {
        private static MiraiHttpSession session;

        private static long groupID;

        public static event Action<IGroupMessageEventArgs> MessageReceived;

        public static async Task Configure()
        {
            MiraiHttpSessionOptions options = new MiraiHttpSessionOptions(ConfigurationManager.AppSettings.Get("host"),
                Convert.ToInt32(ConfigurationManager.AppSettings.Get("port")),
                ConfigurationManager.AppSettings.Get("authKey"));

            groupID = Convert.ToInt64(ConfigurationManager.AppSettings.Get("groupID"));

            // session 使用 DisposeAsync 模式, 所以使用 await using 自动调用 DisposeAsync 方法。
            // 你也可以不在这里 await using, 不过使用完 session 后请务必调用 DisposeAsync 方法
            session = new MiraiHttpSession();
            // 把你实现了 Mirai_CSharp.Plugin.Interfaces 下的接口的类给 new 出来, 然后作为插件塞给 session
            Plugin plugin = new Plugin();
            // 你也可以一个个绑定事件。比如 session.GroupMessageEvt += plugin.GroupMessage;
            // 手动绑定事件后不要再调用AddPlugin, 否则可能导致重复调用
            session.AddPlugin(plugin);
            try
            {
                // 使用上边提供的信息异步连接到 mirai-api-http
                await session.ConnectAsync(options, Convert.ToInt64(ConfigurationManager.AppSettings.Get("qqNumber"))); // 自己填机器人QQ号
            }
            catch
            {
                MessageBox.Show("程序连接Mirai服务器失败，该程序目前只能测试弹幕使用，如需正常使用，请配置正确后重新启动程序。");
            }

        }

        public static async Task SendMessage(string message)
        {
            await session.SendGroupMessageAsync(groupID, new IMessageBase[] 
            { 
                new PlainMessage(message)
            });
        }


        public class Plugin : IPlugin<IGroupMessageEventArgs>, IPlugin
        {
            public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
            {
                if (e.Sender.Group.Id != groupID) return false;

                (App.Current as App).SendDanmu(new DanmuModel
                {
                    AvatorUrl = $"https://qlogo4.store.qq.com/qzone/{e.Sender.Id}/{e.Sender.Id}/100",
                    NickName = e.Sender.Name,
                    QQNumber = e.Sender.Id.ToString(),
                    EncodedContent = e.Chain
                });

                MessageReceived?.Invoke(e);

                return true;
            }

            public Task<bool> HandleEvent(MiraiHttpSession session, IGroupMessageEventArgs e)
            {
                return GroupMessage(session, e);
            }
        }
    }
}

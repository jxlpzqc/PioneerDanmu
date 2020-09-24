using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PioneerDanmu
{
    public class DanmuManager
    {
        private Queue<DanmuModel>[] items;

        public void AddDanmu(DanmuModel danmu)
        {
            int minn = int.MaxValue;
            int mini = 0;
            List<int> miniList = new List<int>();

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item.Count < minn)
                {
                    minn = item.Count;
                    mini = i;
                    miniList.Clear();
                    miniList.Add(mini);
                }
                else if(item.Count == minn)
                {
                    miniList.Add(i);
                }
            }

            var selectedRow = miniList[new Random().Next(0, miniList.Count)];

            items[selectedRow].Enqueue(danmu);

            DanmuAdded?.Invoke(selectedRow);
        }

        public DanmuModel GetDanmu(int row)
        {
            return items[row].Dequeue();
        }
        
        public event Action<int> DanmuAdded;

        public bool HasDanmu(int row)
        {
            return items[row].Any();
        }

        public DanmuManager(int rows = 5)
        {
            items = new Queue<DanmuModel>[rows];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Queue<DanmuModel>();
            }
        }



    }
}

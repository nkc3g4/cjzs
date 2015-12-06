using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 攒机助手
{
    public class HardWare
    {
        /// <summary>
        /// 单个硬件类
        /// </summary>
        /// <param name="name">硬件名称</param>
        /// <param name="price">硬件价格</param>
        public HardWare(string name, int price)
        {
            this.Price = price;
            this.Name = name;
        }
        private int price;

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            if (this.Price == 0) return this.name;
            else { return (this.Name + CreateSpace(30 - System.Text.Encoding.Default.GetBytes(this.Name).Length) + "￥" + this.Price); }
        }
        private string CreateSpace(int num)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= num; i++)
            {
                sb.Append(" ");
            }
            return sb.ToString ();
        }
    }
}

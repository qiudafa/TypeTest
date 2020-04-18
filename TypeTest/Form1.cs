using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TypeTest
{
    public partial class Form1 : Form
    {

        #region 变量

        public Hashtable ht = new Hashtable();
        public List<string> failString = new List<string>();
        public List<Label> labels = new List<Label>();
        public int life;
        public int falieCount;
        public int SPEED;
        public int NUM;
        public int LEVEL;
        public bool stopORcontinue;
        public int score;
        public string APP_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
        public INIhelper.IniHelper iniHelper = new INIhelper.IniHelper();

        #endregion

        public Form1()
        {
            InitializeComponent();
            this.LEVEL = 1;

        }

        public void init()
        {
            this.life = 10;
            this.score = 0;
            this.SPEED = Convert.ToInt32(iniHelper.Read("setting", "firstSPEED","5",APP_PATH));
            this.NUM = Convert.ToInt32(iniHelper.Read("setting", "firstNUM", "30", APP_PATH));
            this.LEVEL = Convert.ToInt32(iniHelper.Read("setting", "firstLEVEL", "1", APP_PATH));
            this.falieCount = 0;
            this.Text = this.panel1.Location.ToString();
            this.stopORcontinue = false;
            this.btnStop.Text = "停止";
            this.lblScore.Text = "";
            this.labels = new List<Label>()
            {
                this.label1,
                this.label2,
                this.label3,
                this.label4,
                this.label5,
                this.label6,
                this.label7,
                this.label8,
                this.label9,
                this.label10,
            };
            foreach(DictionaryEntry item in ht)
            {
                Label lab = (Label)item.Value;
                lab.Text = "";
                lab.Dispose();
            }
            this.ht.Clear();
            this.failString.Clear();
            foreach (Label lab in this.labels)
            {
                lab.Text = "  ";
            }

        }

        /*测试代码
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //this.Text = e.KeyValue.ToString();
            this.Text = this.label1.Location.Y.ToString();
            switch (e.KeyValue)
            {
                case 38:
                    this.label1.Location = new Point(label1.Location.X, label1.Location.Y + 10);
                    break;
                case 40:
                    this.label1.Location = new Point(label1.Location.X, label1.Location.Y - 10);
                    break;
                case 37:
                    this.label1.Location = new Point(label1.Location.X - 10, label1.Location.Y);
                    break;
                case 39:
                    this.label1.Location = new Point(label1.Location.X + 10, label1.Location.Y);
                    break;
            }
        }*/
        

        private void timerCreat_Tick(object sender, EventArgs e)
        {
            Label lab = new Label();
            lab.AutoSize = true;
            lab.Font = new Font("Microsoft Sans Serif", 20,
            FontStyle.Regular, GraphicsUnit.Point, 0); //设置字体
            Random r = new Random();
            lab.ForeColor = Color.FromArgb(r.Next(100, 255), r.Next(100, 255), r.Next(100, 255));
            lab.TextAlign = ContentAlignment.MiddleCenter; //文本内容居中
            string labText = getRandomChar();
            lab.Text = labText;
            lab.Name = labText + DateTime.Now.Millisecond.ToString();
            lab.Location = new Point(r.Next(30, this.panel1.Width - 40), 0);

            this.panel1.Controls.Add(lab);
            this.ht.Add(lab.Name, lab);

        }

        private void timerScrow_Tick(object sender, EventArgs e)
        {
            if (life == 0)
            {
                this.timerScrow.Enabled = false;
                this.timerCreat.Enabled = false;
                return;
            }
            foreach(DictionaryEntry item in ht)
            {
                Label lab = (Label)item.Value;
                if (lab.Location.Y >= this.panel1.Location.Y + this.panel1.Height)
                {

                    failString.Add(lab.Name);
                    life--;
                    this.labels[falieCount++].Text = lab.Text;
                    lab.Dispose();

                }
                else
                {
                    lab.Location = new Point(lab.Location.X, lab.Location.Y + this.SPEED * (LEVEL/2 + 1));
                }
            }
            foreach(string s in failString)
            {
                ht.Remove(s);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            init();
            this.timerCreat.Interval = 1100 + 500*(5-LEVEL)/5;
            this.timerCreat.Enabled = true;
            this.timerScrow.Interval = 50;
            this.timerScrow.Enabled = true;
        }

        public string getRandomChar()
        {
            string[] strs = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
            Random r = new Random();
            return strs[r.Next(0, 25)];
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopORcontinue = !stopORcontinue;
            if (stopORcontinue)
            {
                this.timerScrow.Enabled = false;
                this.timerCreat.Enabled = false;
                this.btnStop.Text = "继续";
            }
            else
            {
                this.timerScrow.Enabled = true;
                this.timerCreat.Enabled = true;
                this.btnStop.Text = "停止";
            }
                

            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue < 65 || e.KeyValue > 90) return;
            foreach(DictionaryEntry item in ht)
            {
                
                if (item.Key.ToString().IndexOf(Convert.ToChar(e.KeyCode)) != -1)
                {
                    Label lab = (Label)item.Value;
                    lab.BackColor = Color.Red;
                    lab.Dispose();
                    ht.Remove(item.Key);
                    this.lblScore.Text = (++this.score).ToString();
                    break;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            form2.Focus();
        }
    }
}

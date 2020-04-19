using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        public Random r = null;
        public int life;
        public int falieCount;
        public int createNum;
        public int SPEED;
        public int NUM;
        public int LEVEL;
        public int failCountNum;
        public bool stopORcontinue;
        public int score;
        public string APP_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
        public INIhelper.IniHelper iniHelper = new INIhelper.IniHelper();
        FileStream fs = null;
        SoundPlayer sp = null;

        //背景音乐部分，本来想做后面懒得做了，可以参考https://www.cnblogs.com/friendan/archive/2012/11/24/2786181.html
        FileStream fsbgm = null;
        SoundPlayer spbgm = null;

        #endregion

        public Form1()
        {
            InitializeComponent();
            this.LEVEL = 1;
            iniHelper.Write("setting", "firstLEVEL", "1", APP_PATH);

        }

        public void init()
        {

            this.score = 0;
            this.createNum = 0;
            this.life = Convert.ToInt32(iniHelper.Read("setting", "firstFailCount", "10", APP_PATH));
            this.SPEED = Convert.ToInt32(iniHelper.Read("setting", "firstSPEED", "5", APP_PATH));
            this.NUM = Convert.ToInt32(iniHelper.Read("setting", "firstNUM", "30", APP_PATH));
            this.LEVEL = Convert.ToInt32(iniHelper.Read("setting", "firstLEVEL", "1", APP_PATH));

            this.falieCount = 0;
            this.Text = "第" + LEVEL + "关";
            this.stopORcontinue = false;
            this.btnStop.Text = "停止";
            this.lblScore.Text = "";
            this.r = new Random(DateTime.Now.Millisecond); //固定随机数种子提高性能
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
            foreach (DictionaryEntry item in ht)
            {
                Label lab = (Label)item.Value;
                lab.Text = "";
                lab.Dispose();
            }
            this.ht.Clear();
            this.failString.Clear();
            int i = 0;
            foreach (Label lab in this.labels)
            {
                if (i++ >= life)
                    lab.Text = "";
                else
                    lab.Text = "  ";
            }
            PlaySound("start");
            //if (spbgm != null)
            //    spbgm.Stop();
            //
            //Thread thread = new Thread(new ThreadStart(bgmThread));
            //thread.Start();

        }


        public void GameEnd()
        {
            //spbgm.Stop();
            PlaySound("gameover");
            string scoreStr = "游戏结束！你的得分是：" + this.score.ToString() + "\t" + "是否重新开始本关卡？";
            if (MessageBox.Show(scoreStr, "游戏结束", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                init();
                this.timerCreat.Interval = 1100 + 500 * (5 - LEVEL) / 5;
                this.timerCreat.Enabled = true;
                this.timerScrow.Interval = 50;
                this.timerScrow.Enabled = true;
            }
        }

        public void GameSuccess()
        {
            PlaySound("start");
            if (LEVEL < 5)
            {

                string scoreStr = "恭喜你过关！你的得分是：" + this.score.ToString() + "\t" + "是否进入下一关？";
                if (MessageBox.Show(scoreStr, "闯关成功", MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    ++LEVEL;
                    iniHelper.Write("setting", "firstLEVEL", (LEVEL).ToString(), APP_PATH);
                    init();
                    this.timerCreat.Interval = 1100 + 500 * (5 - LEVEL) / 5;
                    this.timerCreat.Enabled = true;
                    this.timerScrow.Interval = 50;
                    this.timerScrow.Enabled = true;
                }
            }
            else
            {
                string scoreStr = "恭喜你过关！你的得分是：" + this.score.ToString() + "\t" + "是否重新开始？";
                if (MessageBox.Show(scoreStr, "闯关成功", MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    init();
                    iniHelper.Write("setting", "firstLEVEL", "1", APP_PATH);
                    this.LEVEL = 1;
                    this.timerCreat.Interval = 1100 + 500 * (5 - LEVEL) / 5;
                    this.timerCreat.Enabled = true;
                    this.timerScrow.Interval = 50;
                    this.timerScrow.Enabled = true;
                }
            }

        }

        public void PlaySound(string name)
        {
            switch (name)
            {
                case "eat":
                    fs = new FileStream("resource/eat.wav", FileMode.Open);
                    sp = new SoundPlayer(fs);
                    sp.Play();
                    fs.Close();
                    break;
                case "gameover":
                    fs = new FileStream("resource/gameover.wav", FileMode.Open);
                    sp = new SoundPlayer(fs);
                    sp.Play();
                    fs.Close();
                    break;
                case "start":
                    fs = new FileStream("resource/start.wav", FileMode.Open);
                    sp = new SoundPlayer(fs);
                    sp.Play();
                    fs.Close();
                    break;
                case "bgm2":
                    //Thread thread = new Thread(new ThreadStart(bgmThread));
                    //thread.Start();
                    break;

            }

        }

        public void bgmThread()
        {
            //if (spbgm != null)
            //    spbgm.Stop();
            //fsbgm = new FileStream("resource/bgm2.wav", FileMode.Open);
            //spbgm = new SoundPlayer(fsbgm);
            //spbgm.PlayLooping();
        }

 
        private void timerCreat_Tick(object sender, EventArgs e)
        {
            Label lab = new Label();
            lab.AutoSize = true;
            lab.Font = new Font("Microsoft Sans Serif", 20,
            FontStyle.Regular, GraphicsUnit.Point, 0); //设置字体

            lab.ForeColor = Color.FromArgb(r.Next(50, 240), r.Next(50, 240), r.Next(50, 240));
            lab.TextAlign = ContentAlignment.MiddleCenter; //文本内容居中
            string labText = getRandomChar();
            lab.Text = labText;
            lab.Name = labText + DateTime.Now.Millisecond.ToString();
            lab.Location = new Point(r.Next(this.panel1.Location.X + 30, this.panel1.Location.X + this.panel1.Width - 60), 0);

            this.panel1.Controls.Add(lab);
            this.ht.Add(lab.Name, lab);
            createNum += 1;

        }

        private void timerScrow_Tick(object sender, EventArgs e)
        {
            if (life == 0)
            {
                if (spbgm != null)
                    spbgm.Stop();
                this.timerScrow.Enabled = false;
                this.timerCreat.Enabled = false;
                GameEnd();
                return;
            }
            if (ht.Count == 0 && createNum == NUM)
            {
                this.timerScrow.Enabled = false;
                this.timerCreat.Enabled = false;
                GameSuccess();
                return;
            }
            foreach (DictionaryEntry item in ht)
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
                    lab.Location = new Point(lab.Location.X, lab.Location.Y + this.SPEED * (LEVEL / 2 + 1));
                }
            }
            foreach (string s in failString)
            {
                ht.Remove(s);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            init();
            this.timerCreat.Interval = 1100 + 500 * (5 - LEVEL) / 5;
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
            foreach (DictionaryEntry item in ht)
            {

                if (item.Key.ToString().IndexOf(Convert.ToChar(e.KeyCode)) != -1)
                {
                    Label lab = (Label)item.Value;
                    lab.BackColor = Color.Red;
                    lab.Dispose();
                    ht.Remove(item.Key);
                    this.lblScore.Text = (++this.score).ToString();
                    PlaySound("eat");
                    break;
                }
            }
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            bool isStart = false; //右击时游戏是否开始
            if (this.timerCreat.Enabled == true) isStart = true;
            this.timerScrow.Enabled = false;
            this.timerCreat.Enabled = false;
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (form2.DialogResult == DialogResult.Cancel && isStart)
            {
                this.timerScrow.Enabled = true;
                this.timerCreat.Enabled = true;
            }
        }

    }
}

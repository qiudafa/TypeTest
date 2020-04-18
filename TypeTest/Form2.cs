using System;
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
    public partial class Form2 : Form
    {
        public int SPEED;
        public int NUM;
        public int LEVEL;
        public bool stopORcontinue;
        public int score;
        public string APP_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
        public INIhelper.IniHelper iniHelper = new INIhelper.IniHelper();

        public Form2()
        {
            InitializeComponent();
            this.SPEED = Convert.ToInt32(iniHelper.Read("setting", "firstSPEED", "5", APP_PATH));
            this.NUM = Convert.ToInt32(iniHelper.Read("setting", "firstNUM", "30", APP_PATH));
            this.LEVEL = Convert.ToInt32(iniHelper.Read("setting", "firstLEVEL", "1", APP_PATH));
            this.textBox1.Text = this.SPEED.ToString();
            this.textBox2.Text = this.NUM.ToString();
            this.textBox3.Text = this.LEVEL.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim())|| string.IsNullOrEmpty(this.textBox2.Text.Trim())|| string.IsNullOrEmpty(this.textBox3.Text.Trim()))
            {
                MessageBox.Show("选项不能为空！");
                return;
            }
            iniHelper.Write("setting", "firstSPEED", this.textBox1.Text.Trim(), APP_PATH);
            iniHelper.Write("setting", "firstNUM", this.textBox2.Text.Trim(), APP_PATH);
            iniHelper.Write("setting", "firstLEVEL", this.textBox3.Text.Trim(), APP_PATH);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

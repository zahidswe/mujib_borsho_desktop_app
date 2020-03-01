using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MujibBorsoCountDownApp
{
    public partial class Form1 : Form
    {
        private double timeLeft;
        private TimeSpan timeLeftMS;
        private DateTime endDate = DateTime.Parse("17-March-2020");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                var panelRight = (this.Width * 85) / 485;
                var panelSpace = (this.Width * 60) / 485;
                this.panel4.Left = this.Width - panelRight;
                this.panel3.Left = this.panel4.Left - panelSpace;
                this.panel2.Left = this.panel3.Left - panelSpace;
                this.panel1.Left = this.panel2.Left - panelSpace;

                var panels = GetAll(this, typeof(Panel));

                foreach (var item in panels)
                {
                    item.Height = (this.Height * 78) / 516;
                    item.Width = item.Height;
                }


                var numberLabelFontSize = (panel4.Width * 24) / 54;
                var timeLabelFontSize = (numberLabelFontSize * 14) / 24;

                this.labelDays.Font = new Font(labelDays.Font.Name, numberLabelFontSize);
                this.labelHours.Font = new Font(labelHours.Font.Name, numberLabelFontSize);
                this.labelMinutes.Font = new Font(labelMinutes.Font.Name, numberLabelFontSize);
                this.labelSeconds.Font = new Font(labelSeconds.Font.Name, numberLabelFontSize);

                this.label1.Font = new Font(label1.Font.Name, timeLabelFontSize, FontStyle.Bold);
                this.label2.Font = new Font(label2.Font.Name, timeLabelFontSize, FontStyle.Bold);
                this.label3.Font = new Font(label3.Font.Name, timeLabelFontSize, FontStyle.Bold);
                this.label4.Font = new Font(label4.Font.Name, timeLabelFontSize, FontStyle.Bold);

                this.label4.Left = this.panel4.Left + ((panel4.Width - label4.Width) / 2);
                this.label3.Left = this.panel3.Left + ((panel3.Width - label3.Width) / 2);
                this.label2.Left = this.panel2.Left + ((panel2.Width - label2.Width) / 2);
                this.label1.Left = this.panel1.Left + ((panel1.Width - label1.Width) / 2);

                this.label1.Top = this.panel1.Top + panel1.Height;
                this.label2.Top = this.panel2.Top + panel2.Height;
                this.label3.Top = this.panel3.Top + panel3.Height;
                this.label4.Top = this.panel4.Top + panel4.Height;

                this.labelTitle.Font = new Font(labelTitle.Font.Name, (16 * this.Height) / 516);
                this.labelTitle.Left = this.panel4.Left + panel4.Width - labelTitle.Width;
                this.labelTitle.Top = label4.Top + label4.Height + ((Height * 25) / 305);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImageLayout = ImageLayout.Stretch;
            if (ConfigurationManager.AppSettings["EndTime"] != null)
                endDate = DateTime.Parse(ConfigurationManager.AppSettings["EndTime"]);
            timeLeft = Math.Ceiling(endDate.Subtract(DateTime.Now).TotalSeconds);
            if (timeLeft > 0)
            {
                updateTimer();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                updateTimer();
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Time's up!", "Time has elapsed", MessageBoxButtons.OK);
            }
        }

        private void updateTimer()
        {
            timeLeftMS = TimeSpan.FromSeconds(timeLeft);
            labelDays.Text = timeLeftMS.Days.ToString("00");
            labelHours.Text = timeLeftMS.Hours.ToString("00");
            labelMinutes.Text = timeLeftMS.Minutes.ToString("00");
            labelSeconds.Text = timeLeftMS.Seconds.ToString("00");
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }
    }
}

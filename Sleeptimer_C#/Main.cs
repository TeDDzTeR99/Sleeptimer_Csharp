using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sleeptimer_C_
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void hour_updown_ValueChanged(object sender, EventArgs e)
        {
            if (hour_updown.Value < 0) hour_updown.Value = 0;
            if (hour_updown.Value > 24) hour_updown.Value = 24;
        }

        private void minute_updown_ValueChanged(object sender, EventArgs e)
        {
            if (minute_updown.Value < 0) minute_updown.Value = 0;
            if (minute_updown.Value >= 60)
            {
                hour_updown.Value++;
                minute_updown.Value = 0;
            }
        }

        private void second_updown_ValueChanged(object sender, EventArgs e)
        {
            if (second_updown.Value < 0) second_updown.Value = 0;
            if (second_updown.Value >= 60)
            {
                minute_updown.Value++;
                second_updown.Value = 0;
            }
        }

        private void sleep_button_Click(object sender, EventArgs e)
        {
            decimal sleep_delay = (hour_updown.Value * 3600) + (minute_updown.Value * 60) + second_updown.Value;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "shutdown";
            psi.Arguments = $"/f /s /t {sleep_delay}";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process.Start(psi);
            MessageBox.Show($"Your computer will be shut down in {hour_updown.Value} Hours {minute_updown.Value} Minutes and {second_updown.Value} Seconds!");
        }

        private void abort_button_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "shutdown";
            psi.Arguments = "/a";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process.Start(psi);
            MessageBox.Show("Sleeptimer has been aborted!");
        }
    }
}

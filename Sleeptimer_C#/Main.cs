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
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(sleep_button, "Cancels any active shutdown and schedules a new shutdown");
            toolTip.SetToolTip(abort_button, "Cancel any active shutdown countdown");
        }

        private void RunShutdownCommand(string args)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            });
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
            int delay = (int)((hour_updown.Value * 3600) + (minute_updown.Value * 60) + second_updown.Value);
            RunShutdownCommand($"/f /s /t {delay}");
            MessageBox.Show(
                $"If a shutdown was active, it has been aborted.\nYour computer will shut down in {hour_updown.Value}h {minute_updown.Value}m {second_updown.Value}s.",
                "Shutdown Scheduled",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void abort_button_Click(object sender, EventArgs e)
        {
            RunShutdownCommand("/a");
            MessageBox.Show(
                    "If a shutdown was active, it has been aborted.",
                    "Shutdown",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            this.Close();
        }
    }
}

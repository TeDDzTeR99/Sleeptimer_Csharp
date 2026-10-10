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
        private Timer countdownTimer;
        private int totalSecondsLeft = 0;

        // 1. Storage variables to remember the original time input
        private decimal originalHours = 0;
        private decimal originalMinutes = 0;
        private decimal originalSeconds = 0;

        public Main()
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(sleep_button, "Starts the local countdown to shut down this PC");
            toolTip.SetToolTip(abort_button, "Cancel the active countdown and reset the timer");

            countdownTimer = new Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
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

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (totalSecondsLeft > 0)
            {
                totalSecondsLeft--;

                int hours = totalSecondsLeft / 3600;
                int minutes = (totalSecondsLeft % 3600) / 60;
                int seconds = totalSecondsLeft % 60;

                hour_updown.Value = hours;
                minute_updown.Value = minutes;
                second_updown.Value = seconds;
            }
            else
            {
                countdownTimer.Stop();
                RunShutdownCommand("/f /s /t 0");
                Application.Exit();
            }
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
            totalSecondsLeft = (int)((hour_updown.Value * 3600) + (minute_updown.Value * 60) + second_updown.Value);

            if (totalSecondsLeft <= 0)
            {
                MessageBox.Show("Please set a time greater than 0 seconds.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Remember the original values right before we start counting down
            originalHours = hour_updown.Value;
            originalMinutes = minute_updown.Value;
            originalSeconds = second_updown.Value;

            ToggleInputs(false);
            countdownTimer.Start();
        }

        private void abort_button_Click(object sender, EventArgs e)
        {
            if (countdownTimer.Enabled)
            {
                countdownTimer.Stop();
                RunShutdownCommand("/a");

                // 3. Restore the original inputs back to the UI
                hour_updown.Value = originalHours;
                minute_updown.Value = originalMinutes;
                second_updown.Value = originalSeconds;

                ToggleInputs(true);
                MessageBox.Show("Countdown has been aborted and time has been restored.", "Shutdown Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 4. If no timer is active, just reset everything back to clean 00:00:00 zeros
                hour_updown.Value = 0;
                minute_updown.Value = 0;
                second_updown.Value = 0;

                RunShutdownCommand("/a");
                MessageBox.Show("Timer cleared and any Windows native shutdown has been aborted.", "Shutdown", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToggleInputs(bool enabled)
        {
            hour_updown.Enabled = enabled;
            minute_updown.Enabled = enabled;
            second_updown.Enabled = enabled;
            sleep_button.Enabled = enabled;
        }
    }
}

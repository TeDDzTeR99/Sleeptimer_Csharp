﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sleeptimer_C_
{
    public partial class Main : Form
    {
        // Seconds Windows waits (showing its own shutdown notice) before shutting down.
        private const int ShutdownGraceSeconds = 30;

        private readonly Timer countdownTimer;

        // Wall-clock moment the countdown ends; compared on every tick so the
        // countdown can't drift or stall when ticks are late or the PC sleeps.
        private DateTime shutdownAtUtc;

        // Time the user entered, restored when the countdown is aborted.
        private int originalTotalSeconds;

        // True after the countdown ended and Windows has a shutdown scheduled.
        private bool windowsShutdownPending;

        public Main()
        {
            InitializeComponent();

            if (components == null)
                components = new Container();

            ToolTip toolTip = new ToolTip(components);
            toolTip.SetToolTip(sleep_button, "Starts the local countdown to shut down this PC");
            toolTip.SetToolTip(abort_button, "Cancel the active countdown and reset the timer");

            countdownTimer = new Timer(components);
            countdownTimer.Interval = 250;
            countdownTimer.Tick += CountdownTimer_Tick;

            FormClosing += Main_FormClosing;
            abort_button.Enabled = false;
        }

        private bool RunShutdownCommand(string args)
        {
            try
            {
                using (Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }))
                {
                    if (process == null || !process.WaitForExit(5000))
                        return false;
                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not run the shutdown command:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remaining = shutdownAtUtc - DateTime.UtcNow;

            if (remaining > TimeSpan.Zero)
            {
                ShowTime((int)Math.Ceiling(remaining.TotalSeconds));
                return;
            }

            countdownTimer.Stop();
            ShowTime(0);

            string message = "Sleeptimer: this PC will shut down in " + ShutdownGraceSeconds + " seconds.";
            if (RunShutdownCommand("/s /t " + ShutdownGraceSeconds + " /c \"" + message + "\""))
            {
                windowsShutdownPending = true;
            }
            else
            {
                MessageBox.Show("The shutdown could not be started.", "Shutdown Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowTime(originalTotalSeconds);
                ToggleInputs(true);
            }
        }

        private void ShowTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            hour_updown.Value = (int)time.TotalHours;
            minute_updown.Value = time.Minutes;
            second_updown.Value = time.Seconds;
        }

        private void minute_updown_ValueChanged(object sender, EventArgs e)
        {
            if (minute_updown.Value < 60) return;

            if (hour_updown.Value < hour_updown.Maximum)
            {
                hour_updown.Value++;
                minute_updown.Value = 0;
            }
            else
            {
                minute_updown.Value = 59;
            }
        }

        private void second_updown_ValueChanged(object sender, EventArgs e)
        {
            if (second_updown.Value < 60) return;

            if (hour_updown.Value < hour_updown.Maximum || minute_updown.Value < 59)
            {
                minute_updown.Value++;
                second_updown.Value = 0;
            }
            else
            {
                second_updown.Value = 59;
            }
        }

        private void sleep_button_Click(object sender, EventArgs e)
        {
            int totalSeconds = (int)(hour_updown.Value * 3600 + minute_updown.Value * 60 + second_updown.Value);

            if (totalSeconds <= 0)
            {
                MessageBox.Show("Please set a time greater than 0 seconds.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            originalTotalSeconds = totalSeconds;
            shutdownAtUtc = DateTime.UtcNow.AddSeconds(totalSeconds);

            ToggleInputs(false);
            countdownTimer.Start();
        }

        private void abort_button_Click(object sender, EventArgs e)
        {
            if (countdownTimer.Enabled)
            {
                countdownTimer.Stop();
                ShowTime(originalTotalSeconds);
                ToggleInputs(true);
                MessageBox.Show("Countdown has been aborted and time has been restored.", "Shutdown Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (windowsShutdownPending)
            {
                windowsShutdownPending = false;
                bool aborted = RunShutdownCommand("/a");
                ShowTime(originalTotalSeconds);
                ToggleInputs(true);

                if (aborted)
                    MessageBox.Show("The shutdown has been cancelled and time has been restored.", "Shutdown Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("The shutdown could not be cancelled. Try running 'shutdown /a'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!countdownTimer.Enabled || e.CloseReason != CloseReason.UserClosing)
                return;

            DialogResult result = MessageBox.Show(
                "A countdown is running. Closing the app cancels it and the PC will NOT shut down.\n\nClose anyway?",
                "Countdown Running", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                e.Cancel = true;
        }

        private void ToggleInputs(bool enabled)
        {
            hour_updown.Enabled = enabled;
            minute_updown.Enabled = enabled;
            second_updown.Enabled = enabled;
            sleep_button.Enabled = enabled;
            abort_button.Enabled = !enabled;
        }
    }
}

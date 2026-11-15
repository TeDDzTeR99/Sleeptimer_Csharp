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

        private readonly Timer _countdownTimer;

        // Wall-clock moment the countdown ends; compared on every tick so the
        // countdown can't drift or stall when ticks are late or the PC sleeps.
        private DateTime _shutdownAtUtc;

        // Time the user entered, restored when the countdown is aborted.
        private int _originalTotalSeconds;

        // True after the countdown ended and Windows has a shutdown scheduled.
        private bool _windowsShutdownPending;

        public Main()
        {
            InitializeComponent();

            if (components == null)
                components = new Container();

            ToolTip toolTip = new ToolTip(components);
            toolTip.SetToolTip(sleep_button, "Starts the local countdown to shut down this PC");
            toolTip.SetToolTip(abort_button, "Cancel the active countdown and reset the timer");

            _countdownTimer = new Timer(components);
            _countdownTimer.Interval = 250;
            _countdownTimer.Tick += CountdownTimer_Tick;

            _trayMenu = new ContextMenuStrip(components);
            _trayMenu.Items.Add("Open", null, (s, e) => RestoreFromTray());
            _trayAbortItem = _trayMenu.Items.Add("Abort Sleep", null, (s, e) => abort_button.PerformClick());
            _trayAbortItem.Enabled = false;
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add("Exit", null, (s, e) => { RestoreFromTray(); Close(); });

            _trayIcon = new NotifyIcon(components);
            _trayIcon.Icon = Icon;
            _trayIcon.Text = "Sleeptimer";
            _trayIcon.ContextMenuStrip = _trayMenu;
            _trayIcon.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) RestoreFromTray(); };

            Resize += Main_Resize;
            FormClosing += Main_FormClosing;
            abort_button.Enabled = false;
        }

        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenuStrip _trayMenu;
        private readonly ToolStripItem _trayAbortItem;

        // Minimizing hides the window and shows the tray icon instead.
        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized) return;

            Hide();
            _trayIcon.Visible = true;
        }

        private void RestoreFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            _trayIcon.Visible = false;
        }

        // Tray tooltips are limited to 63 characters.
        private void SetTrayText(string text)
        {
            _trayIcon.Text = text.Length > 63 ? text.Substring(0, 63) : text;
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
            TimeSpan remaining = _shutdownAtUtc - DateTime.UtcNow;

            if (remaining > TimeSpan.Zero)
            {
                int secondsLeft = (int)Math.Ceiling(remaining.TotalSeconds);
                ShowTime(secondsLeft);
                SetTrayText("Sleeptimer - " + FormatTime(secondsLeft) + " until shutdown");
                return;
            }

            _countdownTimer.Stop();
            ShowTime(0);

            string message = "Sleeptimer: this PC will shut down in " + ShutdownGraceSeconds + " seconds.";
            if (RunShutdownCommand("/s /t " + ShutdownGraceSeconds + " /c \"" + message + "\""))
            {
                _windowsShutdownPending = true;
                SetTrayText("Sleeptimer - shutting down");
            }
            else
            {
                MessageBox.Show("The shutdown could not be started.", "Shutdown Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowTime(_originalTotalSeconds);
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

        private static string FormatTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return string.Format("{0:00}:{1:00}:{2:00}", (int)time.TotalHours, time.Minutes, time.Seconds);
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

            _originalTotalSeconds = totalSeconds;
            _shutdownAtUtc = DateTime.UtcNow.AddSeconds(totalSeconds);

            ToggleInputs(false);
            _countdownTimer.Start();
        }

        private void abort_button_Click(object sender, EventArgs e)
        {
            if (_countdownTimer.Enabled)
            {
                _countdownTimer.Stop();
                ShowTime(_originalTotalSeconds);
                ToggleInputs(true);
                MessageBox.Show("Countdown has been aborted and time has been restored.", "Shutdown Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (_windowsShutdownPending)
            {
                _windowsShutdownPending = false;
                bool aborted = RunShutdownCommand("/a");
                ShowTime(_originalTotalSeconds);
                ToggleInputs(true);

                if (aborted)
                    MessageBox.Show("The shutdown has been cancelled and time has been restored.", "Shutdown Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("The shutdown could not be cancelled. Try running 'shutdown /a'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_countdownTimer.Enabled || e.CloseReason != CloseReason.UserClosing)
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
            _trayAbortItem.Enabled = !enabled;

            if (enabled)
                SetTrayText("Sleeptimer");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ranshen_s_Flash
{
    public partial class Form1 : Form
    {
        private Button btnFlash;
        private ComboBox comboBoxScripts;
        private TextBox textBoxOutput;
        private ProgressBar progressBar;
        private Button btnInstallDrivers;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupFormProperties();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void SetupFormProperties()
        {
            this.Text = "Ranshen's Flash";

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            int width = screen.Width * 26 / 100;
            int height = screen.Height * 30 / 100;
            this.ClientSize = new Size(width, height);
            // 禁止最大化窗口
            this.MaximizeBox = false;
            // 禁止调整窗口大小
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            this.Icon = new Icon("bin/resources/android_FILL0.ico");

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeCustomComponents()
        {
            // 调整控件位置和大小
            // 控件宽度为窗口宽度减20px
            int controlWidth = this.ClientSize.Width - 20;

            // 初始化下拉列表
            comboBoxScripts = new ComboBox();
            comboBoxScripts.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxScripts.Items.Add("flash_all");
            comboBoxScripts.Items.Add("flash_all_except_storage");
            comboBoxScripts.Location = new Point(10, 10);
            // 1/3宽度减去间隙
            comboBoxScripts.Width = controlWidth / 3 - 10;
            Controls.Add(comboBoxScripts);

            // 调整按钮位置
            btnInstallDrivers = new Button();
            btnInstallDrivers.Text = "安装驱动";
            btnInstallDrivers.Location = new Point(comboBoxScripts.Right + 10, 10);
            // 1/8宽度减去间隙
            btnInstallDrivers.Width = (controlWidth / 8) - 5; // 
            btnInstallDrivers.Click += new EventHandler(BtnInstallDrivers_Click);
            Controls.Add(btnInstallDrivers);

            btnFlash = new Button();
            btnFlash.Text = "开始刷机";
            btnFlash.Location = new Point(btnInstallDrivers.Right + 10, 10);
            // 1/8宽度减去间隙
            btnFlash.Width = (controlWidth / 8) - 5;
            btnFlash.Click += new EventHandler(BtnFlash_Click);
            Controls.Add(btnFlash);

            textBoxOutput = new TextBox();
            textBoxOutput.Multiline = true;
            textBoxOutput.ScrollBars = ScrollBars.Vertical;
            // 距离顶部50px开始
            textBoxOutput.Location = new Point(10, 50);
            // 设置宽度为窗口宽度的60%
            textBoxOutput.Width = (int)(this.ClientSize.Width * 0.6);
            // 设置高度为窗口高度的50%
            textBoxOutput.Height = (int)(this.ClientSize.Height * 0.5);
            // 设置文本框为只读，用户不能编辑
            textBoxOutput.ReadOnly = true;
            Controls.Add(textBoxOutput);

            textBoxOutput.Text = "项目开源地址：https://github.com/Ranshen1209/Ranshen-s-Flash\n";

            progressBar = new ProgressBar();
            // 进度条位置位于文本框下方，留10px间隙
            progressBar.Location = new Point(10, textBoxOutput.Bottom + 10);
            progressBar.Width = (int)(this.ClientSize.Width * 0.6);
            Controls.Add(progressBar);
        }

        private async void BtnFlash_Click(object sender, EventArgs e)
        {
            if (comboBoxScripts.SelectedItem == null)
            {
                MessageBox.Show("Please select a script to flash.");
                return;
            }

            string selectedScript = comboBoxScripts.SelectedItem.ToString();
            string batFileName = selectedScript + ".bat";
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string batPath = System.IO.Path.Combine(exePath, batFileName);

            // 读取BAT文件行数
            int lineCount = File.ReadAllLines(batPath).Length;
            progressBar.Maximum = lineCount;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Continuous;
            // 默认为绿色
            progressBar.ForeColor = Color.Green;

            // 当前执行的行数
            int currentLine = 0;

            await Task.Run(() =>
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c \"{batPath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Verb = "runas";

                try
                {
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox(line);
                            // 更新进度条
                            progressBar.Value = Math.Min(++currentLine, progressBar.Maximum);
                        }));
                    }
                    // 等待进程结束
                    process.WaitForExit();

                    this.Invoke(new Action(() =>
                    {
                        if (process.ExitCode == 0)
                        {
                            // 成功完成
                            progressBar.Value = progressBar.Maximum;
                        }
                        else
                        {
                            progressBar.Value = progressBar.Maximum;
                            // 出现错误，显示红色
                            progressBar.ForeColor = Color.Red;
                        }
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Failed to flash device: {ex.Message}");
                        // 出现错误，显示红色
                        progressBar.ForeColor = Color.Red;
                    }));
                }
            });
        }




        private void ExecuteScript(string scriptName)
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string scriptPath = System.IO.Path.Combine(exePath, scriptName);

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c \"{scriptPath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                UpdateUI(e.Data);
            }
        }

        private void UpdateUI(string text)
        {
            if (textBoxOutput.InvokeRequired)
            {
                textBoxOutput.Invoke(new Action<string>(UpdateUI), new object[] { text });
            }
            else
            {
                textBoxOutput.AppendText(text + Environment.NewLine);
            }
        }

        private async void BtnInstallDrivers_Click(object sender, EventArgs e)
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string batPath = System.IO.Path.Combine(exePath, "bin\\resources\\install_drivers.bat");

            // 读取BAT文件行数
            int lineCount = File.ReadAllLines(batPath).Length;
            progressBar.Maximum = lineCount;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Continuous;
            // 默认为绿色
            progressBar.ForeColor = Color.Green;

            // 当前执行的行数
            int currentLine = 0;

            await Task.Run(() =>
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c \"{batPath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Verb = "runas";

                try
                {
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox(line);
                            // 更新进度条
                            progressBar.Value = Math.Min(++currentLine, progressBar.Maximum);
                        }));
                    }
                    process.WaitForExit();

                    // 根据进程退出代码更新进度条
                    this.Invoke(new Action(() =>
                    {
                        if (process.ExitCode == 0)
                        {
                            // 成功完成
                            progressBar.Value = progressBar.Maximum;
                        }
                        else
                        {
                            progressBar.Value = progressBar.Maximum;
                            // 出现错误，显示红色
                            progressBar.ForeColor = Color.Red;
                        }
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Failed to install drivers: {ex.Message}");
                        // 出现错误，显示红色
                        progressBar.ForeColor = Color.Red;
                    }));
                }
            });
        }


        // 更新文本框
        private void UpdateTextBox(string text)
        {
            textBoxOutput.AppendText(text + Environment.NewLine);
        }

    }
}

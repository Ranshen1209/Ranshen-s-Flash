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
        private bool isOperationInProgress = false;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            AdjustFormSizeBasedOnDPI();
            SetupFormProperties();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void AdjustFormSizeBasedOnDPI()
        {
            // 设置窗口在屏幕中心打开
            this.StartPosition = FormStartPosition.CenterScreen;

            // 创建 Graphics 对象以获取当前 DPI 设置
            Graphics graphics = this.CreateGraphics();
            float dpiX = graphics.DpiX;
            float dpiY = graphics.DpiY;

            // 默认屏幕DPI为96
            float scale = dpiX / 96;

            // 根据DPI调整窗口大小
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            int baseWidth = screen.Width * 26 / 100;  // 设计时的基准宽度
            int baseHeight = screen.Height * 30 / 100; // 设计时的基准高度
            this.ClientSize = new Size((int)(baseWidth * scale), (int)(baseHeight * scale));

            // 确保释放 Graphics 资源
            graphics.Dispose();
        }

        private void SetupFormProperties()
        {
            this.Text = "Ranshen's Flash";
            this.MaximizeBox = false;
            this.Icon = new Icon("bin/resources/android_FILL0.ico");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isOperationInProgress)
            {
                MessageBox.Show("Operation in progress. Please wait until it's completed before closing the application.");
                e.Cancel = true;
            }
        }

        private void InitializeCustomComponents()
        {
            // 调整控件位置和大小
            // 控件宽度为窗口宽度减20px
            int controlWidth = this.ClientSize.Width - 20;
            int controlHeight = this.ClientSize.Height;

            // 初始化下拉列表
            comboBoxScripts = new ComboBox();
            comboBoxScripts.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxScripts.Items.Add("flash_all");
            comboBoxScripts.Items.Add("flash_all_except_storage");
            comboBoxScripts.Location = new Point(10, 15);
            // 1/3宽度减去间隙
            comboBoxScripts.Width = (controlWidth / 3) - 10;
            Controls.Add(comboBoxScripts);

            // 调整按钮位置
            btnInstallDrivers = new Button();
            btnInstallDrivers.Text = "安装驱动";
            btnInstallDrivers.Location = new Point(comboBoxScripts.Right + 10, 10);
            // 1/8宽度减去间隙
            btnInstallDrivers.Width = (controlWidth / 8);
            btnInstallDrivers.Height = (controlHeight / 15);
            btnInstallDrivers.Click += new EventHandler(BtnInstallDrivers_Click);
            Controls.Add(btnInstallDrivers);

            btnFlash = new Button();
            btnFlash.Text = "开始刷机";
            btnFlash.Location = new Point(btnInstallDrivers.Right + 10, 10);
            // 1/8宽度减去间隙
            btnFlash.Width = (controlWidth / 8);
            btnFlash.Height = (controlHeight / 15);
            btnFlash.Click += new EventHandler(BtnFlash_Click);
            Controls.Add(btnFlash);

            textBoxOutput = new TextBox();
            textBoxOutput.Multiline = true;
            textBoxOutput.ScrollBars = ScrollBars.Vertical;
            // 距离顶部50px开始
            textBoxOutput.Location = new Point(10, 60);
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

            if (!File.Exists(batPath))
            {
                MessageBox.Show("The selected script file does not exist. Please verify the files and try again.");
                return;
            }

            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Marquee;

            isOperationInProgress = true;
            btnFlash.Enabled = false;
            btnInstallDrivers.Enabled = false;

            await Task.Run(() =>
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{batPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                        }));
                    }
                };

                process.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox("Error: " + args.Data);
                        }));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                process.CancelOutputRead();
                process.CancelErrorRead();

                this.Invoke(new Action(() =>
                {
                    if (process.ExitCode == 0)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = progressBar.Maximum;
                    }
                    else
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        progressBar.ForeColor = Color.Red;
                    }
                    btnFlash.Enabled = true;
                    btnInstallDrivers.Enabled = true;
                    isOperationInProgress = false;
                }));
            });
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

            int lineCount = File.ReadAllLines(batPath).Length;
            progressBar.Maximum = lineCount;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.ForeColor = Color.Green;

            isOperationInProgress = true;
            btnFlash.Enabled = false;
            btnInstallDrivers.Enabled = false;

            await Task.Run(() =>
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{batPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        Verb = "runas"
                    }
                };

                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                            progressBar.Value = Math.Min(++lineCount, progressBar.Maximum);
                        }));
                    }
                };

                process.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox("Error: " + args.Data);
                        }));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                process.CancelOutputRead();
                process.CancelErrorRead();

                this.Invoke(new Action(() =>
                {
                    if (process.ExitCode == 0)
                    {
                        progressBar.Value = progressBar.Maximum;
                    }
                    else
                    {
                        progressBar.Value = progressBar.Maximum;
                        progressBar.ForeColor = Color.Red;
                    }
                    btnFlash.Enabled = true;
                    btnInstallDrivers.Enabled = true;
                    isOperationInProgress = false;
                }));
            });
        }



        // 更新文本框
        private void UpdateTextBox(string text)
        {
            textBoxOutput.AppendText(text + Environment.NewLine);
        }

    }
}
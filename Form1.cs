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
        private Process currentProcess = null;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            AdjustFormSizeBasedOnDPI();
            SetupFormProperties();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.Resize += new EventHandler(Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AdjustControlSizesAndPositions(); // 在窗口加载时调整控件大小和位置
        }

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
            this.Icon = new Icon("flashbin/resources/android_FILL0.ico");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isOperationInProgress)
            {
                MessageBox.Show("Operation in progress. Please wait until it's completed before closing the application.");
                e.Cancel = true;
            }
            else if (currentProcess != null && !currentProcess.HasExited)
            {
                currentProcess.Kill();
            }
        }

        private void InitializeCustomComponents()
        {
            // 初始化下拉列表
            comboBoxScripts = new ComboBox();
            comboBoxScripts.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxScripts.Items.Add("flash_all");
            comboBoxScripts.Items.Add("flash_all_except_storage");
            comboBoxScripts.Location = new Point(10, 15);
            Controls.Add(comboBoxScripts);

            // 初始化按钮
            btnInstallDrivers = new Button();
            btnInstallDrivers.Text = "安装驱动";
            btnInstallDrivers.Location = new Point(comboBoxScripts.Right + 10, 10);
            Controls.Add(btnInstallDrivers);

            btnFlash = new Button();
            btnFlash.Text = "开始刷机";
            btnFlash.Location = new Point(btnInstallDrivers.Right + 10, 10);
            Controls.Add(btnFlash);

            // 初始化文本框
            textBoxOutput = new TextBox();
            textBoxOutput.Multiline = true;
            textBoxOutput.ScrollBars = ScrollBars.Vertical;
            textBoxOutput.Location = new Point(10, 60);
            textBoxOutput.ReadOnly = true;
            Controls.Add(textBoxOutput);

            textBoxOutput.Text = "项目开源地址：https://github.com/Ranshen1209/Ranshen-s-Flash\n";

            // 初始化进度条
            progressBar = new ProgressBar();
            Controls.Add(progressBar);

            // 设置控件的 Anchor 属性
            comboBoxScripts.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnInstallDrivers.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnFlash.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            textBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustControlSizesAndPositions();
        }

        private void AdjustControlSizesAndPositions()
        {
            int margin = 10;
            int spacing = 10;

            // 调整控件位置和大小
            comboBoxScripts.Width = (this.ClientSize.Width / 3) - (2 * margin);
            comboBoxScripts.Location = new Point(margin, margin);

            btnInstallDrivers.Width = (this.ClientSize.Width / 4);
            btnInstallDrivers.Height = comboBoxScripts.Height;
            btnInstallDrivers.Location = new Point(comboBoxScripts.Right + spacing, margin);

            btnFlash.Width = (this.ClientSize.Width / 4);
            btnFlash.Height = comboBoxScripts.Height;
            btnFlash.Location = new Point(btnInstallDrivers.Right + spacing, margin);

            textBoxOutput.Width = this.ClientSize.Width - (2 * margin);
            textBoxOutput.Height = this.ClientSize.Height - comboBoxScripts.Height - progressBar.Height - (4 * margin);
            textBoxOutput.Location = new Point(margin, comboBoxScripts.Bottom + spacing);

            progressBar.Width = this.ClientSize.Width - (2 * margin);
            progressBar.Location = new Point(margin, textBoxOutput.Bottom + spacing);
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
                currentProcess = new Process
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

                currentProcess.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                        }));
                    }
                };

                currentProcess.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox("Error: " + args.Data);
                        }));
                    }
                };

                currentProcess.Start();
                currentProcess.BeginOutputReadLine();
                currentProcess.BeginErrorReadLine();
                currentProcess.WaitForExit();
                currentProcess.CancelOutputRead();
                currentProcess.CancelErrorRead();

                this.Invoke(new Action(() =>
                {
                    if (currentProcess.ExitCode == 0)
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
                    currentProcess = null;
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
            string batPath = System.IO.Path.Combine(exePath, "flashbin\\resources\\install_drivers.bat");

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
                currentProcess = new Process
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

                currentProcess.OutputDataReceived += (s, args) =>
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

                currentProcess.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        this.Invoke(new Action(() =>
                        {
                            UpdateTextBox("Error: " + args.Data);
                        }));
                    }
                };

                currentProcess.Start();
                currentProcess.BeginOutputReadLine();
                currentProcess.BeginErrorReadLine();
                currentProcess.WaitForExit();
                currentProcess.CancelOutputRead();
                currentProcess.CancelErrorRead();

                this.Invoke(new Action(() =>
                {
                    if (currentProcess.ExitCode == 0)
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
                    currentProcess = null;
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

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ranshen_s_Flash
{
    public partial class Form1 : Form
    {
        private Button _btnFlash;
        private ComboBox _comboBoxScripts;
        private TextBox _textBoxOutput;
        private ProgressBar _progressBar;
        private Button _btnInstallDrivers;
        private bool _isOperationInProgress;
        private Process _currentProcess;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            AdjustFormSizeBasedOnDPI();
            SetupFormProperties();
            FormClosing += (Form1_FormClosing);
            Resize += (Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AdjustControlSizesAndPositions();
        }

        private void AdjustFormSizeBasedOnDPI()
        {
            StartPosition = FormStartPosition.CenterScreen;
            
            Graphics graphics = CreateGraphics();
            float dpiX = graphics.DpiX;
            float scale = dpiX / 96;

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            int baseWidth = screen.Width * 26 / 100;
            int baseHeight = screen.Height * 30 / 100;
            ClientSize = new Size((int)(baseWidth * scale), (int)(baseHeight * scale));

            graphics.Dispose();
        }

        private void SetupFormProperties()
        {
            Text = Properties.Resources.AppTitle;
            MaximizeBox = false;
            Icon = new Icon("flashbin/resources/android_FILL0.ico");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isOperationInProgress)
            {
                MessageBox.Show(Properties.Resources.OperationInProgressMessage);
                e.Cancel = true;
            }
            else if (_currentProcess != null && !_currentProcess.HasExited)
            {
                _currentProcess.Kill();
            }
        }

        private void InitializeCustomComponents()
        {
            _comboBoxScripts = new ComboBox();
            _comboBoxScripts.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBoxScripts.Items.Add("flash_all");
            _comboBoxScripts.Items.Add("flash_all_except_storage");
            _comboBoxScripts.Location = new Point(10, 15);
            Controls.Add(_comboBoxScripts);

            _btnInstallDrivers = new Button();
            _btnInstallDrivers.Text = Properties.Resources.InstallDriver;
            _btnInstallDrivers.Location = new Point(_comboBoxScripts.Right + 10, 10);
            _btnInstallDrivers.Click += BtnInstallDrivers_Click;
            Controls.Add(_btnInstallDrivers);

            _btnFlash = new Button();
            _btnFlash.Text = Properties.Resources.StartFlashing;
            _btnFlash.Location = new Point(_btnInstallDrivers.Right + 10, 10);
            _btnFlash.Click += BtnFlash_Click;
            Controls.Add(_btnFlash);

            _textBoxOutput = new TextBox();
            _textBoxOutput.Multiline = true;
            _textBoxOutput.ScrollBars = ScrollBars.Vertical;
            _textBoxOutput.Location = new Point(10, 60);
            _textBoxOutput.ReadOnly = true;
            _textBoxOutput.Size = new Size(400, 300);
            Controls.Add(_textBoxOutput);

            string url = "https://github.com/Ranshen1209/Ranshen-s-Flash";
            _textBoxOutput.Text = string.Format(Properties.Resources.ProjectOpenSourceAddress, url) + Environment.NewLine;

            _progressBar = new ProgressBar();
            Controls.Add(_progressBar);

            _comboBoxScripts.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _btnInstallDrivers.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _btnFlash.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            _textBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustControlSizesAndPositions();
        }

        private void AdjustControlSizesAndPositions()
        {
            int margin = 10;
            int spacing = 10;

            _comboBoxScripts.Width = (ClientSize.Width / 3) - (2 * margin);
            _comboBoxScripts.Location = new Point(margin, margin);

            _btnInstallDrivers.Width = (ClientSize.Width / 4);
            _btnInstallDrivers.Height = _comboBoxScripts.Height;
            _btnInstallDrivers.Location = new Point(_comboBoxScripts.Right + spacing, margin);

            _btnFlash.Width = (ClientSize.Width / 4);
            _btnFlash.Height = _comboBoxScripts.Height;
            _btnFlash.Location = new Point(_btnInstallDrivers.Right + spacing, margin);

            _textBoxOutput.Width = ClientSize.Width - (2 * margin);
            _textBoxOutput.Height = ClientSize.Height - _comboBoxScripts.Height - _progressBar.Height - (4 * margin);
            _textBoxOutput.Location = new Point(margin, _comboBoxScripts.Bottom + spacing);

            _progressBar.Width = ClientSize.Width - (2 * margin);
            _progressBar.Location = new Point(margin, _textBoxOutput.Bottom + spacing);
        }

        private async void BtnFlash_Click(object sender, EventArgs e)
        {
            if (_comboBoxScripts.SelectedItem == null)
            {
                MessageBox.Show(Properties.Resources.SelectScriptMessage);
                return;
            }

            string selectedScript = _comboBoxScripts.SelectedItem.ToString();
            string batFileName = selectedScript + ".bat";
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string batPath = Path.Combine(exePath, batFileName);

            if (!File.Exists(batPath))
            {
                MessageBox.Show(Properties.Resources.ScriptFileNotFoundMessage);
                return;
            }

            _progressBar.Value = 0;
            _progressBar.Style = ProgressBarStyle.Marquee;

            _isOperationInProgress = true;
            _btnFlash.Enabled = false;
            _btnInstallDrivers.Enabled = false;

            await Task.Run(() =>
            {
                _currentProcess = new Process
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

                _currentProcess.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                        }));
                    }
                };

                _currentProcess.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                        }));
                    }
                };

                _currentProcess.Start();
                _currentProcess.BeginOutputReadLine();
                _currentProcess.BeginErrorReadLine();
                _currentProcess.WaitForExit();
                _currentProcess.CancelOutputRead();
                _currentProcess.CancelErrorRead();

                Invoke(new Action(() =>
                {
                    if (_currentProcess.ExitCode == 0)
                    {
                        _progressBar.Style = ProgressBarStyle.Continuous;
                        _progressBar.Value = _progressBar.Maximum;
                    }
                    else
                    {
                        _progressBar.Style = ProgressBarStyle.Continuous;
                        _progressBar.Value = 0;
                        _progressBar.ForeColor = Color.Red;
                    }
                    _btnFlash.Enabled = true;
                    _btnInstallDrivers.Enabled = true;
                    _isOperationInProgress = false;
                    _currentProcess = null;
                }));
            });
        }

        private async void BtnInstallDrivers_Click(object sender, EventArgs e)
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string batPath = Path.Combine(exePath, "flashbin\\resources\\install_drivers.bat");

            int lineCount = File.ReadAllLines(batPath).Length;
            _progressBar.Maximum = lineCount;
            _progressBar.Value = 0;
            _progressBar.Style = ProgressBarStyle.Continuous;
            _progressBar.ForeColor = Color.Green;

            _isOperationInProgress = true;
            _btnFlash.Enabled = false;
            _btnInstallDrivers.Enabled = false;

            await Task.Run(() =>
            {
                _currentProcess = new Process
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

                _currentProcess.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                            _progressBar.Value = Math.Min(++lineCount, _progressBar.Maximum);
                        }));
                    }
                };

                _currentProcess.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateTextBox(args.Data);
                        }));
                    }
                };

                _currentProcess.Start();
                _currentProcess.BeginOutputReadLine();
                _currentProcess.BeginErrorReadLine();
                _currentProcess.WaitForExit();
                _currentProcess.CancelOutputRead();
                _currentProcess.CancelErrorRead();

                Invoke(new Action(() =>
                {
                    if (_currentProcess.ExitCode == 0)
                    {
                        _progressBar.Value = _progressBar.Maximum;
                    }
                    else
                    {
                        _progressBar.Value = _progressBar.Maximum;
                        _progressBar.ForeColor = Color.Red;
                    }
                    _btnFlash.Enabled = true;
                    _btnInstallDrivers.Enabled = true;
                    _isOperationInProgress = false;
                    _currentProcess = null;
                }));
            });
        }

        private void UpdateTextBox(string text)
        {
            _textBoxOutput.AppendText(text + Environment.NewLine);
        }
    }
}

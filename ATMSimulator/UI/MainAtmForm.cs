using System;
using System.Drawing;
using System.Windows.Forms;
using ATMSimulator.Services;

namespace ATMSimulator
{
    public class MainAtmForm : Form
    {
        private readonly AtmService _atmService;
        private readonly Form _loginForm;

        private Label lblMonitor;
        private TextBox txtAmount;
        private Button btnWithdraw;
        private Button btnDeposit;
        private Button btnExit;
        private ListBox lstLog;

        public MainAtmForm(AtmService atmService, Form loginForm)
        {
            _atmService = atmService;
            _loginForm = loginForm;
            InitializeComponent();

            _atmService.OnScreenUpdated += state => UpdateMonitor();
            _atmService.OnMessageDisplayed += msg => LogAction(msg);

            UpdateMonitor();
        }

        private void InitializeComponent()
        {
            this.Text = "Банкомат - Головна панель";
            this.Size = new Size(700, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);

            lblMonitor = new Label
            {
                Location = new Point(30, 20),
                Size = new Size(620, 160),
                BackColor = Color.FromArgb(20, 25, 20),
                ForeColor = Color.LimeGreen,
                Font = new Font("Consolas", 13, FontStyle.Bold),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblAmount = new Label { Text = "Сума (грн):", ForeColor = Color.White, Location = new Point(30, 205), Size = new Size(250, 20) };
            txtAmount = new TextBox { Location = new Point(30, 230), Size = new Size(220, 25), Font = new Font("Segoe UI", 12) };

            btnWithdraw = CreateStyledButton("Зняти готівку", new Point(280, 225), Color.FromArgb(180, 40, 40));
            btnWithdraw.Click += (s, e) => HandleFinanceAction(isWithdraw: true);

            btnDeposit = CreateStyledButton("Поповнити рахунок", new Point(470, 225), Color.FromArgb(40, 120, 40));
            btnDeposit.Click += (s, e) => HandleFinanceAction(isWithdraw: false);

            btnExit = CreateStyledButton("ВИХІД", new Point(280, 285), Color.FromArgb(90, 20, 20), new Size(370, 40));
            btnExit.Click += BtnExit_Click;

            var lblLogTitle = new Label { Text = "Журнал операцій:", ForeColor = Color.DarkGray, Location = new Point(30, 360), Size = new Size(200, 20) };
            lstLog = new ListBox
            {
                Location = new Point(30, 385),
                Size = new Size(620, 100),
                BackColor = Color.FromArgb(15, 15, 15),
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 9)
            };

            this.Controls.AddRange(new Control[] { lblMonitor, lblAmount, txtAmount, btnWithdraw, btnDeposit, btnExit, lblLogTitle, lstLog });
        }

        private Button CreateStyledButton(string text, Point location, Color bg, Size? size = null)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = size ?? new Size(170, 40),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void UpdateMonitor()
        {
            if (_atmService.CurrentUser != null)
            {
                lblMonitor.Text = $"Банкомат\n\n" +
                                  $"Клієнт: {_atmService.CurrentUser.GetFullName()}\n" +
                                  $"Рахунок: {_atmService.CurrentUser.UserAccount.AccountNumber}\n" +
                                  $"Баланс: {_atmService.CurrentUser.UserAccount.Balance} UAH";
            }
        }

        private void HandleFinanceAction(bool isWithdraw)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                if (isWithdraw)
                    _atmService.Withdraw(amount);
                else
                    _atmService.Deposit(amount);

                txtAmount.Clear();
                UpdateMonitor();
            }
        }

        private void LogAction(string message)
        {
            lstLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            _atmService.EjectCard();
            _loginForm.Show();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _atmService.CurrentUser != null)
            {
                _atmService.EjectCard();
                _loginForm.Show();
            }
            base.OnFormClosing(e);
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using ATMSimulator.Services;
using ATMSimulator.States;
using ATMSimulator.Data;

namespace ATMSimulator
{
    public class LoginForm : Form
    {
        private readonly AtmService _atmService;
        private TextBox txtCardNumber;
        private TextBox txtPinCode;
        private Button btnLogin;
        private Label lblStatus;

        public LoginForm()
        {
            _atmService = new AtmService(JsonDataStorage.Instance);
            InitializeComponent();
            _atmService.OnMessageDisplayed += msg => lblStatus.Text = msg;
        }

        private void InitializeComponent()
        {
            this.Text = "ATM - Login";
            this.Size = new Size(400, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            var lblTitle = new Label
            {
                Text = "INSERT CARD",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.LightGray,
                Location = new Point(20, 20),
                Size = new Size(360, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblCard = new Label { Text = "Card Number:", ForeColor = Color.DarkGray, Location = new Point(50, 70), Size = new Size(300, 20) };
            txtCardNumber = new TextBox { Location = new Point(50, 90), Size = new Size(280, 25), Font = new Font("Segoe UI", 11), Text = "5555666677778888" };

            var lblPin = new Label { Text = "PIN:", ForeColor = Color.DarkGray, Location = new Point(50, 130), Size = new Size(300, 20) };
            txtPinCode = new TextBox { Location = new Point(50, 150), Size = new Size(280, 25), Font = new Font("Segoe UI", 11), PasswordChar = '*', Text = "4321" };

            btnLogin = new Button
            {
                Text = "ENTER",
                Location = new Point(50, 200),
                Size = new Size(280, 40),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            lblStatus = new Label
            {
                Location = new Point(20, 255),
                Size = new Size(360, 40),
                ForeColor = Color.OrangeRed,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                TextAlign = ContentAlignment.TopCenter
            };

            this.Controls.AddRange(new Control[] { lblTitle, lblCard, txtCardNumber, lblPin, txtPinCode, btnLogin, lblStatus });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            _atmService.InsertCard(txtCardNumber.Text);
            _atmService.EnterPin(txtPinCode.Text);

            if (_atmService.CurrentState is AuthorizedState)
            {
                txtPinCode.Clear();
                var mainAtmForm = new MainAtmForm(_atmService, this);
                mainAtmForm.Show();
                this.Hide();
            }
        }
    }
}
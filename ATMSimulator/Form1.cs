using System;
using System.Drawing;
using System.Windows.Forms;
using ATMSimulator.Services;

namespace ATMSimulator
{
    public partial class Form1 : Form
    {
        private readonly AtmService _atmService;

        // Елементи інтерфейсу банкомата
        private Label lblAtmScreen;
        private TextBox txtCardInput;
        private TextBox txtPinAmountInput;
        private Button btnInsertCard;
        private Button btnEnterPin;
        private Button btnWithdraw;
        private Button btnDeposit;
        private Button btnEjectCard;
        private ListBox lstAtmNotifications;

        public Form1()
        {

            _atmService = new AtmService(JsonDataStorage.Instance);

            InitializeAtmComponents();

            _atmService.OnScreenUpdated += UpdateAtmScreenText;
            _atmService.OnMessageDisplayed += ShowAtmNotification;

            _atmService.UpdateScreen();
        }

        private void InitializeAtmComponents()
        {
            this.Text = "ATM Simulator - Фінальний Проєкт";
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48); 

            lblAtmScreen = new Label
            {
                Location = new Point(30, 20),
                Size = new Size(720, 150),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.LimeGreen,
                Font = new Font("Consolas", 14, FontStyle.Bold),
                BorderStyle = BorderStyle.Fixed3D,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblCard = new Label { Text = "Номер картки:", Location = new Point(30, 190), ForeColor = Color.White, Size = new Size(200, 20) };
            txtCardInput = new TextBox { Location = new Point(30, 215), Size = new Size(250, 25), Font = new Font("Segoe UI", 11) };

            var lblPinAmount = new Label { Text = "ПІН-код:", Location = new Point(30, 260), ForeColor = Color.White, Size = new Size(200, 20) };
            txtPinAmountInput = new TextBox { Location = new Point(30, 285), Size = new Size(250, 25), Font = new Font("Segoe UI", 11) };

            btnInsertCard = CreateStyledButton("Вставити Картку", new Point(310, 210), Color.Teal);
            btnInsertCard.Click += (s, e) => _atmService.InsertCard(txtCardInput.Text);

            btnEnterPin = CreateStyledButton("Підтвердити ПІН", new Point(310, 280), Color.DarkGreen);
            btnEnterPin.Click += (s, e) => _atmService.EnterPin(txtPinAmountInput.Text);

            btnWithdraw = CreateStyledButton("Зняти кошти", new Point(540, 210), Color.Brown);
            btnWithdraw.Click += (s, e) => HandleFinanceAction(isWithdraw: true);

            btnDeposit = CreateStyledButton("Поповнити рахунок", new Point(540, 280), Color.DarkOliveGreen);
            btnDeposit.Click += (s, e) => HandleFinanceAction(isWithdraw: false);

            btnEjectCard = CreateStyledButton("Вихід", new Point(310, 350), Color.DarkRed, new Size(410, 40));
            btnEjectCard.Click += (s, e) => {
                _atmService.EjectCard();
                txtCardInput.Clear();
                txtPinAmountInput.Clear();
            };

            var lblLogs = new Label { Text = "Журнал операцій банкомата:", Location = new Point(30, 410), ForeColor = Color.White, Size = new Size(200, 20) };
            lstAtmNotifications = new ListBox
            {
                Location = new Point(30, 430),
                Size = new Size(720, 100),
                BackColor = Color.FromArgb(20, 20, 20),
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 10)
            };

            this.Controls.AddRange(new Control[] { 
                lblAtmScreen, lblCard, txtCardInput, lblPinAmount, txtPinAmountInput,
                btnInsertCard, btnEnterPin, btnWithdraw, btnDeposit, btnEjectCard,
                lblLogs, lstAtmNotifications 
            });
        }

        private Button CreateStyledButton(string text, Point location, Color baseColor, Size? customSize = null)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = customSize ?? new Size(180, 40),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void HandleFinanceAction(bool isWithdraw)
        {
            if (decimal.TryParse(txtPinAmountInput.Text, out decimal amount))
            {
                if (isWithdraw)
                    _atmService.Withdraw(amount);
                else
                    _atmService.Deposit(amount);
                    
                txtPinAmountInput.Clear();
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректну числову суму.", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateAtmScreenText(string stateName)
        {
            if (_atmService.CurrentUser != null)
            {
                lblAtmScreen.Text = $"[ Авторизовано ]\n\n" +
                                   $"Клієнт: {_atmService.CurrentUser.GetFullName()}\n" +
                                   $"Рахунок: {_atmService.CurrentUser.UserAccount.AccountNumber}\n" +
                                   $"Поточний баланс: {_atmService.CurrentUser.UserAccount.Balance} UAH";
            }
            else
            {
                lblAtmScreen.Text = $"[ Стан системи: {stateName.ToUpper()} ]\n\n" +
                                   $"Вітаємо в нашому банку!\n" +
                                   $"Будь ласка, вставте Вашу картку для початку роботи.";
            }
        }

        private void ShowAtmNotification(string message)
        {
            string timePrefix = $"[{DateTime.Now:HH:mm:ss}] ";
            lstAtmNotifications.Items.Insert(0, timePrefix + message);
        }
    }
}
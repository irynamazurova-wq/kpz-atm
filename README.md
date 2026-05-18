ATMSimulator
Застосунок імітує роботу банкомата з візуальним інтерфейсом. Написаний на C# з використанням Windows Forms. Дані користувачів та лог транзакцій зберігаються локально у файлах.

Можливості
Окреме компактне вікно для входу та повноцінна головна панель операцій банкомата;

Авторизація користувачів за номером картки та пін-кодом із обмеженням до 3 спроб введення та автоматичним блокуванням;

Одночасне відстеження та відображення балансу в UAH, USD та EUR;

Купівля валюти прямо на екрані банкомата за фіксованим актуальним курсом;

Переказ коштів на будь-які інші картки системи за їхнім номером у реальному часі;

Автоматичне локальне збереження стану та балансів користувачів у файлі формату JSON;

Логування спроб та результатів усіх фінансових операцій з часовими мітками у файл transactions_log.txt

Запуск
Bash
git clone https://github.com/irynamazurova-wq/KPZ_ATM.git
cd KPZ_ATM
Відкрити файл рішення KPZ-ATM.sln або файл проєкту через Visual Studio та натиснути клавішу F5 (або кнопку Start). Файли локальних баз даних users.json та transactions_log.txt будуть автоматично створені у папці виконання бінарного файлу (bin/Debug/net10.0-windows/).

Структура проєкту
Plaintext
ATMSimulator/
├── Form1.cs                          # Вікно авторизації користувача 
├── Program.cs                        # Точка входу в систему
├── Data/
│   ├── IDataStorage.cs               # Інтерфейс файлового сховища
│   └── JsonDataStorage.cs            # Реалізація бази даних у форматі JSON
├── Factories/
│   └── UserFactory.cs                # Фабрика для створення об'єктів доменних моделей
├── Interfaces/
│   └── ITransactionProcessor.cs      # Інтерфейс процесора фінансових операцій
├── Models/
│   ├── Account.cs                    # Модель банківського мультивалютного рахунку
│   ├── Card.cs                       # Модель платіжної картки
│   └── User.cs                       # Модель клієнта банку
├── Services/
│   ├── AtmService.cs                 # Головний керуючий сервіс логіки банкомата
│   ├── RealTransactionProcessor.cs   # Реальний процесор зняття, поповнення, переказу
│   └── TransactionLoggerProxy.cs     # Проксі-сервіс для автоматичного логування дій
├── States/
│   ├── IAtmState.cs                  # Спільний інтерфейс станів заліза банкомата
│   ├── NoCardState.cs                # Стан: Картку не вставлено
│   ├── WaitingForPinState.cs         # Стан: Очікування та валідація ПІН-коду
│   └── AuthorizedState.cs            # Стан: Успішна авторизація, доступ до операцій
├── Strategies/
│   ├── IWithdrawStrategy.cs          # Інтерфейс стратегій розрахунку комісії
│   ├── RegularWithdrawStrategy.cs    # Стратегія: Стандартна комісія 1% для Regular
│   └── VipWithdrawStrategy.cs        # Стратегія: Нульова комісія для VIP-клієнтів
└── UI/
    └── MainAtmForm.cs                # Головне графічне вікно панелі банкомата
Programming Principles
SRP (Single Responsibility Principle) — Кожен клас відповідає за одну ізольовану річ. JsonDataStorage — тільки збереження у файл. Account — тільки збереження стану грошей рахунку. LoginForm — тільки зчитування початкових полів інтерфейсу.

OCP (Open/Closed Principle) — Система відкрита для розширення, але закрита для модифікацій. Новий стан банкомата (наприклад, технічне обслуговування) додається створенням нового класу з інтерфейсом IAtmState без редагування сервісу AtmService.

LSP (Liskov Substitution Principle) — Об'єкти будь-яких конкретних станів (NoCardState, AuthorizedState) можуть вільно замінювати один одного через інтерфейсну змінну IAtmState в контексті сервісу банкомата, не ламаючи логіку додатку.

ISP (Interface Segregation Principle) — Клієнтські класи не залежать від надлишкових методів. Інтерфейси сховища IDataStorage та процесингу грошей ITransactionProcessor повністю розділені.

DIP (Dependency Inversion Principle) — Сервіси верхнього рівня залежать від абстракцій, а не від конкретних класів. AtmService приймає в конструктор інтерфейс IDataStorage, що дозволяє легко замінити JSON на SQL базу даних.

DRY (Don't Repeat Yourself) — Логіка знаходження клієнта за номером картки або валідація суми не дублюється у формах інтерфейсу, а централізовано реалізована всередині методів AtmService.

KISS (Keep It Simple, Stupid) — Логіка обробки уникла громіздких сторонніх фреймворків. Використано чистий вбудований System.Text.Json та прямі події (event Action) для миттєвого оновлення монітора банкомата.

YAGNI (You Aren't Gonna Need It) — У моделях та станах реалізовано виключно необхідні поля й методи, які беруть участь у поточних бізнес-вимогах ТЗ (без заготовок на далеке майбутнє).

Design Patterns
Singleton
Файл: ATMSimulator/Data/JsonDataStorage.cs

Гарантує створення лише одного єдиного екземпляра класу локальної бази даних на всю програму з потокобезпечним блокуванням lock, що виключає конфлікти одночасного доступу до файлу JSON.

C#
public static JsonDataStorage Instance {
    get {
        lock (_lock) {
            if (_instance == null) _instance = new JsonDataStorage();
            return _instance;
        }
    }
}
State
Файл: ATMSimulator/States/

Дозволяє об'єкту банкомата динамічно змінювати свою поведінку під час зміни внутрішнього стану додатка. Усі фінансові операції викликаються через інтерфейс поточного стану.

C#
public void InsertCard(string cardNumber) => CurrentState?.InsertCard(this, cardNumber);
public void EnterPin(string pin) => CurrentState?.EnterPin(this, pin);
Proxy
Файл: ATMSimulator/Services/TransactionLoggerProxy.cs

Замісник, який перехоплює виклики фінансових методів до реального процесора транзакцій, виконує наскрізне приховане логування у текстовий файл із часом операції та безпечно передає виконання далі.

C#
_processor = new TransactionLoggerProxy(new RealTransactionProcessor());
Strategy
Файл: ATMSimulator/Strategies/

Виносить алгоритми підрахунку комісії в окрему сімейство класів, дозволяючи підміняти логіку зняття готівки на льоту залежно від типу банківської картки клієнта (Regular комісія 1% чи VIP комісія 0%).

C#
IWithdrawStrategy strategy = user.UserCard.CardType == "VIP" ? new VipWithdrawStrategy() : new RegularWithdrawStrategy();
Factory Method
Файл: ATMSimulator/Factories/UserFactory.cs

Централізує та ізолює складну покрокову логіку створення пов'язаних об'єктів доменних моделей (User, Card, Account), позбавляючи інші сервіси від ручного написання ключового слова new.

C#
var user = UserFactory.CreateUser("1", "Ivan", "Petrenko", "1111", "1234", 5000m, "Regular");
Refactoring Techniques
Extract Method — Величезні шматки коду конфігурації WinForms та первинної генерації бази даних у файлі були декомпоновані на читабельні ізольовані методи, такі як SeedInitialData() та InitializeComponent().

Move Method — Логіку перевірки наявності коштів на рахунку та математику зміни балансу перенесено безпосередньо всередину класу доменної моделі Account.cs ближче до самих даних, що прибрало запах коду Feature Envy (Заздрість до чужих даних).

Replace Magic Number with Named Constant — Замість використання жорстко закодованих чисел та літералів у коді станів, ліміти спроб та фіксовані курси обміну валют винесені в іменовані змінні з модифікаторами const та readonly (наприклад, const int MaxAttempts = 3).

Decompose Conditional (Заміна вкладених умов на Guard Clauses) — Глибокі вкладені блоки перевірок умов (чи існує картка, чи заблокована вона) у методах авторизації станів були розбиті на плоскі структури з раннім виходом (return), ліквідувавши Arrow Anti-pattern (код-стріла).

Encapsulate Field — Поля класів репозиторіїв, сервісів та моделей зроблені строгими приватними (private або private readonly), а доступ до них реалізовано виключно через публічні безпечні методи та геттери властивостей.
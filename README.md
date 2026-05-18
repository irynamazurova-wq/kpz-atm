ATMSimulator

Застосунок імітує роботу банкомата з візуальним інтерфейсом на Windows Forms. Локальна база даних зберігається у файлі формату JSON.

Можливості
* Авторизація користувачів за номером картки та ПІН-кодом;
* Мультивалютні рахунки (одночасний баланс в UAH, USD, EUR);
* Купівля валюти прямо в банкоматі за акту  альним курсом;
* Переказ коштів на інші картки за їхнім номером у реальному часі;
* Обмеження до 3 спроб введення ПІН-коду з автоматичним блокуванням картки;
* Автоматичне логування всіх фінансових операцій у файл transactions_log.txt;
* Збереження актуальних балансів користувачів у файлі users.json;

Запуск
Відкрийте рішення KPZ-ATM.sln у Visual Studio. Натисніть F5 для запуска. Файли бази даних users.json та логів transactions_log.txt створяться автоматично у папці виконання.

Структура проєкту
ATMSimulator/
├── Program.cs                        # Точка входу в систему
├── Form1.cs                          # Вікно авторизації 
├── Data/
│   ├── IDataStorage.cs               # Інтерфейс файлового сховища
│   └── JsonDataStorage.cs            # Реалізація бази даних 
├── Factories/
│   └── UserFactory.cs                # Фабрика об'єктів 
├── Models/
│   ├── Account.cs                    # Мультивалютний рахунок
│   ├── Card.cs                       # Платіжна картка
│   └── User.cs                       # Клієнт банку
├── Services/
│   ├── AtmService.cs                 # Головний сервіс банкомата
│   ├── RealTransactionProcessor.cs   # Процесор фінансових операцій
│   └── TransactionLoggerProxy.cs     # Логування операцій 
├── States/
│   ├── IAtmState.cs                  # Інтерфейс станів заліза 
│   ├── NoCardState.cs                # Стан: Немає картки
│   ├── WaitingForPinState.cs         # Стан: Очікування ПІН-коду
│   └── AuthorizedState.cs            # Стан: Успішна авторизація
├── Strategies/
│   ├── IWithdrawStrategy.cs          # Інтерфейс стратегій комісії 
│   ├── RegularWithdrawStrategy.cs    # Комісія 1% для Regular
│   └── VipWithdrawStrategy.cs        # Комісія 0% для VIP
└── UI/
    └── MainAtmForm.cs                # Головне вікно банкомата

Programming Principles
SRP — Single Responsibility Кожен клас відповідає за одну річ. JsonDataStorage — тільки збереження у файл. Account — тільки стан грошей. MainAtmForm — тільки відображення UI.
OCP — Open/Closed Сервіс AtmService працює через інтерфейс IAtmState. Новий стан можна додати окремим класом, не змінюючи існуючий код додатка.
LSP — Liskov Substitution Будь-який стан (NoCardState, AuthorizedState) може безперешкодно замінювати один одного через інтерфейсну змінну IAtmState.
ISP — Interface Segregation Клієнтські класи не залежать від надлишкових методів. Інтерфейси сховища IDataStorage та процесингу ITransactionProcessor повністю розділені.
DIP — Dependency Inversion Всі залежності передаються через конструктори. AtmService залежить від абстракції IDataStorage, а не від конкретного класу.
DRY — Don't Repeat Yourself Логіка пошуку користувача та зміни балансу винесена в окремі сервіси, а не дублюється в кнопках UI-форм.
KISS — Keep It Simple Проєкт реалізовано без важких сторонніх фреймворків, на чистих подіях (event Action) для миттєвого оновлення екрана.
YAGNI — You Aren't Gonna Need It У доменних моделях збережено лише ті поля, які беруть участь у поточному ТЗ (без заготовок на майбутнє).

Design Patterns
Singleton — src/Data/JsonDataStorage.cs
Гарантує існування єдиного екземпляра класу бази даних для запобігання конфліктів при одночасному читанні чи записі файлу JSON.
State — src/States/
Дозволяє об'єтку банкомата динамічно змінювати свою поведінку під час зміни внутрішнього стану. Кнопки фінансових операцій не спрацюють без авторизації.
Proxy — src/Services/TransactionLoggerProxy.cs
Перехоплює виклики методів до реального процесора транзакцій і автоматично пише кожен крок у файл логів перед виконанням логіки.
Strategy — src/Strategies/
Виносить алгоритми підрахунку комісії в окремі класи. Для VIP-клієнтів підставляється VipWithdrawStrategy (0%), для звичайних — RegularWithdrawStrategy (1%).
Factory Method — src/Factories/UserFactory.cs
Централізує створення об'єктів моделей User, Card, Account, ізолюючи логіку ініціалізації від коду сховища даних.

Refactoring Techniques
Extract Method — великі функції розбиті на дрібні (наприклад, SeedInitialData та InitializeComponent).
Extract Class — логіка обробки транзакцій винесена в окремі процесори, звільняючи сервіс банкомата від зайвої відповідальності.
Replace Magic Number with Named Constant — ліміти спроб (MaxAttempts = 3) та курси валют винесені в константи замість жорсткого коду всередині умов.
Decompose Conditional — глибокі вкладені блоки перевірок стану картки замінені на плоскі Guard Clauses з раннім поверненням (return).
Encapsulate Field — всі поля репозиторіїв та моделей приватні, доступ до них реалізовано тільки через публічні властивості та методи.
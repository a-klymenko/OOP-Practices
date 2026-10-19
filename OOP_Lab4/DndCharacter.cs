using System;
namespace Dnd
{
    public class DndCharacter
    {
        // приватні поля
        private string name = "";  // = "" не було null
        private CharacterClass charClass;
        private int level;
        private int health;


        // статік поля
        private static int characterCount = 0;
        // макс рівень персонажів
        private static int maxLevel = 20;

        //статів кластивості
        public static int CharacterCount
        {
            get
            {
                return characterCount;
            }
        }

        // макс рівень для всіх персонажів
        public static int MaxLevel
        {
            get
            {
                return maxLevel;
            }
            set
            {
                // максимальний рівень більше 0
                if (value < 1)
                {
                    throw new ArgumentException(
                        "Максимальний рівень повинен бути більше 0");
                }
                maxLevel = value;
            }
        }

        // автовластивість за замовчуванням
        public string Faction { get; set; } = "Guild";
        // властивість
        public int Experience { get; private set; } = 0;

        // Властивості
        // перевірка імені
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                //  ім'я не порожне
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ім'я персонажа не може бути порожнім");
                }
                name = value;
            }
        }

        // перевірка класу
        public CharacterClass Class
        {
            get
            {
                return charClass;
            }
            set
            {
                // значення в enum
                if (!Enum.IsDefined(typeof(CharacterClass), value))
                {
                    throw new ArgumentException("Обрано неіснуючий клас");
                }
                charClass = value;
            }
        }

        // рівні
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                // від 1 до 20
                 if (value < 1 || value > MaxLevel)
                {
                    throw new ArgumentException(
                        $"Рівень повинен бути від 1 до {MaxLevel}");
                }
                level = value;
            }
        }

        // здоров'я
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                // здоров'я не може бути від'ємним
                if (value < 0)
                {
                    throw new ArgumentException("Здоров'я не може бути менше 0");
                }
                health = value;
            }
        }

        // обчислювальна властивість
        public string FullTitle
        {
            get
            {
                return $"{Name} ({Class}) - рівень {Level}";
            }
        }

        // Конструктор без параметрів
        public DndCharacter()
            : this( "Безіменний герой", CharacterClass.Warrior, 1, 100)
        {
        }


        // Перевантажений конструктор
        public DndCharacter(string name)
            : this( name, CharacterClass.Warrior, 1, 100)
        {
        }

        public DndCharacter(string name, CharacterClass charClass)
            : this(name, charClass, 1, 100)
        {
        }

        // Повний конструктор
        public DndCharacter(string name, CharacterClass charClass, int level, int health)
        {
            // пер властивості
            Name = name;
            Class = charClass;
            Level = level;
            Health = health;
            // рахуємо кожного створеного персонажа
            characterCount++;
        }

        // приватний метод
        private void LogAction(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }

        // перевантаження методу
        public void GainExperience(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Experience += amount;

            LogAction( $"Персонаж {Name} отримав {amount} досвіду, " +  $"всього досвіду {Experience}");
        }

        public void GainExperience(int amount, string reason)
        {
            if (amount <= 0)
            {
                return;
            }
            Experience += amount;
            LogAction( $"Персонаж {Name} отримав {amount} досвіду " + $"за {reason}, всього досвіду {Experience}");
        }

        // перевантаження методу отримує шкоди
        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                return;
            }
            // здоров'я0
            Health = Math.Max( 0, Health - damage);
            LogAction( $"Персонаж {Name} отримав {damage} шкоди, " + $"залишилось здоров'я {Health}");
        }

        public void TakeDamage(int damage, string damageType)
        {
            if (damage < 0)
            {
                return;
            }
            Health = Math.Max(0, Health - damage);

            LogAction($"Персонаж {Name} отримав {damage} шкоди " + $"типу {damageType}, " + $"залишилось здоров'я {Health}");
        }

        // статік метод здоров'я від рівня
        public static int CalculateRecommendedHealth(int level)
        {
            if (level < 1 || level > MaxLevel)
            {
                throw new ArgumentException($"Рівень повинен бути від 1 до {MaxLevel}");
            }
            // 1 рівен 100 HP +10 HP
            return 100 + (level - 1) * 10;
        }

        //парс
        public static DndCharacter Parse(string text)
        {
            // порожній
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Рядок не може бути порожнім");
            }
            // ділимо ;
            string[] parts = text.Split(new char[]{' ',';',','});
            // 4 частини
            if (parts.Length != 4)
            {
                throw new FormatException("Правильний формат: Ім';клас;рівень;здоров'я");
            }

            // 1 ім'я
            string parsedName = parts[0].Trim();
            if (string.IsNullOrWhiteSpace(parsedName))
            {
                throw new FormatException("Ім'я персонажа не може бути порожнім");
            }

            // 2  клас
            if (!Enum.TryParse(parts[1].Trim(), true, out CharacterClass parsedClass))
            {
                throw new FormatException("Некоректний клас персонажа");
            }
            // перевірка enum
            if (!Enum.IsDefined(typeof(CharacterClass), parsedClass))
            {
                throw new FormatException("не той клас персонажа");
            }

            // 3 частина - рівень
            if (!int.TryParse(parts[2].Trim(), out int parsedLevel))
            {
                throw new FormatException("Рівень повинен бути цілим числом");
            }

            // 4 частина - здоров'я
            if (!int.TryParse(parts[3].Trim(), out int parsedHealth))
            {
                throw new FormatException("Здоров'я повинно бути цілим числом");
            }
            // новий персонаж
            return new DndCharacter(parsedName, parsedClass, parsedLevel, parsedHealth);
        }

        public static bool TryParse(string text, out DndCharacter? character)
        {
            try
            {
                character = Parse(text);
                return true;            
            }
            catch
            {
                character = null;
                return false;
            }
        }

        // формат персонажа
        public override string ToString()
        {
            return $"{Name};{Class};{Level};{Health}";
        }
    }
}
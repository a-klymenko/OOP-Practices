using System;
using System.Collections.Generic;

namespace Dnd
{
    public class CharacterManager
    {
        // список для зберігання
        private readonly List<DndCharacter> characters = new List<DndCharacter>();

        // + персонаж
        public void AddCharacter()
        {
            Console.WriteLine();
            Console.WriteLine("Оберіть спосіб створення персонажа:");
            Console.WriteLine("1 - Конструктор без параметрів");
            Console.WriteLine("2 - Конструктор тільки з ім'ям");
            Console.WriteLine("3 - Конструктор з ім'ям та класом");
            Console.WriteLine("4 - Повний конструктор");
            Console.WriteLine("5 - Створити персонажа з рядка");
            Console.Write("Ваш вибір: ");
            
            string choice = Console.ReadLine() ?? "";

            // змінна героя
            DndCharacter hero;
            try
            {
                switch (choice)
                {
                    // конструктор без параметрів
                    case "1": 
                        hero =new DndCharacter();
                        break;
                        
                    // конструктор з ім'ям
                    case "2":
                        Console.Write("Ім'я персонажа: ");
                        string name1 =Console.ReadLine() ?? "";
                        hero =new DndCharacter(name1);
                        break;

                    // конструктор з ім'ям та класом
                    case "3":
                        Console.Write("Ім'я персонажа: ");
                        string name2 =Console.ReadLine() ?? "";
                        CharacterClass class2 =ReadClass();
                        hero =new DndCharacter(name2,class2);
                        break;

                    // повний конструктор
                    case "4":
                        Console.Write("Ім'я персонажа: ");
                        string name4 =Console.ReadLine() ?? "";
                        CharacterClass class4 =ReadClass();
                        int level = ReadLevel();
                        int health = ReadHealth();
                        hero = new DndCharacter(name4, class4, level, health);
                        break;
                    //default: Console.WriteLine("Некоректний вибір конструктора.");
                        //return;

                    //персонажа через TryParse
                    case "5":
                        Console.WriteLine("Введіть персонажа у форматі:");
                        Console.WriteLine("Name;Class;Level;Health");
                        Console.WriteLine("Наприклад: Arthur;Warrior;5;150");
                        Console.Write("Рядок: ");
                        string text =Console.ReadLine() ?? "";

                        // рядок в об'єкт
                        if (!DndCharacter.TryParse(text, out DndCharacter? parsedHero))
                        {
                            Console.WriteLine("Помилка формату рядка");
                            return;
                        }

                        // не null
                        hero = parsedHero!;
                        break;

                    default:
                        Console.WriteLine("Некоректний вибір");
                        return;
                }

                // додаємо створеного перса до списку
                characters.Add(hero);
                Console.WriteLine("Персонаж додан");
                // показуємо, який саме персонаж створений
                Console.WriteLine(hero);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        // Клас
        private CharacterClass ReadClass()
        {
            while (true)
            {
                Console.WriteLine("Оберіть клас:");
                Console.WriteLine("0 - Warrior");
                Console.WriteLine("1 - Mage");
                Console.WriteLine("2 - Rogue");
                Console.Write("Ваш вибір: ");

                // текст на int
                if (int.TryParse(Console.ReadLine(), out int classNum))
                {
                    // чи є 
                    if (Enum.IsDefined(typeof(CharacterClass), classNum))
                    {
                        return (CharacterClass)classNum;
                    }
                }
                Console.WriteLine("Помилка: введіть число від 0 до 2.");
            }
        }

        // Рівень
        private int ReadLevel()
        {
            while (true)
            {
                Console.Write("Введіть рівень від 1 до {DndCharacter.MaxLevel}: ");
                if (int.TryParse(Console.ReadLine(),out int lvl))
                {
                    if (lvl >= 1 && lvl <= DndCharacter.MaxLevel)
                    {
                        return lvl;
                    }
                }
                Console.WriteLine("Помилка: введіть число від 1 до {DndCharacter.MaxLevel}");
            }
        }

        // Здоров'я
        private int ReadHealth()
        {
            while (true)
            {
                Console.Write("Введіть рівень здоров'я, більше або дорівнює 0: ");
                if (int.TryParse(Console.ReadLine(), out int hp))
                {
                    if (hp >= 0)
                    {
                        return hp;
                    }
                }
                Console.WriteLine("Помилка: введіть ціле невід'ємне число.");
            }
        }

        // Список персонажа
        public void ShowAllCharacters()
        {
            if (characters.Count == 0)
            {
                Console.WriteLine("У списку нікого нема");
                return;
            }
    
            Console.WriteLine("Список персонажів:");
            for (int i = 0; i < characters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {characters[i]}");
            }

            Console.WriteLine($"Всього створено персонажів: " + "{DndCharacter.CharacterCount}");
        }

        // пошук персонажа
        public void FindCharacter()
        {
            Console.WriteLine("Введіть ім'я розшукуємого:");
            string searchName = Console.ReadLine() ?? "";
            DndCharacter? found = characters.Find(c => c.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                Console.WriteLine($"Знайдено: {found}");
            }
            else
            {
                Console.WriteLine("Такого ще нема");
            }
        }

        // демонстрація поведінки
        public void DemonstrateBehavior()
        {
            if (characters.Count == 0)
            {
                Console.WriteLine(
                    "Персонажей нема");
                return;
            }

            DndCharacter hero = characters[0];
            Console.WriteLine($"\n Демонстрація дій ({hero.Name}):");

            // перевантаження
            hero.GainExperience(100);
            hero.GainExperience(50, "виконання квесту");
            hero.TakeDamage(10);
            hero.TakeDamage(15, "магічна атака");
            Console.WriteLine();
            Console.WriteLine("Поточний стан персонажа:");
            Console.WriteLine(hero);
        }

        // - персонажа
        public void DeleteCharacter()
        {
            Console.Write("Введіть ім'я для видалення: ");
            string searchName =Console.ReadLine() ?? "";
            // якщо персонажа не знайдено
            DndCharacter? found = characters.Find(c => c.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                // видалення
                characters.Remove(found);
                Console.WriteLine("Персонажа успішно видалено.");
            }
            else
            {
                Console.WriteLine("Такого нема");
            }
        }

            public void DemonstrateStaticMethods()
            {
                Console.WriteLine("ДЕМОНСТРАЦІЯ 4");
                // static властивість
                Console.WriteLine($"Всього створено персонажів:  + {DndCharacter.CharacterCount}");
                Console.WriteLine($"Максимальний рівень:  + {DndCharacter.MaxLevel}");
                // метод статік
                Console.Write("Введіть рівень для розрахунку здоров'я: ");
                if (int.TryParse(Console.ReadLine(), out int level))
                {
                    try
                    {
                        // static метод через ім'я класу
                        int recommendedHealth = DndCharacter.CalculateRecommendedHealth(level);
                        Console.WriteLine($"Рекомендоване здоров'я:  + {recommendedHealth}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Рівень повинен бути числом");
                }

                // парс
                Console.WriteLine("Демонстрація Parse:");
                try
                {
                    string parseText ="Arthur;Warrior;5;150";
                    DndCharacter parsedHero =DndCharacter.Parse(parseText);
                    Console.WriteLine($"Рядок: {parseText}");
                    Console.WriteLine($"Створено: {parsedHero}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка Parse: {ex.Message}");
                }

                // трайпас
                Console.WriteLine("Демонстрація TryParse:");
                string tryParseText = "Merlin;Mage;8;170";
                bool result = DndCharacter.TryParse(tryParseText, out DndCharacter? tryParsedHero);
                if (result && tryParsedHero != null)
                {
                    Console.WriteLine($"TryParse успішний: {tryParsedHero}");
                }
                else
                {
                    Console.WriteLine("TryParse не зміг створити персонажа");
                }
        }
    }
}
using System;

namespace Dnd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CharacterManager manager = new CharacterManager();
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Додати об'єкт");
                Console.WriteLine("2 - Переглянути всі об'єкти");
                Console.WriteLine("3 - Знайти об'єкт");
                Console.WriteLine("4 - Продемонструвати поведінку");
                Console.WriteLine("5 - Видалити об'єкт");
                Console.WriteLine("6 - Продемонструвати static, Parse і TryParse");
                Console.WriteLine("0 - Вийти з програми");

                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine() ?? "";

                // вибір відповідної дії меню
                switch (choice)
                {
                    // додавання персонажа
                    case "1":
                        manager.AddCharacter();
                        break;
                    // показати всіх персонажів
                    case "2":
                        manager.ShowAllCharacters();
                        break;
                    // пошук персонажа
                    case "3":
                        manager.FindCharacter();
                        break;
                    // демонстрація методів, перевантаження
                    case "4":
                        manager.DemonstrateBehavior();
                        break;
                    // видалення персонажа
                    case "5":
                        manager.DeleteCharacter();
                        break;
                    //демонстрація static-властивостей, static-методу, Parse та TryParse
                    case "6":
                        manager.DemonstrateStaticMethods();
                        break;
                    // вихід із програми
                    case "0":
                        running = false;
                        Console.WriteLine("Завершення роботи програми.");
                        break;
                    // якщо введено невідоме значення
                    default:Console.WriteLine("Некоректний вибір! Спробуйте ще");
                        break;
                }
            }
        }
    }
}
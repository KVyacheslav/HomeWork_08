using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeWork_08
{
    public class Program
    {
        /// <summary>
        /// Список отделов.
        /// </summary>
        private static List<Departament> departaments;

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Параметры запуска.</param>
        public static void Main(string[] args)
        {
            Initialize();
            StartMenu();

            Console.ReadLine();
        }

        /// <summary>
        /// Инициализировать данные.
        /// </summary>
        private static void Initialize()
        {
            Console.SetWindowSize(160, 60);
            departaments = new List<Departament>();
        }

        /// <summary>
        /// Сгенерировать список отделов.
        /// </summary>
        /// <returns>Список отделов</returns>
        private static void GenerateDepartaments()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Генерация отделов <<<\n");
            Console.WriteLine(new string('*', 50));

            departaments = new List<Departament>();
            Random rnd = new Random();
            Worker.Reset();

            int countDepartaments = rnd.Next(3, 6);
            int id = 0;

            for (int i = 1; i < countDepartaments; i++)
            {
                Departament tmpDep = new Departament($"Departament_{i}");
                int countWorkers = rnd.Next(5, 15);

                for (int j = 1; j < countWorkers; j++)
                {
                    string firstName = $"FirstName_{++id}";
                    string lastName = $"LastName_{id}";
                    byte age = (byte)rnd.Next(20, 56);
                    uint salary = (uint)rnd.Next(4, 22) * 10_000;
                    byte countProject = (byte)rnd.Next(1, 6);
                    tmpDep.Add(new Worker(firstName, lastName, age, salary, countProject, tmpDep));
                }

                departaments.Add(tmpDep);
            }

            Console.WriteLine($"\nСгенерировано {departaments.Count} отделов и {id} сотрудников.");

            Console.ReadKey(true);
            Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Вывести на консоль меню работы с отделами.
        /// </summary>
        private static void PrintMainMenu()
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Корпорация зла <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Меню работы с отделами <<<\n");
            Console.WriteLine("1) Вывести информацию об отделах.");
            Console.WriteLine("2) Загрузить список отделов из файла.");
            Console.WriteLine("3) Сохранить список отделов в файл.");
            Console.WriteLine("4) Добавить отдел.");
            Console.WriteLine("5) Удалить отдел.");
            Console.WriteLine("6) Изменить отдел.");
            Console.WriteLine("7) Сгенерировать отделы.");
            Console.WriteLine("8) Сортировка отделов.");
            Console.WriteLine("0) Выход из программы.\n");
            Console.WriteLine(new string('*', 50));
        }

        /// <summary>
        /// Запуск функций программы.
        /// </summary>
        private static void StartMenu()
        {
            PrintMainMenu();
            var userInput = GetUserInput();
            switch (userInput)
            {
                case 0:
                    Console.WriteLine("Всего доброго!");
                    return;
                case 1:
                    PrintInfoDepartaments();
                    break;
                case 2:
                    LoadDepartaments();
                    break;
                case 3:
                    SaveDepartaments();
                    break;
                case 4:
                    AddDepartament();
                    break;
                case 5:
                    DeleteDepartament();
                    break;
                case 6:
                    ChangeDepartament();
                    break;
                case 7:
                    GenerateDepartaments();
                    break;
                case 8:
                    MenuSortDepartaments();
                    break;
                default:
                    Console.Clear();
                    StartMenu();
                    break;
            }
        }

        /// <summary>
        /// Вывести полную информацию о всех отделах.
        /// </summary>
        private static void PrintInfoDepartaments()
        {
            Console.Clear();
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Информация об отделах. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\nКоличество отделов: {departaments.Count}.\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            foreach (var dep in departaments)
            {
                Console.WriteLine($"Название отдела: {dep.Name}.");
                Console.WriteLine($"Дата создания отдела: {dep.DateCreation.ToShortDateString()}.");
                Console.WriteLine($"Количество сотрудников в отделе: {dep.CountWorkers}.\n");
            }

            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\nНажмите Enter для возврата в меню...");

            Console.ReadLine(); Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Загрузить список отделов.
        /// </summary>
        private static void LoadDepartaments()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Загрузка данных из файла. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            while (true)
            {
                Console.Write("Введите название файла без расширения: ");
                string path = Console.ReadLine() + ".json";

                if (!File.Exists(path))
                {
                    Console.WriteLine("Такого файла нет!");
                    continue;
                }

                Console.WriteLine("Данные загружаются...");
                try
                {
                    Worker.Reset();
                    departaments = Service.LoadDepartamentsFromJSON(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                    continue;
                }
                Console.WriteLine("Данные успешно загружены!");

                Console.ReadKey(true); Console.Clear();
                StartMenu();
            }
        }

        /// <summary>
        /// Сохранить список отделов.
        /// </summary>
        private static void SaveDepartaments()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Сохранение данных в файл. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            Console.Write("Введите название файла без расширения: ");
            string path = Console.ReadLine();
            string pathXml = path + ".xml";
            string pathJson = path + ".json";

            Console.WriteLine("Фай сохраняется...");
            Service.SaveDepartamentsToXml(pathXml, departaments);
            Service.SaveDepartamentsToJson(pathJson, departaments);
            Console.WriteLine("Данные успешно сохранены!");

            Console.ReadKey(true);
            Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Добавить отдел.
        /// </summary>
        private static void AddDepartament()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Создание департамента. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();
            Console.WriteLine("Введите название отдела:");

            while (true)
            {
                Console.Write(">>> ");
                string depName = Console.ReadLine();

                if (string.IsNullOrEmpty(depName.Trim()))
                    continue;

                Departament tmpDep = new Departament(depName);

                if (!departaments.Contains(tmpDep))
                {
                    departaments.Add(new Departament(depName));
                    Console.WriteLine($"Отдел с названием {depName} успешно создан.");
                }
                else
                    Console.WriteLine("Такой отдел существует уже!");

                break;
            }

            Console.ReadKey(true);
            Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Удалить департамент.
        /// </summary>
        private static void DeleteDepartament()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Удаление департамента. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();
            Console.WriteLine("Введите название отдела:");

            bool isFound = false;       // Найден ли отдел?
            
            while (true)
            {
                Console.Write(">>> ");
                string depName = Console.ReadLine();

                if (string.IsNullOrEmpty(depName.Trim()))
                    continue;

                foreach (var departament in departaments)
                {
                    if (departament.Name == depName)
                    {
                        Console.WriteLine("Отдел найден.");
                        Console.WriteLine($"Количество сотрудников в отделе: {departament.CountWorkers}.");
                        Console.Write("Действительно хотите удалить отдел? (д/н): ");
                        if (Console.ReadKey(true).Key == ConsoleKey.L)
                        {
                            departaments.Remove(departament);
                            Console.WriteLine($"\nОтдел {depName} успешно удален.");
                        }

                        isFound = true;

                        break;
                    }
                }

                if (!isFound)
                    Console.WriteLine("Отдел не найден.");

                break;
            }

            Console.ReadKey(true);
            Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Изменить департамент.
        /// </summary>
        private static void ChangeDepartament()
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Изменение департамента. <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Список отделов:");
            for (int i = 0; i < departaments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {departaments[i].Name}");
            }
            Console.WriteLine("\nВведите номер отдела:");

            bool isFound = false;           // Найден ли отдел?

            while (true)
            {
                int userInput = GetUserInput();

                if (userInput < 1 || userInput > departaments.Count)
                    continue;

                for (int i = 0; i < departaments.Count; i++)
                {
                    if (i == userInput)
                    {
                        isFound = true;
                        Console.WriteLine("Отдел найден.");
                        Console.WriteLine("Для продолжения нажмите Enter...");
                        Console.ReadLine();
                        Console.Clear();
                        StartMenuEditDepartament(--i);
                        break;
                    }
                }

                if (!isFound)
                    Console.WriteLine("Отдел не найден.");

                break;
            }

            Console.ReadKey(true);
            Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Вывести на консоль меню работы с отделом.
        /// </summary>
        private static void PrintMenuEditDepartament()
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Редактирование отдела <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Меню работы с отделом <<<\n");
            Console.WriteLine("1) Изменить название отдела.");
            Console.WriteLine("2) Изменить список работников.");
            Console.WriteLine("0) Выход из меню.\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();
        }

        /// <summary>
        /// Запуск функций работы с отделом.
        /// </summary>
        private static void StartMenuEditDepartament(int index)
        {
            PrintMenuEditDepartament();

            int userInput = GetUserInput();

            switch (userInput)
            {
                case 1:
                    ChangeDepartamentName(index);
                    break;
                case 2:
                    EditWorkers(index);
                    break;
                case 0:
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    StartMenuEditDepartament(index);
                    break;
            }

            Console.WriteLine("Нажмите ENTER для продолжения...");
            Console.ReadLine();Console.Clear();
        }

        private static void MenuSortDepartaments()
        {
            Console.Clear();
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Сортировка отделов <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Выбирете критерий, по чему будете сортировать отделы:\n");
            Console.WriteLine("1) По названию.");
            Console.WriteLine("2) По количеству рабочих.");
            Console.WriteLine("3) По дате создания.");
            Console.WriteLine("0) Выход.");

            int userInput = GetUserInput();

            switch (userInput)
            {
                case 0:
                    Console.Clear();
                    StartMenu();
                    return;
                case 1:
                    departaments = departaments
                        .OrderBy(d => d.Name)
                        .ToList();
                    break;
                case 2:
                    departaments = departaments
                        .OrderBy(d => d.GetListWorkers().Count)
                        .ToList();
                    break;
                case 3:
                    departaments = departaments
                        .OrderBy(d => d.DateCreation)
                        .ToList();
                    break;
                default:
                    MenuSortDepartaments();
                    break;
            }

            Console.WriteLine("\nОтделы успешно отсортированы. Для продолжения нажмите Enter...");
            Console.ReadLine(); Console.Clear();
            StartMenu();
        }

        /// <summary>
        /// Редактирование рабочих.
        /// </summary>
        /// <param name="lists"></param>
        private static void EditWorkers(int index)
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Редактирование рабочих отдела {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\nКоличество рабочих в отделе: {departaments[index].CountWorkers}.\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n1) Добавить рабочего.");
            Console.WriteLine("2) Удалить рабочего.");
            Console.WriteLine("3) Изменить рабочего.");
            Console.WriteLine("4) Показать список рабочих.");
            Console.WriteLine("5) Сортировать список рабочих.");
            Console.WriteLine("0) Выход из меню.\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            int userInput = GetUserInput();

            Console.Clear();

            switch (userInput)
            {
                case 0:
                    StartMenuEditDepartament(index);
                    break;
                case 1:  
                    AddWorker(index);
                    break;
                case 2:
                    RemoveWorker(index);
                    break;
                case 3:
                    EditWorker(index);
                    break;
                case 4:
                    PrintAllWorkersInDepartament(index);
                    break;
                case 5:
                    MenuSortWorkers(index);
                    break;
                default:
                    EditWorkers(index);
                    break;

            }
        }

        /// <summary>
        /// Меню сортировки работников.
        /// </summary>
        private static void MenuSortWorkers(int index)
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Меню сортировки рабочих в отделе {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n1) Сортировка по ID.");
            Console.WriteLine("2) Сортировка по имени и фамилии.");
            Console.WriteLine("3) Сортировка по возрасту.");
            Console.WriteLine("4) Сортировка по зарплате.");
            Console.WriteLine("5) Сортировка по количеству проектов.");
            Console.WriteLine("0) Выход из меню.");

            int userInput = GetUserInput();

            switch (userInput)
            {
                case 1:
                    departaments[index] = departaments[index].SortById();
                    break;
                case 2:
                    departaments[index] = departaments[index].SortByName();
                    break;
                case 3:
                    departaments[index] = departaments[index].SortByAge();
                    break;
                case 4:
                    departaments[index] = departaments[index].SortBySalary();
                    break;
                case 5:
                    departaments[index] = departaments[index].SortByCountProjects();
                    break;
                case 0:
                    Console.Clear();
                    EditWorkers(index);
                    return;
                default:
                    MenuSortWorkers(index);
                    return;
            }

            Console.WriteLine("Данные успешно отсортированы. Для продолжения нажмите Enter...");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Показать список рабочих в отделе.
        /// </summary>
        /// <param name="index">Индекс отдела.</param>
        private static void PrintAllWorkersInDepartament(int index)
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Список рабочих в отделе {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            var workers = departaments[index].GetListWorkers();

            Console.WriteLine($"{"ИД",8} | {"Имя",12} | {"Фамилия",12} | {"Возраст",8} | {"Отдел",14} | {"Зарплата",9:N0} | {"Кол-во проектов",16}");

            foreach (var worker in workers)
            {
                Console.WriteLine(worker);
            }

            Console.WriteLine();
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ReadLine();
        }

        /// <summary>
        /// Удалить работника.
        /// </summary>
        /// <param name="index">Индекс работника.</param>
        private static void RemoveWorker(int index)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Удаление рабочего рабочего в отдел {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            Console.WriteLine("Введите ID сотрудника или 0 для выхода:");

            while(true)
            {
                int userInput = GetUserInput();
                bool isFound = false;           // найден ли сотрудник?

                if (userInput == 0)
                    return;

                var workers = departaments[index].GetListWorkers();

                foreach (var worker in workers)
                {
                    if (worker.Id == userInput)
                    {
                        departaments[index].GetListWorkers().Remove(worker);
                        Console.WriteLine($"Рабочий {worker.FirstName} {worker.LastName} успешно удален!");
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                    Console.WriteLine("Сотрудника с таким ID нет.");
            }

            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Добавить рабочего в отдел.
        /// </summary>
        /// <param name="index">Индекс отдела.</param>
        private static void AddWorker(int index)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Добавление рабочего в отдел {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            Console.Write("Введите имя рабочего: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите фамилию рабочего: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите возраст рабочего: ");
            byte age = byte.Parse(Console.ReadLine());
            Console.Write("Введите зарплату рабочего: ");
            uint salary = uint.Parse(Console.ReadLine());
            Console.Write("Введите количество проектов закрепленных за рабочим: ");
            byte countProjects = byte.Parse(Console.ReadLine());

            new Worker(firstName, lastName, age, salary, countProjects, departaments[index]);

            Console.WriteLine("Работник успешно добавлен.");
            Console.ReadLine();
        }

        /// <summary>
        /// Изменить рабочего.
        /// </summary>
        /// <param name="index">Индекс работника.</param>
        private static void EditWorker(int index)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Изменение рабочего в отдел {departaments[index].Name} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            Console.WriteLine("Введите ID сотрудника или 0 для выхода:");

            while (true)
            {
                int userInput = GetUserInput();
                bool isFound = false;           // найден ли сотрудник?

                if (userInput == 0)
                    return;

                var workers = departaments[index].GetListWorkers();

                for (int i = 0; i < workers.Count; i++)
                {
                    if (workers[i].Id == userInput)
                    {
                        //departaments[index].GetListWorkers().Remove(workers[i]);
                        //Console.WriteLine($"Рабочий {workers[i].FirstName} {workers[i].LastName} успешно удален!");
                        StartMenuEditWorker(workers, i);
                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                    break;

                Console.WriteLine("Сотрудника с таким ID нет.");
            }

            EditWorkers(index);
        }

        /// <summary>
        /// Меню изменения рабочего.
        /// </summary>
        /// <param name="worker">Рабочий.</param>
        private static void StartMenuEditWorker(List<Worker> workers, int indexWorker)
        {
            Console.Clear();
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Меню изменение рабочего {workers[indexWorker].FirstName} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();
            Console.WriteLine("1) Изменить зарплату.");
            Console.WriteLine("2) Изменить количество проектов.");
            Console.WriteLine("3) Изменить отдел.");
            Console.WriteLine("0) Выход.");

            int userInput = GetUserInput();


            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    ChangeSalaryWorker(workers, indexWorker);
                    break;
                case 2:
                    ChangeCountProjectsWorker(workers, indexWorker);
                    break;
                case 3:
                    ChangeDepartamentWorker(workers, indexWorker);
                    break;
                default:
                    Console.Clear();
                    StartMenuEditWorker(workers, indexWorker);
                    break;
            }

        }

        /// <summary>
        /// Изменить зарплату сотруднику.
        /// </summary>
        /// <param name="workers">Список рабочих.</param>
        /// <param name="indexWorker">Индекс рабочего.</param>
        private static void ChangeSalaryWorker(List<Worker> workers, int indexWorker)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Изменение зарплаты рабочего {workers[indexWorker].FirstName} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            while (true)
            {
                int amount = GetUserInput();

                if (amount < 10000)
                {
                    Console.WriteLine("Зарплата не может быть менее 10 000.");
                    continue;
                }

                workers[indexWorker].SetSalary((uint)amount);
                break;
            }

            Console.ReadLine(); Console.Clear();
            StartMenuEditWorker(workers, indexWorker);
        }

        /// <summary>
        /// Изменить количество проектов закрепленных за рабочим.
        /// </summary>
        /// <param name="workers">Список рабочих.</param>
        /// <param name="indexWorker">Индекс рабочего.</param>
        private static void ChangeCountProjectsWorker(List<Worker> workers, int indexWorker)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Изменение кол-ва проектов рабочего {workers[indexWorker].FirstName} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();

            while (true)
            {
                int amount = GetUserInput();

                if (amount < 0 || amount > 5)
                {
                    Console.WriteLine("Кол-во проектов не должно быть менее 0 или более 5.");
                    continue;
                }

                workers[indexWorker].SetCountProjects((byte)amount);
                break;
            }

            Console.ReadLine();
            Console.Clear();
            StartMenuEditWorker(workers, indexWorker);
        }

        /// <summary>
        /// Изменить отдел работнику.
        /// </summary>
        /// <param name="workers">Список работников.</param>
        /// <param name="indexWorker">Индекс работника.</param>
        private static void ChangeDepartamentWorker(List<Worker> workers, int indexWorker)
        {
            Console.Clear();

            Console.WriteLine(new string('*', 50));
            Console.WriteLine($"\n>>> Изменение департамента рабочего {workers[indexWorker].FirstName} <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine();
            Console.WriteLine("Список департаментов:");
            departaments.ForEach(dep => Console.WriteLine(dep.Name));

            Console.WriteLine("\nВведите название отдела или \"exit\" для выхода:");
            while (true)
            {
                Console.Write(">>> ");
                string name = Console.ReadLine();
                bool isFound = false;

                for (int i = 0; i < departaments.Count; i++)
                {
                    if (name.ToLower() == departaments[i].Name.ToLower())
                    {
                        workers[indexWorker].SetDepartament(departaments, i);
                        isFound = true;
                        break;
                    }
                }

                if (isFound || name.ToLower() == "exit")
                    break;
            }

            Console.ReadLine();
            Console.Clear();
            StartMenuEditWorker(workers, indexWorker);
        }

        /// <summary>
        /// Изменение названия отдела.
        /// </summary>
        /// <param name="departament"></param>
        private static void ChangeDepartamentName(int index)
        {
            Console.Clear();
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\n>>> Редактирование имени отдела <<<\n");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("\nВведите название отдела:");

            while (true)
            {
                Console.Write(">>> ");
                string depName = Console.ReadLine();

                if (string.IsNullOrEmpty(depName.Trim()))
                    continue;

                var tmpDep = departaments[index];
                tmpDep.Name = depName;

                departaments[index] = tmpDep;

                //departaments[index].SetName(depName);

                Console.WriteLine("Название отдела успешно изменено!");
                Console.ReadLine();
                break;
            }

            Console.Clear();
            StartMenuEditDepartament(index);
        }

        /// <summary>
        /// Получить число пользователя.
        /// </summary>
        /// <returns>Число.</returns>
        private static int GetUserInput()
        {
            while (true)
            {
                Console.Write($">>> ");

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Значение должно быть числовым!");
            }
        }
        
    }
}

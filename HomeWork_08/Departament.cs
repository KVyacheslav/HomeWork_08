using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_08
{
    /// <summary>
    /// Класс, описывающий департамент.
    /// </summary>
    public struct Departament : IEnumerable
    {
        #region Fields

        /// <summary>
        /// Название департамента.
        /// </summary>
        private string name;

        /// <summary>
        /// Дата создания департамента.
        /// </summary>
        private DateTime dateCreation;

        /// <summary>
        /// Список сотрудников.
        /// </summary>
        private List<Worker> workers;

        #endregion

        #region Properties

        /// <summary>
        /// Название департамента.
        /// </summary>
        public string Name
        {
            get => this.name;
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    Console.WriteLine("Имя не должно быть пустым или null.");
                    return;
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Дата создания департамента.
        /// </summary>
        public DateTime DateCreation => this.dateCreation;

        /// <summary>
        /// Количество рабочих.
        /// </summary>
        public int CountWorkers => this.workers.Count;

        /// <summary>
        /// Рабочий.
        /// </summary>
        /// <param name="index">Индекс.</param>
        /// <returns>Рабочий.</returns>
        public Worker this[int index] => this.workers[index];

        #endregion

        #region Constructors

        /// <summary>
        /// Создание департамента.
        /// </summary>
        /// <param name="name">Название департамента.</param>
        public Departament(string name)
        {
            this.workers = new List<Worker>();
            this.dateCreation = DateTime.Now;
            this.name = name;
        }

        /// <summary>
        /// Создание департамента.
        /// </summary>
        /// <param name="name">Название департамента.</param>
        /// <param name="dateCreation">Дата создания департамента.</param>
        public Departament(string name, DateTime dateCreation)
        {
            this.workers = new List<Worker>();
            this.dateCreation = dateCreation;
            this.name = name;
        }

        /// <summary>
        /// Создание департамента.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <param name="dateCreation">Дата создания.</param>
        /// <param name="workers">Список рабочих.</param>
        private Departament(string name, DateTime dateCreation, List<Worker> workers)
        {
            this.name = name;
            this.dateCreation = dateCreation;
            this.workers = workers;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Получить список работников.
        /// </summary>
        public List<Worker> GetListWorkers() => this.workers;
        
        /// <summary>
        /// Сортировка по Id.
        /// </summary>
        public Departament SortById()
        {
            return new Departament(name, dateCreation, workers
               .OrderByDescending(w => w.Id)
                .ToList());
        }

        /// <summary>
        /// Сортировка по имени.
        /// </summary>
        public Departament SortByName()
        {
            return new Departament(name, dateCreation, workers
               .OrderBy(w => w.FirstName)
                .ThenBy(w => w.LastName)
                .ToList());
        }

        /// <summary>
        /// Сортировка по имени.
        /// </summary>
        public Departament SortByAge()
        {
            return new Departament(name, dateCreation, workers
                .OrderBy(w => w.Age)
                .ToList());
        }

        /// <summary>
        /// Сортировка по зарплате.
        /// </summary>
        public Departament SortBySalary()
        {
            return new Departament(name, dateCreation, workers
               .OrderBy(w => w.Salary)
                .ToList());
        }

        /// <summary>
        /// Сортировка по количеству проектов.
        /// </summary>
        public Departament SortByCountProjects()
        {
            return new Departament(name, dateCreation, workers
               .OrderBy(w => w.CountProjects)
                .ToList());
        }
        
        /// <summary>
        /// Добавить рабочего.
        /// </summary>
        /// <param name="worker">Рабочий</param>
        public void Add(object worker)
        {
            if (!(worker is Worker))
                return;

            Worker w = (Worker)worker;

            if (!workers.Contains(w))
                this.workers.Add(w);
        }

        /// <summary>
        /// Метод для перечисления рабочих.
        /// </summary>
        /// <returns>Перечисление.</returns>
        public IEnumerator GetEnumerator()
        {
            return workers.GetEnumerator();
        }

        #endregion

        #region Override methods

        public override bool Equals(object obj)
        {
            if (!(obj is Departament))
                return false;

            Departament departament = (Departament)obj;

            if (departament.Name != this.Name)
                return false;

            return true;
        }

        #endregion
    }
}

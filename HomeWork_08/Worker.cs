using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_08
{
    /// <summary>
    /// Класс, описывающий работника.
    /// </summary>
    public struct Worker
    {
        #region Fields

        // Статические поля

        /// <summary>
        /// Перечислитель для идентификации рабочих.
        /// </summary>
        private static int index;

        // Нестатические поля

        /// <summary>
        /// Идентификатор.
        /// </summary>
        private int id;

        /// <summary>
        /// Имя.
        /// </summary>
        private string firstName;

        /// <summary>
        /// Фамилия.
        /// </summary>
        private string lastName;

        /// <summary>
        /// Возраст.
        /// </summary>
        private byte age;

        /// <summary>
        /// Зарплата.
        /// </summary>
        private uint salary;

        /// <summary>
        /// Количество одновременно обслуживащих проектов.
        /// </summary>
        private byte countProjects;

        /// <summary>
        /// Департамент.
        /// </summary>
        private Departament departament;

        #endregion

        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName => this.firstName;

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName => this.lastName;

        /// <summary>
        /// Возраст.
        /// </summary>
        public byte Age => this.age;

        /// <summary>
        /// Зарплата.
        /// </summary>
        public uint Salary =>  this.salary;

        /// <summary>
        /// Количество одновременно обслуживащих проектов.
        /// </summary>
        public byte CountProjects => this.countProjects;

        /// <summary>
        /// Департамент.
        /// </summary>
        public Departament Departament => this.departament;

        #endregion

        #region Constructors

        /// <summary>
        /// Создание рабочего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="age">Возраст.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="countProjects">Количество закрепленных проектов.</param>
        /// <param name="departament">Департамент.</param>
        public Worker(string firstName, string lastName, byte age, 
            uint salary, byte countProjects, Departament departament)
        {
            this.id = ++index;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.salary = salary;
            this.countProjects = countProjects;
            this.departament = departament;
            departament.Add(this);
        }

        /// <summary>
        /// Создание рабочего.
        /// ПРИМЕЧАНИЕ! Используется только для загрузки рабочих из файла.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="age">Возраст.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="countProjects">Количество закрепленных проектов.</param>
        /// <param name="departament">Департамент.</param>
        public Worker(int id, string firstName, string lastName, byte age,
            uint salary, byte countProjects, Departament departament)
        {
            if (id > index)
                index = ++id;
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.salary = salary;
            this.countProjects = countProjects;
            this.departament = departament;
            departament.Add(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Увеличить зарплату на сумму.
        /// </summary>
        /// <param name="amount">Сумма.</param>
        public void SetSalary(uint amount)
        {
            Console.WriteLine($"Вы увеличили сотруднику {this.firstName} {this.LastName} зарплату на: {this.salary - amount}.");
            this.salary = amount;
            Console.WriteLine($"Текущая зарплата сотрудника {this.firstName} {this.LastName}: {this.salary}.");
        }

        /// <summary>
        /// Установить количество проектов.
        /// </summary>
        /// <param name="countProjects">Количество проектов.</param>
        public void SetCountProjects(byte countProjects)
        {
            this.countProjects = countProjects;
            Console.WriteLine($"Текущее количество проектов сотрудника {this.firstName} {this.LastName}: {this.countProjects}.");
        }

        /// <summary>
        /// Изменить отдел.
        /// </summary>
        /// <param name="departaments">Список отделов.</param>
        /// <param name="index">Индекс отдела.</param>
        public void SetDepartament(List<Departament> departaments, int index)
        {
            this.departament.GetListWorkers().Remove(this);
            this.departament = departaments[index];
            departament.Add(this);
        }

        /// <summary>
        /// Сброс счетчика.
        /// </summary>
        public static void Reset() => index = 0;

        #endregion

        #region Override methods

        public override string ToString()
        {
            return $"{id,8} | {firstName,12} | {lastName,12} | {age,8} | {departament.Name,14} | {salary,9:N0} | {countProjects,16}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Worker))
                return false;

            Worker worker = (Worker)obj;

            if (this.id != worker.id)
                return false;

            if (this.firstName != worker.firstName)
                return false;

            if (this.lastName != worker.lastName)
                return false;

            if (!this.Departament.Equals(worker.Departament))
                return false;

            if (this.salary != worker.salary)
                return false;

            if (this.countProjects != worker.countProjects)
                return false;

            return true;
        }

        #endregion
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HomeWork_08
{
    /// <summary>
    /// Сервис для сохранения или загрузки файлов.
    /// </summary>
    public static class Service
    {
        #region SaveToXml

        /// <summary>
        /// Сохранить данные в XML.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        /// <param name="departaments"> Список департаментов. </param>
        public static void SaveDepartamentsToXml(string path, List<Departament> departaments)
        {
            XElement arrDepartaments = new XElement("Departaments");

            foreach (var dep in departaments)
            {
                XElement tmpElem = new XElement("Departament");
                XAttribute name = new XAttribute("name", dep.Name);
                XAttribute date = new XAttribute("date_creation", dep.DateCreation.ToString());
                XElement workers = FillWorkers(dep.GetListWorkers());

                tmpElem.Add(name, date);
                tmpElem.Add(workers);
                arrDepartaments.Add(tmpElem);
            }

            arrDepartaments.Save(path);
        }

        /// <summary>
        /// Заполнить данные рабочих из списка в элемент.
        /// </summary>
        /// <param name="workers">Список рабочих.</param>
        /// <returns> Элемент массива рабочих. </returns>
        private static XElement FillWorkers(List<Worker> workers)
        {
            XElement arrWorkers = new XElement("Workers");

            foreach (var w in workers)
            {
                XElement worker = new XElement("Worker");
                XElement id = new XElement("Id", w.Id);
                XElement firstName = new XElement("FirstName", w.FirstName);
                XElement lastName = new XElement("LastName", w.LastName);
                XElement age = new XElement("Age", w.Age);
                XElement departament = new XElement("Departament", w.Departament.Name);
                XElement salary = new XElement("Salary", w.Salary);
                XElement countProjects = new XElement("CountProjects", w.CountProjects);
                worker.Add(id, firstName, lastName, age, departament, salary, countProjects);
                arrWorkers.Add(worker);
            }

            return arrWorkers;
        }

        /// <summary>
        /// Загрузить список департаментов из файла XML.
        /// </summary>
        /// <param name="path"> Путь к файлу XML. </param>
        /// <returns> Список департаментов. </returns>
        public static List<Departament> LoadDepartamentsFromXml(string path)
        {
            string xml = File.ReadAllText(path);

            var departaments = XDocument.Parse(xml)
                                .Descendants("Departaments")
                                .Elements("Departament")
                                .ToList();

            List<Departament> listDep = new List<Departament>();

            if (departaments.Count == 0)
                return listDep;


            foreach (var dep in departaments)
            {
                var arrWorkers = dep
                                .Element("Workers")
                                .Elements("Worker")
                                .ToList();

                string name = dep.Attribute("name").Value;
                DateTime date = DateTime.Parse(dep.Attribute("date_creation").Value);
                Departament departament = new Departament(name);

                foreach (var w in arrWorkers)
                {
                    int id = int.Parse(w.Element("Id").Value);
                    string firstName = w.Element("FirstName").Value;
                    string lastName = w.Element("LastName").Value;
                    byte age = byte.Parse(w.Element("Age").Value);
                    uint salary = uint.Parse(w.Element("Salary").Value);
                    byte countProjects = byte.Parse(w.Element("CountProjects").Value);
                    departament.Add(new Worker(id, firstName, lastName, age, salary, countProjects, departament));
                }
                
                listDep.Add(departament);
            }

            return listDep;
        }

        #endregion

        #region SaveToJson

        /// <summary>
        /// Сохранить данные в JSON.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="departaments">Список отделов.</param>
        public static void SaveDepartamentsToJson(string path, List<Departament> departaments)
        {
            JArray arrDepartaments = new JArray();

            foreach (var departament in departaments)
            {
                JObject oDepartament = new JObject();
                JArray arrWorkers = new JArray();
                oDepartament["name"] = departament.Name;
                oDepartament["data_creation"] = departament.DateCreation.ToShortDateString();
                var workers = departament.GetListWorkers();

                foreach (var worker in workers)
                {
                    JObject oWorker = new JObject();
                    oWorker["id"] = worker.Id;
                    oWorker["first_name"] = worker.FirstName;
                    oWorker["last_name"] = worker.LastName;
                    oWorker["age"] = worker.Age;
                    oWorker["salary"] = worker.Salary;
                    oWorker["count_projects"] = worker.CountProjects;
                    oWorker["departament"] = worker.Departament.Name;
                    arrWorkers.Add(oWorker);
                }

                oDepartament["workers"] = arrWorkers;

                arrDepartaments.Add(oDepartament);
            }

            File.WriteAllText(path,arrDepartaments.ToString());
        }

        /// <summary>
        /// Загрузить данные из файла JSON.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns></returns>
        public static List<Departament> LoadDepartamentsFromJSON(string path)
        {
            string json = File.ReadAllText(path);

            JArray arrDepartaments = JArray.Parse(json);

            List<Departament> departaments = new List<Departament>();

            if (arrDepartaments.Count == 0)
                return departaments;

            foreach (var departament in arrDepartaments)
            {
                string name = departament["name"].ToString();
                DateTime date = DateTime.Parse(departament["data_creation"].ToString());
                Departament dep = new Departament(name, date);

                var workers = departament["workers"].ToArray();

                foreach (var worker in workers)
                {
                    string firstName = worker["first_name"].ToString();
                    string lastName = worker["last_name"].ToString();
                    byte age = byte.Parse(worker["age"].ToString());
                    uint salary = uint.Parse(worker["salary"].ToString());
                    byte countProjects = byte.Parse(worker["count_projects"].ToString());
                    new Worker(firstName, lastName, age, salary, countProjects, dep);
                }

                departaments.Add(dep);
            }


            return departaments;
        }

        #endregion
    }
}

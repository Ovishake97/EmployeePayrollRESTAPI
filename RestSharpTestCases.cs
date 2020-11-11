using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeJSOnServer
{
    [TestClass]
    public class RestSharpTestCases
    {
        //Initialising a client
        RestClient client;
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000");
        }
        /// Declaring a class to store the data from the json file
        public class Employee {
            public int id { get; set; }
            public string name { get; set; }
            public string salary { get; set; }

            public Employee(int id, string name, string salary)
            {
                this.id = id;
                this.name = name;
                this.salary = salary;
            }
        }
        /// Reading the json file and returning the data
        private IRestResponse GetEmployeeList()
        {
            RestRequest request = new RestRequest("/Employees/list", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        /// validating the status code of the returned data with the expected one 
        /// and matching the count of the returned data with the actual count
        /// UC1
        [TestMethod]
        public void OnCallingGetEmployeeList()
       {
            IRestResponse response = GetEmployeeList();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(3, employeeList.Count);
            foreach (Employee emp in employeeList)
            {
                Console.WriteLine(emp.id + "\t" + emp.name + "\t" + emp.salary);
            }
        }
        /// On calling the post api and passing a json object
        /// the employee is added and validated with the obtained data and status code
        /// UC2
        [TestMethod]
        public void OnCallingPostAddsEmployee() {
            RestRequest request = new RestRequest("/Employees/list", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("id", 4);
            jsonObj.Add("name", "Robert");
            jsonObj.Add("salary", "30000");
            //Adding a parameter of the header type "application/json" 
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Robert", employee.name);
            Assert.AreEqual("30000", employee.salary);
        }
        /// On calling post API adds a multiple employees to the file
        /// and validates the count of the newly added data
        /// UC3
        [TestMethod]
        public void OnCallingPostAddsMultipleEmployee() {
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee(5, "Manish", "30928"));
            employeeList.Add(new Employee(6, "Sam", "10000"));
            employeeList.Add(new Employee(7, "Christie", "77185"));
            List<Employee> result = new List<Employee>();
            employeeList.ForEach(employee =>
            {
                RestRequest request = new RestRequest("/Employees/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("id", employee.id);
                jsonObj.Add("name", employee.name);
                jsonObj.Add("salary", employee.salary);
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Employee employeeData = JsonConvert.DeserializeObject<Employee>(response.Content);
                result.Add(employeeData);
            }
            );
            
            Assert.AreEqual(3, result.Count);
        }
    }
}

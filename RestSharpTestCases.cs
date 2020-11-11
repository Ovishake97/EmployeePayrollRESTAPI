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
    }
}

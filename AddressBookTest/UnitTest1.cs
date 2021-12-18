using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RestSharp;
using System.Net;
using System.Collections.Generic;
using AddressBook_RestSharp;
using Newtonsoft.Json;

namespace AddressBookTest
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient(" http://localhost:4000 ");
        }
        private IRestResponse GetAddressBook()
        {
            RestRequest request = new RestRequest("AddressBookJson", Method.GET);

            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void OnCallingGetAPI_ReturnAddressBookList()
        {
            IRestResponse response = GetAddressBook();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            List<AddressBook> AddressBookList = JsonConvert.DeserializeObject<List<AddressBook>>(response.Content);
            Assert.AreEqual(3, AddressBookList.Count);

            foreach (AddressBook addressBook in AddressBookList)
            {
                Console.WriteLine("id : " + addressBook.id + "\t" + "FirstName : " + addressBook.FirstName +
                    "\t" + "LastName : " + addressBook.LastName + "\t" + "Address : " + addressBook.Address +
                    "\t" + "City : " + addressBook.City + "\t" + "State : " + addressBook.State +
                    "\t" + "ZipCode : " + addressBook.ZipCode + "\t" + "PhoneNumber : " + addressBook.PhoneNumber +
                    "\t" + "Email : " + addressBook.Email);
            }
        }

        [TestMethod]
        public void OnCallingPostAPI_ReturnAddressBookObject()
        {
            RestRequest request = new RestRequest("/AddressBookJson", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("id", 4);
            jsonObj.Add("FirstName", "samir");
            jsonObj.Add("LastName", "Chand");
            jsonObj.Add("Address", "A");
            jsonObj.Add("City", "C");
            jsonObj.Add("State", "M");
            jsonObj.Add("Zipcode", "425477");
            jsonObj.Add("PhoneNumber", "4872392");
            jsonObj.Add("Email", "samir1@gmail.com");

            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            AddressBook address = JsonConvert.DeserializeObject<AddressBook>(response.Content);
            Assert.AreEqual("samir", address.FirstName);
            Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void OnCallingPostAPIForAddressBookListWithMultipleAddress_ReturnEmployeeObject()
        {
            List<AddressBook> addressBooks = new List<AddressBook>();
            addressBooks.Add(new AddressBook { FirstName = "golu", LastName = "lad", Address = "Street ABC", City = "Allahabad", State = "UttarPradesh", ZipCode = "239292", PhoneNumber = "8798979898", Email = "Oqqsj@gmail.com" });

            foreach (var addressBook in addressBooks)
            {
                RestRequest request = new RestRequest("/AddressBookJson", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("FirstName", addressBook.FirstName);
                jsonObj.Add("LastName", addressBook.LastName);
                jsonObj.Add("Address", addressBook.Address);
                jsonObj.Add("City", addressBook.City);
                jsonObj.Add("State", addressBook.State);
                jsonObj.Add("ZipCode", addressBook.ZipCode);
                jsonObj.Add("PhoneNumber", addressBook.PhoneNumber);
                jsonObj.Add("Email", addressBook.Email);
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                AddressBook addressbook = JsonConvert.DeserializeObject<AddressBook>(response.Content);
                Assert.AreEqual(addressBook.FirstName, addressbook.FirstName);
                Assert.AreEqual(addressBook.LastName, addressbook.LastName);
                Console.WriteLine(response.Content);
            }

        }

        [TestMethod]
        public void OnCallingPutAPI_ReturnContactObjects()
        {
            RestRequest request = new RestRequest("/AddressBookJson/2", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("FirstName", "Shikhar");
            jsonObj.Add("LastName", "Dhawan");
            jsonObj.Add("PhoneNumber", "7858070934");
            jsonObj.Add("Address", "indian cricket");
            jsonObj.Add("City", "delhi");
            jsonObj.Add("State", "Inida");
            jsonObj.Add("ZipCode", "355888");
            jsonObj.Add("Email", "sr7@gmail.com");

            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AddressBook address = JsonConvert.DeserializeObject<AddressBook>(response.Content);
            Assert.AreEqual("Shikhar", address.FirstName);
            Assert.AreEqual("Dhawan", address.LastName);
            Assert.AreEqual("355888", address.ZipCode);
            Console.WriteLine(response.Content);
        }
        
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            RestRequest request = new RestRequest("/AddressBookJson/5", Method.DELETE);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }


}

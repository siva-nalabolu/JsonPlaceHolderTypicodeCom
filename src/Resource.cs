using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JsonPlaceHolderTypicodeCom
{

    public class Tests
    {
        HttpClient client = new HttpClient();
        [SetUp]
        public void Setup()
        {
          
        }

        [TearDown]
        public void AfterTest()
        {
            _ = new HttpClient();
        }
        
        public class MyArray
        {
            public string userId { get; set; }
            public string id { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }

        [Test, Category("Get")]
        public async Task GetAllResources()
        {
            try
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
                var resp = response.Content.ReadAsStringAsync().Result.ToString();
                List<MyArray> myDeserializedClass = JsonConvert.DeserializeObject<List<MyArray>>(resp);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                if (myDeserializedClass.Count.Equals(100))
                {
                    foreach (var item in myDeserializedClass)
                    {
                        Assert.IsNotNull(item.id);
                        Assert.IsNotNull(item.userId);
                        Assert.IsNotNull(item.title);
                        Assert.IsNotNull(item.body);
                    }
                }
                else
                {
                    Assert.Fail("The API  returned more that 100 items");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Test Failed");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [TestCase("1", Category ="Get")]
        public async Task GetResourseByID(string input)
        {
            try
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/" + input);
                var resp = JsonConvert.DeserializeObject<MyArray>(response.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
                Assert.IsNotNull(resp.id);
                Assert.IsNotNull(resp.body);
                Assert.IsNotNull(resp.title);
                Assert.IsNotNull(resp.userId);
            }catch(Exception ex)
            {
                Assert.Fail("Test Failed" + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
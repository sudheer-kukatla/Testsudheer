using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Test
{   
    [TestClass]
    public class BonusPoolControllerTest
    {
        string url = "https://localhost:5001/api/BonusPool";
        [TestMethod]
        public async Task GetEmployeesTest()
        {

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<ICollection<Employee>>(responseBody);
                Assert.AreEqual(jsonData.Count, 12);
            }

        }
        [TestMethod]
        public async Task GetEmployeesBonusForInvalidSelectedEmployeeIdTest()
        {
            using (var client = new HttpClient())
            {
                var calculateBonusDto = new CalculateBonusDto { SelectedEmployeeId = 0 };
                StringContent httpConent = new StringContent(JsonConvert.SerializeObject(calculateBonusDto), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, httpConent);
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.AreEqual("Empoyee id not found, please provide valid empoyee id", responseBody);
            }
        }

        [TestMethod]
        public async Task GetEmployeesBonusForValidSelectedEmployeeIdTest()
        {
            using (var client = new HttpClient())
            {
                var calculateBonusDto = new CalculateBonusDto
                {
                    SelectedEmployeeId = 1,
                    TotalBonusPoolAmount = 1000
                };
                StringContent httpConent = new StringContent(JsonConvert.SerializeObject(calculateBonusDto), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, httpConent);
                string responseBody = await response.Content.ReadAsStringAsync();
                var bonusPoolCalculatorResultDto = JsonConvert.DeserializeObject<BonusPoolCalculatorResultDto>(responseBody);
                Assert.AreEqual(91, bonusPoolCalculatorResultDto.Amount);
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClimateWebAPI.Controllers;
using ClimateWebAPI.Models;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Net;

namespace WebApiTest
{
    [TestClass]
    public class UnitTestClimateAPI
    {
        [TestMethod]
        public void GetRecommendation_ShouldBeOK()
        {
 
            var controller = new ClimateController();
            var response = controller.Get("2886242") ;
            var contentResult = response as OkNegotiatedContentResult<RecommendationResponse>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void GetRecommendation_InvalidCityCode()
        {
            var controller = new ClimateController();
            IHttpActionResult response = controller.Get("123");
            var contentResult = response as OkNegotiatedContentResult<RecommendationResponse>;
            Assert.IsNull(contentResult);
          }

        [TestMethod]
        public void GetRecommendation_MockData_Valid()
        {
            var controller = new ClimateController();
            var response = controller.Get("101");
            var contentResult = response as OkNegotiatedContentResult<RecommendationResponse>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Temparature, 22);
        }
    }
}

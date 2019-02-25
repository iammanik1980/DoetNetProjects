using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClimateWebAPI.Models;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using System.Configuration;

namespace ClimateWebAPI.Controllers
{
    public class ClimateController : ApiController
    {
        
       public IHttpActionResult Get(string cityCode)
        {
            string level = string.Empty;
            int temparature;

            // checking the city code , if the city code between 100 to 104 then mock temarature to be returned
            // otherwise temparature to be fetched from api.openweathermap.org
            var list = new List<string>() { "100", "101", "102", "103", "104" };



            if (list.Contains(cityCode))
                temparature = GetMockTemparature(cityCode); // getting mock data
            else
                try
                {
                    temparature = GetTemparature(cityCode); // getting data from api.openweathermap.org API
                }
                catch
                {
                    //return NotFound();
                    return Content(HttpStatusCode.NotFound, string.Format("No data found for city code : {0}",cityCode));
                }
                

            if (temparature >= 26)
                level = "Level 1";
            else if (temparature > 21 && temparature <= 26)
                level = "Level 2";
            else if (temparature > 15 && temparature <= 21)
                level = "Level 3";
            else if (temparature > 5 && temparature <= 15)
                level = "Level 4";
            else if (temparature <= 5)
                level = "Level 5";

            RecommendationResponse recommendation = new RecommendationResponse() { Temparature = temparature, Level = level };

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, recommendation, "application/json");
            return Ok(recommendation);

        }

        /// <summary>
        /// This method is getting actual temparature in Fahrenheit from api.openweathermap.org 
        /// for the citycode passed as parameter. Then the temparature is converted into celsius as per the requirement.
        /// </summary>
        /// <param name="cityCode"> city code for which the data to be fetched </param>
        /// <returns></returns>
        private int GetTemparature(string cityCode )
        {
           
            // sample URL : "http://api.openweathermap.org/data/2.5/weather?id=2892794&APPID=f85e60e31ed7bf2c09309057f31b8dd8"
            double tempInFahrenheit = 0.00;
            Int32 tempIncelsius = 0;

                string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
                string thirdPartyAppId = ConfigurationManager.AppSettings["thirdPartyAppId"];
                string sCompleteUrl = "http://" + baseUrl + cityCode + "&APPID=" + thirdPartyAppId;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sCompleteUrl);
                request.Method = "GET";

                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    Stream newStream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(newStream);
                    var result = sr.ReadToEnd();
                    RootObject root = JsonConvert.DeserializeObject<RootObject>(result);
                    if(root != null && root.weather != null)
                    {
                        tempInFahrenheit = root.main.temp;
                        /*
                         conversion rule
                         C/5 = (F-32)/9
                         
                         */
                        tempIncelsius = Convert.ToInt32(((tempInFahrenheit - 32) / 9) * 5);
                    }
                }
            return tempIncelsius;
        }

        /// <summary>
        ///  this method to return to return some mock data to test different cases for our requirement
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        private int GetMockTemparature(string cityCode)
        {
            int mockTemparature =0;
            // this is mock list of city code and temparature. This is to use to test all the possible cases.
            Dictionary<string, int> pairs = new Dictionary<string, int>();
            pairs.Add("100", 30);
            pairs.Add("101", 22);
            pairs.Add("102", 16);
            pairs.Add("103", 6);
            pairs.Add("104", 1);

            if (pairs.ContainsKey(cityCode))
                mockTemparature = pairs[cityCode];

            return mockTemparature;
        }

      }
}

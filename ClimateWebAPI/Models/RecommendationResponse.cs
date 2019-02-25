using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClimateWebAPI.Models
{
    public class RecommendationResponse
    {
        public int Temparature { get; set; }
        public string Level { get; set; }
    }
}
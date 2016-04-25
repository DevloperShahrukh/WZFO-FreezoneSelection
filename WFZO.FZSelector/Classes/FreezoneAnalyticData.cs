using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFZO.FZSelector.Classes
{
    [Serializable]
    public class FreezoneAnalyticData
    {
        public int RegionId { get; set; }
        public int FreezoneId { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
    }


    [Serializable]
    public class SubCatIdsWithWeight
    {
        public string SubCatIds { get; set; }
        public int Weight { get; set; }
    }
}

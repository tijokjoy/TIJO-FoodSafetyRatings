using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UKFoodStandardsAgencyRating.BusinessObjects;

namespace UKFoodStandardsAgencyRating.DataAccess
{
    public class APIClient:IAPIClient
    {
        string _baseAPIURL;
        string _apiVersion;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        
        public APIClient()
        {
            _baseAPIURL = @"http://api.ratings.food.gov.uk/"; // TODO: can be called from a config file
            _apiVersion = "2";
          

        }

        /// <summary>
        /// Generic function for GET API requests
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<string> GETApiFunc(string path)
        {
            try
            {
                logger.Info("TTT");
                    var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-version", _apiVersion);
                var response = await client.GetAsync(_baseAPIURL + path);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) {
                logger.Error(ex, ex.Message);
                return null;
            }

        }


        /// <summary>
        /// Fecth the list of Authorities from FSA API
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthorities()
        {
            try
            {
                
              string path = @"Authorities/Basic";
                var data = await GETApiFunc(path);
                var authorities = JsonConvert.DeserializeObject<Authorities>(data).authorities
                    .Select(x => new { x.Name, x.LocalAuthorityId, x.LocalAuthorityIdCode, x.EstablishmentCount }); ;

                return JsonConvert.SerializeObject(authorities);
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                return null;
            }
            

        }


        /// <summary>
        /// Fetch Establishment details from FSA API and calculate the percentage of ratings
        /// </summary>
        /// <param name="localAuthorityId"></param>
        /// <returns></returns>
        public async Task<string> GetEstablishmentRatings(string localAuthorityId)
        {
            try
            {
                string path = @"Establishments?name=&address=&longitude=&latitude=&maxDistanceLimit=&businessTypeId=&schemeTypeKey=&ratingKey=&ratingOperatorKey=&localAuthorityId="+localAuthorityId+@"&countryId=&sortOptionKey=&pageNumber=&pageSize=";
                var data = await GETApiFunc(path);
                var establishments = JsonConvert.DeserializeObject<Establishments>(data).establishments;
                var totalCount = establishments.Count;
                var ratingOverview = new List<RatingOverview>();
                foreach(var ratingGroup in establishments.GroupBy(x => x.RatingValue))
                {                    
                    var percentage = Math.Round(((double)ratingGroup.Count()/ (double)totalCount) * 100,1);
                    ratingOverview.Add(new RatingOverview { Rating = ratingGroup.Key, Percentage =percentage });

                }
                return JsonConvert.SerializeObject(ratingOverview);

            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                return null;
            }
        }
    }
}

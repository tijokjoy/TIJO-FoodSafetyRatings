using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UKFoodStandardsAgencyRating.DataAccess;

namespace UKFoodStandardsAgencyRating.Controllers
{
    [Route("api/[controller]")]
    public class FSADataController : Controller
    {
        private readonly ILogger<FSADataController> _logger;

        private IAPIClient _ApiClient;
        public FSADataController(IAPIClient apiClient, ILogger<FSADataController> logger) //Dependecy Injection
        {
            _ApiClient = apiClient;
            _logger = logger;

        }      
        
        /// <summary>
        /// Controller action to return Authorties
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<string> GetAuthorties()
        {
            _logger.LogInformation("GetAuthorties");
            return await _ApiClient.GetAuthorities();
        }

        /// <summary>
        /// Controller action to return the rating overview for the input localauthorityID
        /// </summary>
        /// <param name="localAuthorityId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<string> GetEstablishmentRatings(string localAuthorityId)
        {
            _logger.LogInformation("GetEstablishmentRatings");
            return await _ApiClient.GetEstablishmentRatings(localAuthorityId);
        }




        
    }
}

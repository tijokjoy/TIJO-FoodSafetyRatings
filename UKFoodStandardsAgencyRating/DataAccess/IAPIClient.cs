using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UKFoodStandardsAgencyRating.DataAccess
{
    
 /// <summary>
/// Interface for dependency Injection
/// </summary>
    public interface IAPIClient
    {
        Task<string> GetAuthorities();
        Task<string> GetEstablishmentRatings(string localAuthorityId);
        
    }
}

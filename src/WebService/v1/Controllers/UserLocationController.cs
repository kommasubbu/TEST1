using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Exceptions;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Helper;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Repository;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Filters;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Controllers
{
    [Route(Version.PATH + "/[controller]"), TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class UserLocationController : Controller
    {
        private readonly IUserLocationsRepository _userlocationService;

        public UserLocationController(IUserLocationsRepository userlocationService)
        {
            this._userlocationService = userlocationService;
        }
        [HttpGet()]
        [Route("GetAllAsync")]
        public async Task<UserLocationListApiModel> GetAllAsync()
        {
            return new UserLocationListApiModel(await this._userlocationService.GetAllAsync());
        }


        [HttpGet()]
        [Route("GetAllLocationsByUser")]
        public async Task<UserLocationListApiModel> GetAllLocationsByUser([FromQuery] string From,
            [FromQuery] string To,
            [FromQuery] int UserId,
             [FromQuery] int TenantId
            )
        {
            DateTimeOffset fromDate = DateHelper.ParseDate(From);
            DateTimeOffset toDate = DateHelper.ParseDate(To);
            return new UserLocationListApiModel(await this._userlocationService.GetAllLocationsByUser(fromDate, toDate, UserId, TenantId));
        }
        [HttpGet]
        public async Task<UserLocationListApiModel> ListAsync(
         [FromQuery] string From,
          [FromQuery] string To,
           [FromQuery] string order,
           [FromQuery] int? skip,
           [FromQuery] int? limit,
            [FromQuery] int TenantId,
           [FromQuery] bool? includeDeleted)
        {
            if (order == null) order = "asc";
            if (skip == null) skip = 0;
            if (limit == null) limit = 1000;
            if (includeDeleted == null) includeDeleted = false;
            DateTime Fromdate = DateTime.Parse(From, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime Todate = DateTime.Parse(To, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return new UserLocationListApiModel(
                await this._userlocationService.GetListAsync(
                    Fromdate,
                    Todate,
                    order,
                    skip.Value,
                    limit.Value,
                    TenantId)
               );
        }

        [HttpPost]
        public async Task<UserLocationApiModel> PostAsync(
           [FromBody] UserLocationApiModel location)
        {

            if (location == null)
            {
                throw new InvalidInputException("location not provided in request body.");
            }
            UserLocationServiceModel _interactions = await this._userlocationService.CreateAsync(location.ToServiceModel());

            return new UserLocationApiModel(_interactions);
        }
        [HttpPut("{id}")]
        public async Task<UserLocationApiModel> PutAsync(
             [FromRoute] string id,
             [FromBody] UserLocationApiModel location)
        {
            if (location == null)
            {
                throw new InvalidInputException("document not provided in request body.");
            }


            UserLocationServiceModel updatedinteraction = await this._userlocationService.UpsertIfNotDeletedAsync(id, location.ToServiceModel());

            return new UserLocationApiModel(updatedinteraction);
        }


        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] string id)
        {
            await this._userlocationService.DeleteAsync(id);
        }
    }
}

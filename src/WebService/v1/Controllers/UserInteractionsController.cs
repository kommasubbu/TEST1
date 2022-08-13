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
    public class UserInteractionsController : Controller
    {
        private readonly IUserInteractionsRepository _userInteractionsRepository;

        public UserInteractionsController(IUserInteractionsRepository userInteractionsRepository)
        {
            this._userInteractionsRepository = userInteractionsRepository;
        }
        [HttpGet("{id}")]
        public async Task<UserInteractionApiModel> GetAsync([FromRoute] string id)
        {
            UserInteractionsServiceModel interaction = await this._userInteractionsRepository.GetAsync(id);
            return new UserInteractionApiModel(interaction);
        }
        [HttpGet()]
        [Route("GetAllAsync")]
        public async Task<UserInteractionsListApiModel> GetAllAsync()
        {
            return new UserInteractionsListApiModel(await this._userInteractionsRepository.GetAllAsync());
        }


        [HttpPost()]
        [Route("GetUserViolationCount")]
        public async Task<int> GetUserViolationCount([FromQuery] string From,
            [FromQuery] string To,
            [FromQuery] int UserId,
             [FromQuery] int TenantId
            )
        {
            DateTimeOffset fromDate = DateHelper.ParseDate(From);
            DateTimeOffset toDate = DateHelper.ParseDate(To);
            return await this._userInteractionsRepository.GetUserViolationCount(fromDate, toDate, UserId, TenantId);
        }


        [HttpGet()]
        [Route("GetAllInteractionByUser")]
        public async Task<UserInteractionsListApiModel> GetAllInteractionByUser([FromQuery] string From,
           [FromQuery] string To,
           [FromQuery] int UserId,
            [FromQuery] int TenantId
           )
        {
            DateTimeOffset fromDate = DateHelper.ParseDate(From);
            DateTimeOffset toDate = DateHelper.ParseDate(To);
            return new UserInteractionsListApiModel(await this._userInteractionsRepository.GetAllInteractionByUser(fromDate, toDate, UserId, TenantId));
        }
        [HttpGet]
        public async Task<UserInteractionsListApiModel> ListAsync(
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
            return new UserInteractionsListApiModel(
                await this._userInteractionsRepository.GetListAsync(
                    Fromdate,
                    Todate,
                    order,
                    skip.Value,
                    limit.Value,
                    TenantId)
               );
        }


        [HttpPost]
        public async Task<UserInteractionApiModel> PostAsync(
         [FromBody] UserInteractionApiModel interaction)
        {

            if (interaction == null)
            {
                throw new InvalidInputException("interaction not provided in request body.");
            }
            UserInteractionsServiceModel _interactions = await this._userInteractionsRepository.CreateAsync(interaction.ToServiceModel());

            return new UserInteractionApiModel(_interactions);
        }

        [HttpPut("{id}")]
        public async Task<UserInteractionApiModel> PutAsync(
            [FromRoute] string id,
            [FromBody] UserInteractionApiModel document)
        {
            if (document == null)
            {
                throw new InvalidInputException("document not provided in request body.");
            }


            UserInteractionsServiceModel updatedinteraction = await this._userInteractionsRepository.UpsertIfNotDeletedAsync(id, document.ToServiceModel());

            return new UserInteractionApiModel(updatedinteraction);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] string id)
        {
            await this._userInteractionsRepository.DeleteAsync(id);
        }
    }
}

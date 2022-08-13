using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Exceptions;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Models;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Repository;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Exceptions;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Filters;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.v1.Controllers
{
    [Route(Version.PATH), TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ValuesController : Controller
    {
        private readonly IKeyValueContainer container;
        public ValuesController(
           IKeyValueContainer container
          )
        {
            this.container = container;           
        }
        [HttpGet("collections/{collectionId}/values/{key}")]
        public async Task<ValueApiModel> Get(string collectionId, string key)
        {
            this.EnsureValidId(collectionId);

            var result = await this.container.GetAsync(collectionId, key);

            return new ValueApiModel(result);
        }

        [HttpGet("collections/{collectionId}/values/getall/{tenantId}")]
        public async Task<ValueListApiModel> Get(string collectionId,int tenantId)
        {
            this.EnsureValidId(collectionId);

            var result = await this.container.GetAllAsync(collectionId, tenantId);

            return new ValueListApiModel(result, collectionId);
        }

        [HttpPost("collections/{collectionId}/values")]
        public async Task<ValueApiModel> Post(string collectionId, [FromBody] ValueApiModel model)
        {
            if (model == null)
            {
                throw new InvalidInputException("The request is empty");
            }

            
            this.EnsureValidId(collectionId);

            var result = await this.container.CreateAsync(collectionId, model.ToServiceModel());

            return new ValueApiModel(result);
        }



        [HttpPost("collections/{collectionId}/values/bulkinsert")]
        public async Task<bool> Post(string collectionId, [FromBody] List<ValueApiModel> models)
        {
            if (models.Count < 0)
            {
                throw new InvalidInputException("The request is empty");
            }


            this.EnsureValidId(collectionId);

            foreach (var model in models)
            {
                var result = await this.container.CreateAsync(collectionId, model.ToServiceModel());
            }

            return true;
        }

        [HttpPut("collections/{collectionId}/values/{key}")]
        public async Task<ValueApiModel> Put(string collectionId, string key, [FromBody] ValueApiModel model)
        {
            if (model == null)
            {
                throw new InvalidInputException("The request is empty");
            }

            this.EnsureValidId(collectionId);

            var result = model.key == null ? await this.container.CreateAsync(collectionId, model.ToServiceModel()) : await this.container.UpsertAsync(collectionId, key, model.ToServiceModel());

            return new ValueApiModel(result);
        }

        [HttpDelete("collections/{collectionId}/values/{key}")]
        public async Task Delete(string collectionId, string key)
        {
            this.EnsureValidId(collectionId);

            await this.container.DeleteAsync(collectionId, key);
        }


        [HttpPost("collections/{collectionId}/values/bulkdelete")]
        public async Task<bool> DeleteMany(string collectionId, [FromBody] queryApiModel json)
        {
            this.EnsureValidId(collectionId);

           return await this.container.DeleteAsyncWithQuery(collectionId, json.query);
        }
        private void EnsureValidId(string collectionId)
        {
            // Currently, there is no official document describing valid character set of document ID
            // We just verified and enabled characters below
            var validCharacters = "_-";

            if (!collectionId.All(c => char.IsLetterOrDigit(c) || validCharacters.Contains(c)))
            {
                var message = $"Invalid collectionId: '{collectionId}'";
                throw new BadRequestException(message);
            }

           
        }

    }
}

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
    public class DocumentsController : Controller
    {
        private readonly IDocumentsRepository _documentService;

        public DocumentsController(IDocumentsRepository documentService)
        {
            this._documentService = documentService;
        }
        [HttpGet("{id}")]
        public async Task<DocumentsApiModel> GetAsync([FromRoute] string id)
        {
            DocumentsServiceModel document = await this._documentService.GetAsync(id);
            return new DocumentsApiModel(document);
        }
        [HttpGet()]
        [Route("GetAllAsync")]
        public async Task<DocumentsListApiModel> GetAllAsync()
        {
            return new DocumentsListApiModel(await this._documentService.GetAllAsync() );
        }

        [HttpGet()]
        [Route("GetAllDocumnetsByUser")]
        public async Task<DocumentsListApiModel> GetAllDocumnetsByUser([FromQuery] string From,
        [FromQuery] string To,
        [FromQuery] int UserId,
         [FromQuery] int TenantId
        )
        {
            DateTimeOffset fromDate = DateHelper.ParseDate(From);
            DateTimeOffset toDate = DateHelper.ParseDate(To);
            return new DocumentsListApiModel(await this._documentService.GetAllDocumnetsByUser(fromDate, toDate, UserId, TenantId));
        }
        [HttpGet]      
        public async Task<DocumentsListApiModel> ListAsync(
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
            DateTime ToDate = DateTime.Parse(To, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return new DocumentsListApiModel(
                await this._documentService.GetListAsync(
                    Fromdate,
                    ToDate,
                    order,
                    skip.Value,
                    limit.Value,
                    TenantId)
               );
        }


        [HttpPost]      
        public async Task<DocumentsApiModel> PostAsync(       
         [FromBody] DocumentsApiModel document)
        {
            
            if (document == null)
            {
                throw new InvalidInputException("document not provided in request body.");
            }
            DocumentsServiceModel _doc = await this._documentService.CreateAsync(document.ToServiceModel());

            return new DocumentsApiModel(_doc);
        }

        [HttpPut("{id}")]
        public async Task<DocumentsApiModel> PutAsync(
            [FromRoute] string id,
            [FromBody] DocumentsApiModel document)
        {
            if (document == null)
            {
                throw new InvalidInputException("document not provided in request body.");
            }

           
            DocumentsServiceModel updatedDoc = await this._documentService.UpsertIfNotDeletedAsync(id, document.ToServiceModel());

            return new DocumentsApiModel(updatedDoc);
        }

        [HttpDelete("{id}")]      
        public async Task DeleteAsync([FromRoute] string id)
        {
            await this._documentService.DeleteAsync(id);
        }
    }
}

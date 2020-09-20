using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Interception.Function.Services;
using System;
using System.Web.Http;

namespace Interception.Function
{
    public class Function
    {
        private readonly ILogger<Function> logger;
        private readonly IServiceSample service;

        public Function(ILogger<Function> logger, IServiceSample service)
        {
            this.logger = logger;
            this.service = service;
        }

        [FunctionName("Function")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "models/{id}")] HttpRequest request, int id)
        {
            try
            {
                var model = await this.service.GetModel(id);

                return model == null
                    ? new NotFoundResult() as IActionResult
                    : new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResult();
            }
        }
    }
}

using InitialDatasetService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InitialDatasetService.Controllers
{
    [ApiController]
    [Route("/api1")]
    public class OntologyController : ControllerBase
    {
        private readonly IDatasetInitialization _datasetInitialization;
        public OntologyController(IDatasetInitialization datasetInitialization)
        {
            _datasetInitialization = datasetInitialization;
        }
        [HttpGet]
        public void Get()
        {
            _datasetInitialization.InitializeDataset();
        }
    }
}

using InitialDatasetService.Interfaces;
using InitialDatasetService.MicroServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QueryBuilderLibrary.Implementations;

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

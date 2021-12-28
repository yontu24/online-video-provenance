using InitialDatasetService.MicroServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InitialDatasetService.Controllers
{
    [ApiController]
    [Route("/api1")]
    public class OntologyController : ControllerBase
    {
        [HttpGet]
        public void Get()
        {
            OntologyInitializationService.initializeOntology();
        }
    }
}

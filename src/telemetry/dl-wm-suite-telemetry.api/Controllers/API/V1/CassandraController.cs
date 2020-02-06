using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.telemetry.api.Controllers.API.V1
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiVersionNeutral]
    [ApiController]
    public class CassandraController : ControllerBase
    {
        private readonly IDeviceCassandraRepository _cassandraRepository;

        public CassandraController(IDeviceCassandraRepository cassandraRepository)
        {
            _cassandraRepository = cassandraRepository;
        }

        [Route("Initializer", Name = "PostCassandraInitializerRoot")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostCassandraInitializerAsync()
        {
            await _cassandraRepository.CreateKeySpaceDevice();
            await _cassandraRepository.CreateTableDevice();
            return Ok();
        }
    }
}

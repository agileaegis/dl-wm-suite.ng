using System;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using dl.wm.suite.telemetry.api.Validators;
using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.telemetry.api.Controllers.API.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize]
    public class TelemetriesController : ControllerBase
    {
        private readonly IDeviceCassandraRepository _deviceCassandraRepository;
        private readonly ITelemetryRowCassandraRepository _telemetryRowCassandraRepository;

        public TelemetriesController(IDeviceCassandraRepository deviceCassandraRepository,  ITelemetryRowCassandraRepository telemetryRowCassandraRepository)
        {
            _deviceCassandraRepository = deviceCassandraRepository;
            _telemetryRowCassandraRepository = telemetryRowCassandraRepository;
        }

        /// <summary>
        /// Get - Retrieve Stored Device providing Telemetry Id
        /// </summary>
        /// <param name="id">Telemetry Id the Request Index for Retrieval</param>
        /// <remarks>Retrieve Telemetry providing Id</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="204">Undocumented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetTelemetryRowCassandraRoot")]
        public async Task<IActionResult> GetTelemetryRowCassandraAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var telemetryRowFromRepo = _telemetryRowCassandraRepository.Get(id);

            return Ok(telemetryRowFromRepo);
        }

        /// <summary>
        /// Get All - Retrieve Stored telemetry Rows
        /// </summary>
        /// <remarks>Retrieve All Telemetry Rows</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="204">Undocumented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet(Name = "GetAllTelemetryRowCassandraRoot")]
        public async Task<IActionResult> GetAllTelemetryRowCassandraAsync()
        {
            return Ok(_telemetryRowCassandraRepository.GetAll());
        }


        /// <summary>
        /// POST : Add a New Telemetry Row.
        /// </summary>
        /// <param name="telemetryRowForCreationModel">telemetryRowForCreationModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Vehicle is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Device is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{imei}", Name = "PostTelemetryRowDataRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostTelemetryRowDataAsync(string imei, [FromBody] TelemetryRowForCreationModel telemetryRowForCreationModel)
        {
            try
            {
                var telemetryRowToBeStored = new TelemetryRow()
                {
                    Id = Guid.NewGuid().ToString(),
                    Imei = imei,
                    CreatedDate = DateTime.Now,
                    CreatedDateUtc = DateTime.UtcNow,
                    CommandType = telemetryRowForCreationModel.TelemetryRowCommandType,
                    Version = telemetryRowForCreationModel.TelemetryRowHeaderVersion,
                    Timestamp = telemetryRowForCreationModel.TelemetryRowTimestamp,
                    Battery = telemetryRowForCreationModel.TelemetryRowBattery,
                    Temperature = telemetryRowForCreationModel.TelemetryRowTemperature,
                    Distance = telemetryRowForCreationModel.TelemetryRowDistance,
                    FillLevel = telemetryRowForCreationModel.TelemetryRowFillLevel,
                    Latitude = telemetryRowForCreationModel.TelemetryRowLatitude,
                    Longitude = telemetryRowForCreationModel.TelemetryRowLongitude,
                    Altitude = telemetryRowForCreationModel.TelemetryRowAltitude,
                    Speed = telemetryRowForCreationModel.TelemetryRowSpeed,
                    Course = telemetryRowForCreationModel.TelemetryRowCourse,
                    NumOfSatellites = telemetryRowForCreationModel.TelemetryRowNumOfSatellites,
                    TimeToFix = telemetryRowForCreationModel.TelemetryRowTimeToFix,
                    SignalLength = telemetryRowForCreationModel.TelemetryRowSignalLength,
                    StatusFlags = telemetryRowForCreationModel.TelemetryRowStatusFlags,
                    LatestResetCause = telemetryRowForCreationModel.TelemetryRowLatestResetCause,
                    FirmwareVersion = telemetryRowForCreationModel.FirmwareVersion
                };  

                _telemetryRowCassandraRepository.Save(telemetryRowToBeStored.Id, telemetryRowToBeStored);

                return Ok(_telemetryRowCassandraRepository.Get(telemetryRowToBeStored.Id));
            }
            catch (Exception e)
            {
                return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_DEVICE"});
            }
        }
    }
}

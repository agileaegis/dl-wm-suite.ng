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
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceCassandraRepository _cassandraRepository;

        public DevicesController(IDeviceCassandraRepository cassandraRepository)
        {
            _cassandraRepository = cassandraRepository;
        }

        /// <summary>
        /// Get - Retrieve Stored Device providing Device Id
        /// </summary>
        /// <param name="id">Device Id the Request Index for Retrieval</param>
        /// <remarks>Retrieve Devices providing Id</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="204">Undocumented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetDeviceCassandraRoot")]
        public async Task<IActionResult> GetDeviceCassandraAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var deviceFromRepo = await _cassandraRepository.GetSingleDevice(id);


            return Ok(deviceFromRepo);
        }

        /// <summary>
        /// Get - Retrieve All Stored Device providing Devices
        /// </summary>
        /// <remarks>Retrieve all Devices</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="204">Undocumented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet(Name = "GetAllDeviceCassandraRoot")]
        public async Task<IActionResult> GetAllDeviceCassandraAsync()
        {
            return Ok(await _cassandraRepository.GetAllDevices());
        }


        /// <summary>
        /// POST : Provisioning a New Device.
        /// </summary>
        /// <param name="deviceForProvisioningModel">deviceForProvisioningModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Vehicle is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Device is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostDeviceProvisioningRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostDeviceProvisioningAsync(
            [FromBody] DeviceForProvisioningModel deviceForProvisioningModel)
        {
            try
            {
                var deviceCreatedFromRepo = await _cassandraRepository.AddDevice(new Device()
                {
                    Imei = deviceForProvisioningModel.DeviceForProvisioningImei,
                    FirmwareVer = deviceForProvisioningModel.DeviceForProvisioningFirmwareVersion,
                    IsActivated = false,
                    IsEnabled = false,
                    ProvisioningDate = DateTime.UtcNow,
                });
                return Created("PostDeviceProvisioningRouteAsync",
                    deviceCreatedFromRepo);
            }
            catch (Exception e)
            {
                return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_DEVICE"});
            }
        }
    }
}

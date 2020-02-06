﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dl.wm.suite.fleet.api.Controllers.API.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GeofencesController : ControllerBase
  {
    // GET api/v1/Geofences
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        return Ok();
    }
  }
}

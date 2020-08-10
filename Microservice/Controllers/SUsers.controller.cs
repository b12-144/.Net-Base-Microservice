#region Imports
using Shared.Entities;
using Microservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Codes;
#endregion

namespace Microservice.Controllers {
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/users")]
    [AllowAnonymous]//todo remove this in production
    public class UsersController: ControllerBase {

        #region Fields
        private readonly UsersService service;//injected from Startup.cs
        #endregion

        #region Constructor
        public UsersController(UsersService service) {
            this.service = service;
        }
        #endregion

        #region GetAll
        [HttpGet()]
        [OpenApiOperation("getAll", "Returns all users", "Returns all users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EUser>> GetAll(int listCount = -1, int pageNumber = 0, string orderBy="id desc") {
            try {
                return Ok(service.GetAll(listCount, pageNumber, orderBy));
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region GetByID
        [HttpGet("{id}")]
        [OpenApiOperation("getByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetByID(Int64 id) {
            try {
                var bill = service.GetByID(id);
                return Ok(bill);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Save
        [HttpPut("save")]
        [OpenApiOperation("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        public async Task<IActionResult> Save([FromBody] EUser eUser) {
            try {
                var result = await service.SaveAsync(eUser);
                return Ok(result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Insert
        [HttpPost]
        [OpenApiOperation("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]    
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  
        public async Task<IActionResult> Insert([FromBody] EUser eUser) {
            try {
                var result = await service.InsertAsync(eUser);
                return StatusCode(StatusCodes.Status201Created, result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Update
        [HttpPut]
        [OpenApiOperation("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] EUser eUser) {
            try {
                var result = await service.UpdateAsync(eUser);
                return Ok(result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Remove
        [HttpDelete("remove/{id}")]
        [OpenApiOperation("remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Remove(Int64 id) {
            try {
                var result = await service.RemoveAsync(id);
                return Ok(result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);                
            }
        }
        #endregion
    }
}

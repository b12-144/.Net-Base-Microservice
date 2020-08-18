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
    [OpenApiTag("Users")]
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
        [OpenApiOperation("getAllUsers", "Returns all users", "Returns all users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [OpenApiOperation("getUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<EUser> GetByID(Int64 id) {
            try {
                var response = service.GetByID(id);
                if (response == null) return NotFound();
                return Ok(response);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Save
        [HttpPut("save")]
        [OpenApiOperation("saveUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        public async Task<ActionResult<Int64>> Save([FromBody] EUser eUser) {
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
        [OpenApiOperation("insertUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]    
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  
        public async Task<ActionResult<Int64>> Insert([FromBody] EUser eUser) {
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
        [OpenApiOperation("updateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update([FromBody] EUser eUser) {
            try {
                var result = await service.UpdateAsync(eUser);
                if (result == false) return NotFound();
                return Ok(result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Remove
        [HttpDelete("remove/{id}")]
        [OpenApiOperation("removeUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Remove(Int64 id) {
            try {
                var result = await service.RemoveAsync(id);
                if (result == false) return NotFound();
                return Ok(result);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);                
            }
        }
        #endregion

        #region Search
        [HttpPost("search/{txt}")]
        [OpenApiOperation("searchUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<EUser>> Search(string txt) {
            try {
                var list = service.Search(txt);
                return Ok(list);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Search
        [HttpPost("advanced-search")]
        [OpenApiOperation("advancedSearchUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<EUser>> AdvancedSearch([FromBody] EUserAdvancedSearchRequest e) {
            try {
                var list = service.AdvancedSearch(e);
                return Ok(list);
            } catch (Exception ex) {
                SLogger.LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}

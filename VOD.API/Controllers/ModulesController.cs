using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class ModulesController : ControllerBase
    {
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;

        public ModulesController(IAdminService db, LinkGenerator linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }

        [HttpGet()]
        public async Task<ActionResult<List<ModuleDTO>>> Get(int courseId, bool include = false)
        {
            try
            {
                var dtos = courseId.Equals(0) ?
                     await _db.GetAsync<Module, ModuleDTO>(include) :
                     await _db.GetAsync<Module, ModuleDTO>(g =>
                     g.CourseId.Equals(courseId), include);
                if (!include)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Downloads = null;
                        dto.Videos = null;
                    }
                }
                return dtos;
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModuleDTO>> Get(int id, int courseId, bool include = false)
        {
            try
            {
                var dto = courseId.Equals(0) ?
                    await _db.SingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id), include) :
                    await _db.SingleAsync<Module, ModuleDTO>(s =>
                     s.Id.Equals(id) && s.CourseId.Equals(courseId), include);
                if (dto == null)
                {
                    return NotFound();
                }
                if (!include)
                {
                    dto.Downloads = null;
                    dto.Videos = null;
                }
                return dto;
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");                
            }
        }

        [HttpPost]
        public async Task<ActionResult<ModuleDTO>> Post(int courseId, ModuleDTO model)
        {
            try
            {
                
                if (model == null)
                {
                    return BadRequest("No entity provided");
                }
                if (courseId.Equals(0))
                {
                    courseId = model.CourseId;
                }
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(model.CourseId));
                if (!exists)
                {
                    return NotFound("Could not find related entity");
                }
                var id = await _db.CreateAsync<ModuleDTO, Module>(model);
                if (id < 1)
                {
                    return BadRequest("Unable to add the entity");
                }

                var dto = await _db.SingleAsync<Module, ModuleDTO>(s =>
                s.Id.Equals(id) && s.CourseId.Equals(courseId));

                if (dto == null)
                {
                    return BadRequest("Unable to add the entity");
                }
                var uri = _linkGenerator.GetPathByAction("Get", "Modules", new { id, courseId });
                return Created(uri, dto);
                
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, int courseId, ModuleDTO model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("No entity provided");
                }
                if (!id.Equals(model.Id))
                {
                    return BadRequest("Differing ids");
                }
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(model.CourseId));
                if (!exists)
                {
                    return NotFound("Could not find related entity");
                }

                 exists = await _db.AnyAsync< Module>(a => a.Id.Equals(id));
                if (!exists)
                {
                    return NotFound("Could not find entity");
                }
                var success = await _db.UpdateAsync<ModuleDTO, Module>(model);
                if (success)
                {
                    return NoContent();
                }
               
                    
               

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update entity");
            }
            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, int courseId)
        {
            try
            {
                var exists = await _db.AnyAsync<Module>(a =>
                a.Id.Equals(id) && a.CourseId.Equals(courseId));

                if (!exists)
                {
                    return BadRequest("Could not find entity");
                }

                var success = await _db.DeleteAsync<Module>(d =>
                d.Id.Equals(id) && d.CourseId.Equals(courseId));
                if (success)
                {
                    return NoContent();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failded to delete the entity");
            }
            return BadRequest("Failed to delete the entity");
        }
    }
}
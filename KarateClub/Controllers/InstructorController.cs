using Dtos.InstructorDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        // ============================
        // ADD
        // ============================
        [HttpPost("AddInstructor")]
        public IActionResult AddInstructor([FromBody] CreateInstructorDTO instructorDTO)
        {
            if (instructorDTO == null)
                return BadRequest("Invalid Data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsInstructor instructor = new clsInstructor(instructorDTO, clsInstructor.enMode.add);

            if (!instructor.Save())
                return StatusCode(500, "Failed to add instructor");

            return Ok(instructor.IDTO);
        }

        // ============================
        // UPDATE
        // ============================
        [HttpPut("Update/{id}")]
        public IActionResult UpdateInstructor(int id, [FromBody] CreateInstructorDTO instructorDTO)
        {
            if (instructorDTO == null)
                return BadRequest("Invalid data or ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsInstructor instructor = new clsInstructor(instructorDTO, clsInstructor.enMode.update);
            instructor.InstructorID = id;

            if (!instructor.Save())
                return StatusCode(500, "Failed to update instructor");

            return Ok(instructor.IDTO);
        }

        // ============================
        // GET BY ID
        // ============================
        [HttpGet("{id}")]
        public IActionResult GetInstructor(int id)
        {
            clsInstructor? instructor = clsInstructor.Find(id);

            if (instructor == null)
                return NotFound();

            return Ok(instructor.IDTO);
        }

        // ============================
        // GET ALL
        // ============================
        [HttpGet("GetAllInstructors")]
        public IActionResult GetAllInstructors()
        {
            List<CreateInstructorDTO> instructors = clsInstructor.GetAllInstructors();

            if (instructors == null || instructors.Count == 0)
                return NotFound("There is no data.");

            return Ok(instructors);
        }

        // ============================
        // DELETE
        // ============================
        [HttpDelete("DeleteInstructor/{id}")]
        public IActionResult DeleteInstructor(int id)
        {
            if (clsInstructor.Find(id) == null)
                return NotFound($"Instructor with id {id} not found");

            if (!clsInstructor.DeleteInstructor(id))
                return BadRequest("An error occurred while deleting instructor");

            return Ok($"Instructor with id {id} deleted successfully");
        }
    }
}

using Dtos.MemberInstructorDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberInstructorController : ControllerBase
    {
        // ============================
        // ADD
        // ============================
        [HttpPost("AddMemberInstructor")]
        public IActionResult AddMemberInstructor([FromBody] MemberInstructorDTO memberInstructorDTO)
        {
            if (memberInstructorDTO == null)
                return BadRequest("Invalid data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsMemberInstructor mi = new clsMemberInstructor(memberInstructorDTO, clsMemberInstructor.enMode.add);

            if (!mi.Save())
                return StatusCode(500, "Failed to add member-instructor record.");

            return Ok(mi.MIDTO);
        }

        // ============================
        // UPDATE
        // ============================
        [HttpPut("Update")]
        public IActionResult UpdateMemberInstructor([FromQuery] int memberId, [FromQuery] int instructorId, [FromBody] MemberInstructorDTO memberInstructorDTO)
        {
            if (memberInstructorDTO == null)
                return BadRequest("Invalid data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (memberId != memberInstructorDTO.MemberID || instructorId != memberInstructorDTO.InstructorID)
                return BadRequest("ID mismatch.");

            clsMemberInstructor mi = new clsMemberInstructor(memberInstructorDTO, clsMemberInstructor.enMode.update);

            if (!mi.Save())
                return StatusCode(500, "Failed to update member-instructor record.");

            return Ok(mi.MIDTO);
        }

        // ============================
        // GET BY IDs
        // ============================
        [HttpGet("Get")]
        public IActionResult GetMemberInstructor([FromQuery] int memberId, [FromQuery] int instructorId)
        {
            clsMemberInstructor? mi = clsMemberInstructor.Find(memberId, instructorId);

            if (mi == null)
                return NotFound();

            return Ok(mi.MIDTO);
        }

        // ============================
        // GET ALL
        // ============================
        [HttpGet("GetAllMemberInstructors")]
        public IActionResult GetAllMemberInstructors()
        {
            List<MemberInstructorDTO> list = clsMemberInstructor.GetAllMemberInstructors();

            if (list == null || list.Count == 0)
                return NotFound("There is no data.");

            return Ok(list);
        }

        // ============================
        // DELETE
        // ============================
        [HttpDelete("Delete")]
        public IActionResult DeleteMemberInstructor([FromQuery] int memberId, [FromQuery] int instructorId)
        {
            if (clsMemberInstructor.Find(memberId, instructorId) == null)
                return NotFound($"MemberInstructor record with MemberID={memberId} and InstructorID={instructorId} not found.");

            if (!clsMemberInstructor.DeleteMemberInstructor(memberId, instructorId))
                return BadRequest("An error occurred while deleting member-instructor record.");

            return Ok($"MemberInstructor record with MemberID={memberId} and InstructorID={instructorId} deleted successfully.");
        }
    }
}

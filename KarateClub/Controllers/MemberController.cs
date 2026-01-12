using Dtos.MembersDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpPost("AddMember")]
        public IActionResult AddMember([FromBody] MemberDTO memberDTO)
        {
            if (memberDTO == null)
                return BadRequest("Invalid Data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsMember member = new clsMember(memberDTO, clsMember.enMode.add);

            if (!member.Save())
                return StatusCode(500, "Failed to add member");

            return Ok(member.MDTO);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateMember(int id, [FromBody] MemberDTO memberDTO)
        {
            if (memberDTO == null)
                return BadRequest("Invalid data or ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Optional: Check ID consistency if you want to
            // if (id != memberDTO.MemberID)
            //    return BadRequest("ID mismatch");

            clsMember member = new clsMember(memberDTO, clsMember.enMode.update);
            member.MemberId = id;

            if (!member.Save())
                return StatusCode(500, "Failed to update member");

            return Ok(member.MDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetMember(int id)
        {
            clsMember? member = clsMember.Find(id);

            if (member == null)
                return NotFound();

            return Ok(member.MDTO);
        }

        [HttpGet("GetAllMembers")]
        public IActionResult GetAllMembers()
        {
            List<MemberDTO> members = clsMember.GetAllMembers();

            if (members == null || members.Count == 0)
                return NotFound("There is no data.");

            return Ok(members);
        }

        [HttpDelete("DeleteMember/{id}")]
        public IActionResult DeleteMember(int id)
        {
            if (!clsMember.IsMemberExist(id))
                return NotFound($"Member with id {id} not found");

            if (!clsMember.DeleteMember(id))
                return BadRequest("An error occurred while deleting member");

            return Ok($"Member with id {id} deleted successfully");
        }

        [HttpPut("Deactivate/{id}")]
        public IActionResult DeactivateMember(int id)
        {
            if (!clsMember.IsMemberExist(id))
                return NotFound($"Member with id {id} not found");

            if (!clsMember.DeactivateMember(id))
                return BadRequest("Failed to deactivate member");

            return Ok($"Member with id {id} deactivated successfully");
        }
    }
}

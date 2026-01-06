using Dtos.BeltRankDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeltRankController : ControllerBase
    {
        // ============================
        // ADD
        // ============================
        [HttpPost("AddBeltRank")]
        public IActionResult AddBeltRank([FromBody] CreateBeltRankDTO beltRankDTO)
        {
            if (beltRankDTO == null)
                return BadRequest("Invalid data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsBeltRank beltRank = new clsBeltRank(beltRankDTO, clsBeltRank.enMode.add);

            if (!beltRank.Save())
                return StatusCode(500, "Failed to add belt rank.");

            return Ok(beltRank.BRDTO);
        }

        // ============================
        // UPDATE
        // ============================
        [HttpPut("Update/{id}")]
        public IActionResult UpdateBeltRank(int id, [FromBody] CreateBeltRankDTO beltRankDTO)
        {
            if (beltRankDTO == null)
                return BadRequest("Invalid data or ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsBeltRank beltRank = new clsBeltRank(beltRankDTO, clsBeltRank.enMode.update);
            beltRank.BeltRankID = id;

            if (!beltRank.Save())
                return StatusCode(500, "Failed to update belt rank.");

            return Ok(beltRank.BRDTO);
        }

        // ============================
        // GET BY ID
        // ============================
        [HttpGet("{id}")]
        public IActionResult GetBeltRank(int id)
        {
            clsBeltRank? beltRank = clsBeltRank.Find(id);

            if (beltRank == null)
                return NotFound();

            return Ok(beltRank.BRDTO);
        }

        // ============================
        // GET ALL
        // ============================
        [HttpGet("GetAllBeltRanks")]
        public IActionResult GetAllBeltRanks()
        {
            List<CreateBeltRankDTO> beltRanks = clsBeltRank.GetAllBeltRanks();

            if (beltRanks == null || beltRanks.Count == 0)
                return NotFound("There is no data.");

            return Ok(beltRanks);
        }

        // ============================
        // DELETE
        // ============================
        [HttpDelete("DeleteBeltRank/{id}")]
        public IActionResult DeleteBeltRank(int id)
        {
            if (clsBeltRank.Find(id) == null)
                return NotFound($"BeltRank with id {id} not found.");

            if (!clsBeltRank.DeleteBeltRank(id))
                return BadRequest("An error occurred while deleting belt rank.");

            return Ok($"BeltRank with id {id} deleted successfully.");
        }
    }
}

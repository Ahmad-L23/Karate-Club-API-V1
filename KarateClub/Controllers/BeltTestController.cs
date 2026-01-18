using Dtos.BeltTestsDTOS;
using KarateClubBusinessLayer;
using KarateClubDataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeltTestController : ControllerBase
    {
        // GET: api/BeltTest/member/{memberId}/tests
        [HttpGet("member/{memberId}/tests")]
        public IActionResult GetTestsForMember(int memberId)
        {
            var tests = clsBeltTest.GetTestsForMember(memberId);
            if (tests == null || tests.Count == 0)
                return NotFound();

            return Ok(tests);
        }

        // GET: api/BeltTest/all
        [HttpGet("all")]
        public IActionResult GetAllTests()
        {
            var tests = clsBeltTest.GetAllTests();
            return Ok(tests);
        }

        // GET: api/BeltTest/member/{memberId}/last
        [HttpGet("member/{memberId}/last")]
        public IActionResult GetLastTestForMember(int memberId)
        {
            // Get raw BeltTestDTO (no joins)
            var lastTestDto = ClsBeltTestData.GetLastTestForMember(memberId);
            if (lastTestDto == null)
                return NotFound();

            // Compose related objects
            var member = clsMember.Find(lastTestDto.MemberID);
            var instructor = clsInstructor.Find(lastTestDto.TestByInstructor);
            var rank = clsBeltRank.Find(lastTestDto.RankID);

            if (member == null || instructor == null || rank == null)
                return StatusCode(500, "Related data not found for the test.");

            var viewDto = new BeltTestViewDTO
            {
                TestID = lastTestDto.TestID,
                MemberID = lastTestDto.MemberID,
                MemberName = member.person?.Name ?? "Unknown Member",
                RankID = lastTestDto.RankID,
                RankName = rank.RankName,
                Result = lastTestDto.Result,
                TestDate = lastTestDto.TestDate,
                InstructorID = lastTestDto.TestByInstructor,
                InstructorName = instructor.person?.Name ?? "Unknown Instructor"
            };

            return Ok(viewDto);
        }

        // POST: api/BeltTest
        [HttpPost]
        public IActionResult AddTest([FromBody] BeltTestDTO testDto)
        {
            if (testDto == null)
                return BadRequest("Test data is required.");

            var test = new clsBeltTest(testDto, clsBeltTest.enMode.add);

            if (test.Save())
                return Ok(new { Message = "Test added successfully.", TestID = test.TestID });
            else
                return StatusCode(500, "Failed to add test.");
        }

        // PUT: api/BeltTest/{testId}
        [HttpPut("{testId}")]
        public IActionResult UpdateTest(int testId, [FromBody] BeltTestDTO testDto)
        {
            if (testDto == null || testId != testDto.TestID)
                return BadRequest("Test ID mismatch or missing data.");

            var existingDto = ClsBeltTestData.Find(testId);
            if (existingDto == null)
                return NotFound();

            var test = new clsBeltTest(testDto, clsBeltTest.enMode.update);

            if (test.Save())
                return Ok("Test updated successfully.");
            else
                return StatusCode(500, "Failed to update test.");
        }

        // DELETE: api/BeltTest/{testId}
        [HttpDelete("{testId}")]
        public IActionResult DeleteTest(int testId)
        {
            if (!clsBeltTest.IsTestExist(testId))
                return NotFound();

            if (clsBeltTest.DeleteTest(testId))
                return Ok("Test deleted successfully.");
            else
                return StatusCode(500, "Failed to delete test.");
        }
    }
}

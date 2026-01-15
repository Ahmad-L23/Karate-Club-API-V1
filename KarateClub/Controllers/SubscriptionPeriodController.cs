using Dtos.SubscriptionPeriodDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPeriodController : ControllerBase
    {
        [HttpPost("AddSubscriptionPeriod")]
        public IActionResult AddSubscriptionPeriod([FromBody] SubscriptionPeriodDTO spDTO)
        {
            if (spDTO == null)
                return BadRequest("Invalid data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsSubscriptionPeriod sp = new clsSubscriptionPeriod(spDTO, clsSubscriptionPeriod.enMode.add);

            if (!sp.Save())
                return StatusCode(500, "Failed to add subscription period");

            return Ok(sp.SPDTO);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateSubscriptionPeriod(int id, [FromBody] SubscriptionPeriodDTO spDTO)
        {
            if (spDTO == null)
                return BadRequest("Invalid data or ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Optional: check ID consistency if you want
            // if (id != spDTO.PeriodID)
            //     return BadRequest("ID mismatch");

            clsSubscriptionPeriod sp = new clsSubscriptionPeriod(spDTO, clsSubscriptionPeriod.enMode.update);
            sp.PeriodID = id;

            if (!sp.Save())
                return StatusCode(500, "Failed to update subscription period");

            return Ok(sp.SPDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubscriptionPeriod(int id)
        {
            clsSubscriptionPeriod? sp = clsSubscriptionPeriod.Find(id);

            if (sp == null)
                return NotFound();

            return Ok(sp.SPDTO);
        }

        [HttpGet("GetAllSubscriptionPeriods")]
        public IActionResult GetAllSubscriptionPeriods()
        {
            List<SubscriptionPeriodDTO> periods = clsSubscriptionPeriod.GetAll();

            if (periods == null || periods.Count == 0)
                return NotFound("There is no data.");

            return Ok(periods);
        }

        [HttpDelete("DeleteSubscriptionPeriod/{id}")]
        public IActionResult DeleteSubscriptionPeriod(int id)
        {
            if (!clsSubscriptionPeriod.IsExist(id))
                return NotFound($"Subscription period with id {id} not found");

            if (!clsSubscriptionPeriod.Delete(id))
                return BadRequest("An error occurred while deleting subscription period");

            return Ok($"Subscription period with id {id} deleted successfully");
        }

        // Extra endpoints similar to business methods

        [HttpGet("GetActiveSubscriptions")]
        public IActionResult GetActiveSubscriptions()
        {
            var activeSubscriptions = clsSubscriptionPeriod.GetActiveSubscriptions();

            if (activeSubscriptions == null || activeSubscriptions.Count == 0)
                return NotFound("No active subscriptions found.");

            return Ok(activeSubscriptions);
        }

        [HttpGet("GetSubscriptionsByMember/{memberId}")]
        public IActionResult GetSubscriptionsByMember(int memberId)
        {
            var memberSubscriptions = clsSubscriptionPeriod.GetSubscriptionsByMember(memberId);

            if (memberSubscriptions == null || memberSubscriptions.Count == 0)
                return NotFound($"No subscriptions found for member with id {memberId}");

            return Ok(memberSubscriptions);
        }

        [HttpGet("GetUpcomingExpiringSubscriptions")]
        public IActionResult GetUpcomingExpiringSubscriptions()
        {
            var upcomingExpiring = clsSubscriptionPeriod.GetUpcomingExpiringSubscriptions();

            if (upcomingExpiring == null || upcomingExpiring.Count == 0)
                return NotFound("No upcoming expiring subscriptions found.");

            return Ok(upcomingExpiring);
        }

        [HttpGet("GetTotalFeesByMember/{memberId}")]
        public IActionResult GetTotalFeesByMember(int memberId)
        {
            decimal totalFees = clsSubscriptionPeriod.GetTotalFeesByMember(memberId);

            return Ok(new { MemberId = memberId, TotalFees = totalFees });
        }
    }
}

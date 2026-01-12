using Dtos.PaymentsDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("AddPayment")]
        public IActionResult AddPayment([FromBody] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
                return BadRequest("Invalid data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsPayment payment = new clsPayment(paymentDTO, clsPayment.enMode.add);

            if (!payment.Save())
                return StatusCode(500, "Failed to add payment");

            return Ok(payment.PDTO);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
                return BadRequest("Invalid data");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsPayment payment = new clsPayment(paymentDTO, clsPayment.enMode.update);
            payment.PaymentID = id;

            if (!payment.Save())
                return StatusCode(500, "Failed to update payment");

            return Ok(payment.PDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            clsPayment? payment = clsPayment.Find(id);

            if (payment == null)
                return NotFound();

            return Ok(payment.PDTO);
        }

        [HttpGet("GetAllPayments")]
        public IActionResult GetAllPayments()
        {
            List<PaymentDTO> payments = clsPayment.GetAllPayments();

            if (payments == null || payments.Count == 0)
                return NotFound("There is no data.");

            return Ok(payments);
        }

        [HttpGet("GetPaymentsByMember/{memberId}")]
        public IActionResult GetPaymentsByMember(int memberId)
        {
            List<PaymentDTO> payments = clsPayment.GetPaymentsByMemberId(memberId);

            if (payments == null || payments.Count == 0)
                return NotFound($"No payments found for member with id {memberId}");

            return Ok(payments);
        }

        [HttpDelete("DeletePayment/{id}")]
        public IActionResult DeletePayment(int id)
        {
            if (!clsPayment.IsPaymentExist(id))
                return NotFound($"Payment with id {id} not found");

            if (!clsPayment.DeletePayment(id))
                return BadRequest("An error occurred while deleting payment");

            return Ok($"Payment with id {id} deleted successfully");
        }
    }
}

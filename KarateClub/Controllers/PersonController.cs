using Dtos.PersonDTOS;
using KarateClubBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        [HttpPost("AddPerson")]

        public IActionResult AddPerson([FromBody] CreatePersonDTO personDTO)
        {
            if (personDTO == null)
                return BadRequest("Invalid Data");

            clsPerson person = new clsPerson(personDTO, clsPerson.enMode.add);

            if(!person.Save())
                return StatusCode(500, "Failed to add person");
            


            return Ok(person.PDTO);
        }

        [HttpPut("Update{id}")]
        public IActionResult UpdatePerson(int id, [FromBody] CreatePersonDTO personDTO)
        {
            if (personDTO == null)
                return BadRequest("Invalid data or ID mismatch");

            clsPerson person = new clsPerson(personDTO, clsPerson.enMode.update);
            person.PersonID = id;
            if (!person.Save())
                return StatusCode(500, "Failed to update person");

            return Ok(person.PDTO);
        }


        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            clsPerson person = clsPerson.Find(id);

            if (person == null)
                return NotFound();

            return Ok(person.PDTO);
        }


    }
}

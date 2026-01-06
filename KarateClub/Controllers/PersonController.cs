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
            clsPerson? person = clsPerson.Find(id);

            if (person == null)
                return NotFound();

            return Ok(person.PDTO);
        }


        [HttpGet("GetAllPersons")]
        public IActionResult GetAllPersons()
        {
            List<CreatePersonDTO> persons = clsPerson.GetAllPrrsons();

            if (persons == null || persons.Count == 0)
            {
                return NotFound("There is no data.");
            }

            return Ok(persons);
        }
        
        
        [HttpDelete("DeletePerson")]
        
        public IActionResult DeletePerson(int id)
        {
            if(clsPerson.Find(id) == null)
            {
                return NotFound($"Person with id {id} not found");
            }
            if(!(clsPerson.deletePerson(id)))
            {
                return BadRequest("an error occurred while deleting person");
            }

            return Ok($"Person with {id} deleted sucessfully");
        }

        

    }
}

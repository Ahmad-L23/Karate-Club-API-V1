using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.PersonDTOS
{
    public class CreatePersonDTO
    {
        public int PersonID {  get; set; }
        public string Name { get; set; }
        public string? Address {  get; set; }
        public string? ContactInfo { get; set; }
    }
}

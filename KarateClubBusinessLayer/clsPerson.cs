using Dtos.PersonDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarateClubBusinessLayer
{
    public class clsPerson
    {
        public enum enMode { add, update }
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? ContactInfo { get; set; }

        public CreatePersonDTO PDTO
        {
            get
            {
                return new CreatePersonDTO
                {
                    PersonID = this.PersonID,
                    Name = this.Name,
                    Address = this.Address,
                    ContactInfo = this.ContactInfo
                };
            }
        }

        public enMode Mode = enMode.add;
        public clsPerson()
        {
            PersonID = 0;
            Name = "";
            Address = null;
            ContactInfo = null;
            Mode = enMode.add;
        }

        public clsPerson(CreatePersonDTO pDTO, enMode mode = enMode.add)
        {
            PersonID = pDTO.PersonID;
            Name = pDTO.Name;
            Address = pDTO.Address;
            ContactInfo = pDTO.ContactInfo;
            Mode = mode;
        }

        private bool _AddNewPerson()
        {
            PersonID = clsPersonData.AddNewPerson(PDTO);

            return (PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PDTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewPerson();

                case enMode.update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static clsPerson? Find(int id)
        {
            CreatePersonDTO? person = clsPersonData.FindPerson(id);

            if (person == null)
                return null;
            return new clsPerson(person,enMode.update);
        }


        public static List<CreatePersonDTO> GetAllPrrsons()
        {
            return clsPersonData.GetAll();
        }


        public static bool deletePerson(int personID)
        {
            return clsPersonData.DeletePerson(personID);
        }
    }
}

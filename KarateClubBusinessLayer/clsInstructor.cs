using Dtos.InstructorDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;

namespace KarateClubBusinessLayer
{
    public class clsInstructor
    {
        public enum enMode { add, update }

        public int InstructorID { get; set; }
        public int PersonID { get; set; }
        public clsPerson? person {  get; set; }
        public string? Qualification { get; set; }

        public CreateInstructorDTO IDTO
        {
            get
            {
                return new CreateInstructorDTO
                {
                    InstructorID = this.InstructorID,
                    PersonID = this.PersonID,
                    Qualification = this.Qualification
                };
            }
        }

        public enMode Mode = enMode.add;

        // ============================
        // Constructors
        // ============================
        public clsInstructor()
        {
            InstructorID = 0;
            PersonID = 0;
            Qualification = null;
            Mode = enMode.add;
        }

        public clsInstructor(CreateInstructorDTO dto, enMode mode = enMode.add)
        {
            InstructorID = dto.InstructorID;
            PersonID = dto.PersonID;
            person = clsPerson.Find(PersonID);
            Qualification = dto.Qualification;
            Mode = mode;
        }

        // ============================
        // Private Methods
        // ============================
        private bool _AddNewInstructor()
        {
            InstructorID = clsInstructorsData.AddNewInstructor(IDTO);
            return (InstructorID != -1);
        }

        private bool _UpdateInstructor()
        {
            return clsInstructorsData.UpdateInstructor(IDTO);
        }

        // ============================
        // Public Methods
        // ============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewInstructor();

                case enMode.update:
                    return _UpdateInstructor();
            }

            return false;
        }

        public static clsInstructor? Find(int id)
        {
            CreateInstructorDTO? dto = clsInstructorsData.FindInstructor(id);

            if (dto == null)
                return null;

            return new clsInstructor(dto, enMode.update);
        }

        public static List<CreateInstructorDTO> GetAllInstructors()
        {
            return clsInstructorsData.GetAll();
        }

        public static bool DeleteInstructor(int instructorID)
        {
            return clsInstructorsData.DeleteInstructor(instructorID);
        }
    }
}

using Dtos.MemberInstructorDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;

namespace KarateClubBusinessLayer
{
    public class clsMemberInstructor
    {
        public enum enMode { add, update }

        public int MemberID { get; set; }
        public int InstructorID { get; set; }
        public DateTime AssignDate { get; set; }

        public MemberInstructorDTO MIDTO
        {
            get
            {
                return new MemberInstructorDTO
                {
                    MemberID = this.MemberID,
                    InstructorID = this.InstructorID,
                    AssignDate = this.AssignDate
                };
            }
        }

        public enMode Mode = enMode.add;

        // ============================
        // Constructors
        // ============================
        public clsMemberInstructor()
        {
            MemberID = 0;
            InstructorID = 0;
            AssignDate = DateTime.Now;
            Mode = enMode.add;
        }

        public clsMemberInstructor(MemberInstructorDTO dto, enMode mode = enMode.add)
        {
            MemberID = dto.MemberID;
            InstructorID = dto.InstructorID;
            AssignDate = dto.AssignDate;
            Mode = mode;
        }

        // ============================
        // Private Methods
        // ============================
        private bool _AddNewMemberInstructor()
        {
            return clsMemberInstructorData.AddMemberInstructor(MIDTO);
        }

        private bool _UpdateMemberInstructor()
        {
            return clsMemberInstructorData.UpdateMemberInstructor(MIDTO);
        }

        // ============================
        // Public Methods
        // ============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewMemberInstructor();

                case enMode.update:
                    return _UpdateMemberInstructor();
            }

            return false;
        }

        public static clsMemberInstructor? Find(int memberId, int instructorId)
        {
            MemberInstructorDTO? dto = clsMemberInstructorData.FindMemberInstructor(memberId, instructorId);

            if (dto == null)
                return null;

            return new clsMemberInstructor(dto, enMode.update);
        }

        public static List<MemberInstructorDTO> GetAllMemberInstructors()
        {
            return clsMemberInstructorData.GetAll();
        }

        public static bool DeleteMemberInstructor(int memberId, int instructorId)
        {
            return clsMemberInstructorData.DeleteMemberInstructor(memberId, instructorId);
        }
    }
}

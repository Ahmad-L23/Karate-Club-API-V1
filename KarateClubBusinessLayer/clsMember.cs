using Dtos.MembersDTOS;
using Dtos.PersonDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarateClubBusinessLayer
{
    public class clsMember
    {
        public enum enMode { add, update }

        public int MemberId { get; set; }
        public int PersonId { get; set; }
        public clsPerson? person {  get; set; }
        public string? EmergencyContactInfo { get; set; }
        public int LastBeltRank { get; set; }
        public bool isActive { get; set; }


        // ===============================
        // DTO
        // ===============================
        public MemberDTO MDTO
        {
            get
            {
                return new MemberDTO
                {
                    MemberID = this.MemberId,
                    PersonId = this.PersonId,
                    EmergencyContactInfo = this.EmergencyContactInfo,
                    LastBeltRank = this.LastBeltRank,
                    isActive = this.isActive
                };
            }
        }


        public enMode Mode = enMode.add;


        // ===============================
        // CONSTRUCTORS
        // ===============================
        public clsMember()
        {
            MemberId = 0;
            PersonId = 0;
            EmergencyContactInfo = null;
            LastBeltRank = 0;
            isActive = true;
            Mode = enMode.add;
        }

        public clsMember(MemberDTO mDTO, enMode mode = enMode.add)
        {
            MemberId = mDTO.MemberID;
            PersonId = mDTO.PersonId;
            person = clsPerson.Find(PersonId);
            EmergencyContactInfo = mDTO.EmergencyContactInfo;
            LastBeltRank = mDTO.LastBeltRank;
            isActive = mDTO.isActive;
            Mode = mode;
        }


        // ===============================
        // PRIVATE METHODS
        // ===============================
        private bool _AddNewMember()
        {
            MemberId = ClsMemberData.AddNewMember(MDTO);
            return (MemberId != -1);
        }

        private bool _UpdateMember()
        {
            return ClsMemberData.UpdateMember(MDTO);
        }


        // ===============================
        // SAVE
        // ===============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewMember();

                case enMode.update:
                    return _UpdateMember();
            }

            return false;
        }


        // ===============================
        // STATIC METHODS
        // ===============================
        public static clsMember? Find(int memberId)
        {
            MemberDTO? member = ClsMemberData.FindMember(memberId);
            CreatePersonDTO? person = clsPersonData.FindPerson(member.PersonId);
            if (member == null)
                return null;

            return new clsMember(member, enMode.update);
        }


        public static List<MemberDTO> GetAllMembers()
        {
            return ClsMemberData.GetAll();
        }


        public static bool DeleteMember(int memberId)
        {
            return ClsMemberData.DeleteMember(memberId);
        }


        public static bool DeactivateMember(int memberId)
        {
            return ClsMemberData.DeactivateMember(memberId);
        }

        public static bool IsMemberExist(int memberId)
        {
            return ClsMemberData.IsMemberExist(memberId);
        }
    }
}

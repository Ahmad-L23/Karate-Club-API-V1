using System;
using System.Collections.Generic;
using Dtos.BeltTestsDTOS;
using KarateClubDataAccessLayer;
using KarateClubBusinessLayer; // Assuming clsMember, clsBeltRank, clsInstructor are here

namespace KarateClubBusinessLayer
{
    public class clsBeltTest
    {
        public enum enMode { add, update }

        public int TestID { get; set; }
        public int MemberID { get; set; }
        public int RankID { get; set; }
        public bool Result { get; set; }
        public DateTime TestDate { get; set; }
        public int TestByInstructor { get; set; }
        public int? PaymentID { get; set; }

        // Composition properties
        public clsMember? Member { get; set; }
        public clsBeltRank? Rank { get; set; }
        public clsInstructor? Instructor { get; set; }

        public enMode Mode { get; set; }

        // ===============================
        // DTO Property
        // ===============================
        public BeltTestDTO BDTO
        {
            get
            {
                return new BeltTestDTO
                {
                    TestID = this.TestID,
                    MemberID = this.MemberID,
                    RankID = this.RankID,
                    Result = this.Result,
                    TestDate = this.TestDate,
                    TestByInstructor = this.TestByInstructor,
                    PaymentID = this.PaymentID
                };
            }
        }

        // ===============================
        // CONSTRUCTORS
        // ===============================

        // Default constructor (add mode)
        public clsBeltTest()
        {
            TestID = 0;
            MemberID = 0;
            RankID = 0;
            Result = false;
            TestDate = DateTime.Now;
            TestByInstructor = 0;
            PaymentID = null;

            Member = null;
            Rank = null;
            Instructor = null;

            Mode = enMode.add;
        }

        // Constructor from DTO and mode, composes related objects inside
        public clsBeltTest(BeltTestDTO dto, enMode mode = enMode.add)
        {
            TestID = dto.TestID;
            MemberID = dto.MemberID;
            RankID = dto.RankID;
            Result = dto.Result;
            TestDate = dto.TestDate;
            TestByInstructor = dto.TestByInstructor;
            PaymentID = dto.PaymentID;

            // Compose related objects
            Member = clsMember.Find(MemberID);
            Rank = clsBeltRank.Find(RankID);
            Instructor = clsInstructor.Find(TestByInstructor);

            Mode = mode;
        }

        // ===============================
        // PRIVATE METHODS
        // ===============================

        private bool _AddNewTest()
        {
            int newId = ClsBeltTestData.AddNewTest(BDTO);
            if (newId != -1)
            {
                TestID = newId;
                return true;
            }
            return false;
        }

        private bool _UpdateTest()
        {
            return ClsBeltTestData.UpdateTest(BDTO);
        }

        // ===============================
        // SAVE
        // ===============================

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewTest();

                case enMode.update:
                    return _UpdateTest();

                default:
                    return false;
            }
        }

        // ===============================
        // STATIC METHODS
        // ===============================

        public static BeltTestViewDTO? Find(int testId)
        {
            var dto = ClsBeltTestData.Find(testId);
            if (dto == null) return null;
            return dto;
        }

        public static List<BeltTestViewDTO> GetTestsForMember(int memberId)
        {
            return ClsBeltTestData.GetTestsForMember(memberId);
        }

        public static List<BeltTestViewDTO> GetAllTests()
        {
            return ClsBeltTestData.GetAllWithNames();
        }

        public static BeltTestDTO? GetLastTestForMember(int memberId)
        {
            var dto = ClsBeltTestData.GetLastTestForMember(memberId);
            if (dto == null) return null;
            return dto;
        }

        public static bool DeleteTest(int testId)
        {
            return ClsBeltTestData.DeleteTest(testId);
        }

        public static bool IsTestExist(int testId)
        {
            return ClsBeltTestData.IsExist(testId);
        }
    }
}

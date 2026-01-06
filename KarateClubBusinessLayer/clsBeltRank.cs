using Dtos.BeltRankDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;

namespace KarateClubBusinessLayer
{
    public class clsBeltRank
    {
        public enum enMode { add, update }

        public int BeltRankID { get; set; }
        public string? RankName { get; set; }
        public decimal TestFees { get; set; }

        public CreateBeltRankDTO BRDTO
        {
            get
            {
                return new CreateBeltRankDTO
                {
                    BeltRankID = this.BeltRankID,
                    RankName = this.RankName,
                    TestFees = this.TestFees
                };
            }
        }

        public enMode Mode = enMode.add;

        // ============================
        // Constructors
        // ============================
        public clsBeltRank()
        {
            BeltRankID = 0;
            RankName = null;
            TestFees = 0;
            Mode = enMode.add;
        }

        public clsBeltRank(CreateBeltRankDTO dto, enMode mode = enMode.add)
        {
            BeltRankID = dto.BeltRankID;
            RankName = dto.RankName;
            TestFees = dto.TestFees;
            Mode = mode;
        }

        // ============================
        // Private Methods
        // ============================
        private bool _AddNewBeltRank()
        {
            BeltRankID = clsBeltRankData.AddNewBeltRank(BRDTO);
            return (BeltRankID != -1);
        }

        private bool _UpdateBeltRank()
        {
            return clsBeltRankData.UpdateBeltRank(BRDTO);
        }

        // ============================
        // Public Methods
        // ============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewBeltRank();

                case enMode.update:
                    return _UpdateBeltRank();
            }

            return false;
        }

        public static clsBeltRank? Find(int id)
        {
            CreateBeltRankDTO? dto = clsBeltRankData.FindBeltRank(id);

            if (dto == null)
                return null;

            return new clsBeltRank(dto, enMode.update);
        }

        public static List<CreateBeltRankDTO> GetAllBeltRanks()
        {
            return clsBeltRankData.GetAll();
        }

        public static bool DeleteBeltRank(int beltRankID)
        {
            return clsBeltRankData.DeleteBeltRank(beltRankID);
        }
    }
}

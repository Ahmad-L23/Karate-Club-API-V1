using Dtos.SubscriptionPeriodDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;

namespace KarateClubBusinessLayer
{
    public class clsSubscriptionPeriod
    {
        public enum enMode { add, update }

        public int PeriodID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal Fees { get; set; }
        public int memberId { get; set; }
        public int PaymentID { get; set; }

        public enMode Mode = enMode.add;

        // ===============================
        // DTO
        // ===============================
        public SubscriptionPeriodDTO SPDTO
        {
            get
            {
                return new SubscriptionPeriodDTO
                {
                    PeriodID = this.PeriodID,
                    startDate = this.startDate,
                    endDate = this.endDate,
                    Fees = this.Fees,
                    memberId = this.memberId,
                    PaymentID = this.PaymentID
                };
            }
        }

        // ===============================
        // CONSTRUCTORS
        // ===============================
        public clsSubscriptionPeriod()
        {
            PeriodID = 0;
            startDate = DateTime.Now;
            endDate = DateTime.Now;
            Fees = 0m;
            memberId = 0;
            PaymentID = 0;
            Mode = enMode.add;
        }

        public clsSubscriptionPeriod(SubscriptionPeriodDTO spDTO, enMode mode = enMode.add)
        {
            PeriodID = spDTO.PeriodID;
            startDate = spDTO.startDate;
            endDate = spDTO.endDate;
            Fees = spDTO.Fees;
            memberId = spDTO.memberId;
            PaymentID = spDTO.PaymentID;
            Mode = mode;
        }

        // ===============================
        // PRIVATE METHODS
        // ===============================
        private bool _AddNewSubscriptionPeriod()
        {
            PeriodID = clsSubscriptionPeriods.AddNewSubscriptionPeriod(SPDTO);
            return PeriodID != -1;
        }

        private bool _UpdateSubscriptionPeriod()
        {
            return clsSubscriptionPeriods.UpdateSubscriptionPeriod(SPDTO);
        }

        // ===============================
        // SAVE
        // ===============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewSubscriptionPeriod();

                case enMode.update:
                    return _UpdateSubscriptionPeriod();

                default:
                    return false;
            }
        }

        // ===============================
        // STATIC METHODS
        // ===============================
        public static clsSubscriptionPeriod? Find(int periodId)
        {
            var spDTO = clsSubscriptionPeriods.FindSubscriptionPeriod(periodId);
            if (spDTO == null)
                return null;

            return new clsSubscriptionPeriod(spDTO, enMode.update);
        }

        public static List<SubscriptionPeriodDTO> GetAll()
        {
            return clsSubscriptionPeriods.GetAll();
        }

        public static bool Delete(int periodId)
        {
            return clsSubscriptionPeriods.DeleteSubscriptionPeriod(periodId);
        }

        public static bool IsExist(int periodId)
        {
            return clsSubscriptionPeriods.IsSubscriptionPeriodExist(periodId);
        }

        public static List<SubscriptionWithBasicMemberInfoDTO> GetActiveSubscriptions()
        {
            return clsSubscriptionPeriods.GetActiveSubscriptions();
        }

        public static List<SubscriptionPeriodDTO> GetSubscriptionsByMember(int memberId)
        {
            return clsSubscriptionPeriods.GetSubscriptionsByMember(memberId);
        }

        public static List<SubscriptionWithBasicMemberInfoDTO> GetUpcomingExpiringSubscriptions()
        {
            return clsSubscriptionPeriods.GetUpcomingExpiringSubscriptions();
        }

        public static decimal GetTotalFeesByMember(int memberId)
        {
            return clsSubscriptionPeriods.GetTotalFeesByMember(memberId);
        }
    }
}

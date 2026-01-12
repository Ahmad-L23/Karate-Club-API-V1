using Dtos.PaymentsDTOS;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KarateClubBusinessLayer
{
    public class clsPayment
    {
        public enum enMode { add, update }

        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }

        // ===============================
        // DTO
        // ===============================
        public PaymentDTO PDTO
        {
            get
            {
                return new PaymentDTO
                {
                    PaymentID = this.PaymentID,
                    Amount = this.Amount,
                    Date = this.Date,
                    MemberID = this.MemberID
                };
            }
        }

        public enMode Mode = enMode.add;

        // ===============================
        // CONSTRUCTORS
        // ===============================
        public clsPayment()
        {
            PaymentID = 0;
            Amount = 0;
            Date = DateTime.Now;
            MemberID = 0;
            Mode = enMode.add;
        }

        public clsPayment(PaymentDTO pDTO, enMode mode = enMode.add)
        {
            PaymentID = pDTO.PaymentID;
            Amount = pDTO.Amount;
            Date = pDTO.Date;
            MemberID = pDTO.MemberID;
            Mode = mode;
        }

        // ===============================
        // PRIVATE METHODS
        // ===============================
        private bool _AddNewPayment()
        {
            PaymentID = clsPaymentData.AddNewPayment(PDTO);
            return (PaymentID != -1);
        }

        private bool _UpdatePayment()
        {
            return clsPaymentData.UpdatePayment(PDTO);
        }

        // ===============================
        // SAVE
        // ===============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewPayment();

                case enMode.update:
                    return _UpdatePayment();
            }

            return false;
        }

        // ===============================
        // STATIC METHODS
        // ===============================
        public static clsPayment? Find(int paymentId)
        {
            PaymentDTO? payment = clsPaymentData.FindPayment(paymentId);

            if (payment == null)
                return null;

            return new clsPayment(payment, enMode.update);
        }

        public static List<PaymentDTO> GetAllPayments()
        {
            return clsPaymentData.GetAll();
        }

        public static List<PaymentDTO> GetPaymentsByMemberId(int memberId)
        {
            return clsPaymentData.GetPaymentsByMemberId(memberId);
        }

        public static bool DeletePayment(int paymentId)
        {
            return clsPaymentData.DeletePayment(paymentId);
        }

        public static bool IsPaymentExist(int paymentId)
        {
            return clsPaymentData.IsPaymentExist(paymentId);
        }
    }
}

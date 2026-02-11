using Dtos.UsersDTOs;
using KarateClubDataAccessLayer;
using System;
using System.Collections.Generic;
using BCrypt.Net;

namespace KarateClubBusinessLayer
{
    public class clsUser
    {
        // ===============================
        // MODE
        // ===============================
        public enum enMode { add, update }

        // ===============================
        // PROPERTIES
        // ===============================
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public int PersonId { get; set; }

        // ===============================
        // DTO
        // ===============================
        public UserWithPersonDTO UDTO
        {
            get
            {
                return new UserWithPersonDTO
                {
                    UserId = this.UserId,
                    UserName = this.UserName,
                    Password = this.Password,
                    PersonId = this.PersonId
                };
            }
        }

        public enMode Mode = enMode.add;

        // ===============================
        // CONSTRUCTORS
        // ===============================
        public clsUser()
        {
            UserId = 0;
            UserName = "";
            Password = "";
            PersonId = 0;
            Mode = enMode.add;
        }

        public clsUser(UserWithPersonDTO uDTO, enMode mode = enMode.add)
        {
            UserId = uDTO.UserId;
            UserName = uDTO.UserName;
            Password = uDTO.Password;
            PersonId = uDTO.PersonId;
            Mode = mode;
        }

        // ===============================
        // PRIVATE METHODS
        // ===============================
        private bool _AddNewUser()
        {
            // Hash password before saving
         
            UserId = ClsUserData.AddNew(UDTO);
            return UserId != -1;
        }

        private bool _UpdateUser()
        {

            return ClsUserData.Update(UDTO);
        }

        // ===============================
        // SAVE
        // ===============================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.add:
                    return _AddNewUser();
                case enMode.update:
                    return _UpdateUser();
            }
            return false;
        }

        // ===============================
        // STATIC METHODS
        // ===============================

        // Find by ID (without password)
        public static clsUser? Find(int userId)
        {
            UserViewWithPersonDTO? user = ClsUserData.Find(userId);
            if (user == null) return null;

            // For Save/Update we need password, so we create a BLL object with empty password
            return new clsUser(new UserWithPersonDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = "", // password unknown
                PersonId = user.PersonId
            }, enMode.update);
        }

        // Find by username (returns full info including hashed password)
        public static UserWithPersonDTO? FindByUserName(string userName)
        {
            return ClsUserData.FindByUserName(userName);
        }

        // Get all users (without password)
        public static List<UserViewWithPersonDTO> GetAll()
        {
            return ClsUserData.GetAll();
        }

        // Delete user
        public static bool Delete(int userId)
        {
            return ClsUserData.Delete(userId);
        }

        // Check existence by ID
        public static bool IsUserExist(int userId)
        {
            return ClsUserData.IsExist(userId);
        }

        // Check if username exists
        public static bool IsUserNameExist(string userName)
        {
            return ClsUserData.IsUserNameExist(userName);
        }
    }
}

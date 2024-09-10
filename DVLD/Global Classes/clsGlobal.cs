using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
using Microsoft.Win32;
using static System.Windows.Forms.LinkLabel;


namespace DVLD.Classes
{
    static  class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                // Define the registry key where you want to store the values
                string keyName = @"SOFTWARE\DVLD";

                // Open (or create) the registry key in writable mode
                RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

                if (key != null)
                {
                    key.SetValue("Username", Username);
                    key.SetValue("Password", Password);

                    key.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                // Define the registry key where you want to store the values
                string keyName = @"SOFTWARE\DVLD";

                // Open (or create) the registry key in writable mode
                RegistryKey key = Registry.CurrentUser.CreateSubKey(keyName);

                if (key != null)
                {
                    Username = key.GetValue("Username", null) as string;
                    Password = key.GetValue("Password", null) as string;

                    key.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            // Hash the entered password
            string enteredPasswordHash = HashPassword(enteredPassword);

            // Compare hashed passwords
            return enteredPasswordHash == storedHashedPassword;
        }

        // Hash function (from the previous example)
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
        }
    }
}

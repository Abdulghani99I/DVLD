using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
using Microsoft.Win32;
using static System.Windows.Forms.LinkLabel;


namespace DVLD.Classes
{
    internal static  class clsGlobal
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
    }
}

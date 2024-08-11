using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
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
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + "\\data.txt";

                string dataToSave = string.Empty;

                // make sure from file exist and result should have 'Username' and 'Password'
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    // concatonate username and passwrod with seperator.
                    dataToSave = Username + "#//#" + Password;
                }
                

                // write if (file exist), or create (if not exist)
                File.WriteAllText(filePath, dataToSave);
                return true;
            }
            catch (Exception ex)
            {
               MessageBox.Show ($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                //gets the current project's directory
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath  = currentDirectory + "\\data.txt";

                string line = File.ReadAllText(filePath);
                string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.RemoveEmptyEntries);
                
                // make sure from file exist and result should have 'Username' and 'Password'
                if (File.Exists(filePath) && result?.Length == 2)
                {
                    Username = result[0];
                    Password = result[1];
                    return true;
                }   
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show ($"An error occurred: {ex.Message}");
                return false;   
            }
        }
    }
}

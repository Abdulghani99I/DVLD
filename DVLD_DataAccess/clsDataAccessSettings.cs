using System;
using System.Configuration;

namespace DVLD_DataAccess
{
    internal static class clsDataAccessSettings
    {
        internal readonly static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
    }
}

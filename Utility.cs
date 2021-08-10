#if SQLiteVersion
// #if SQLiteVersion is a "preprocessor directive", isolates the differences in the SQLite and SQL Server
using System;

namespace ContosoUniversity
{
    public static class Utility
    {
        public static string GetLastChars(Guid token)
        {
            return token.ToString().Substring(token.ToString().Length - 3);
        }
    }
}
#else
// The author maintain one code base for both versions.
namespace ContosoUniversity
{
    public static class Utility
    {
        public static string GetLastChars(byte[] token)
        {
            return token[7].ToString();
        }
    }
}
#endif
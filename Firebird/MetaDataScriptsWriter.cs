using System.Data.Common;
using System.Diagnostics;

namespace Firebird
{
    public class MetaDataScriptsWriter
    {
        public static void WriteMetadata(string connectionString, string outputDirectory)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };

            var database = builder.ContainsKey("Database") ? builder["Database"].ToString() : "";
            var user = builder.ContainsKey("User") ? builder["User"].ToString() : "";
            var password = builder.ContainsKey("Password") ? builder["Password"].ToString() : "";

            var psi = new ProcessStartInfo
            {
                FileName = DatabaseConsts.ISQL_FILEPATH,
                Arguments = $"-user {user} -password {password} -x \"{database}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            using var process = Process.Start(psi);
            using var reader = process.StandardOutput;
            string result = reader.ReadToEnd();
            File.WriteAllText(Path.Combine(outputDirectory, DatabaseConsts.EXPORT_DB_FILENAME), result);
            process.WaitForExit();
        }
    }
}

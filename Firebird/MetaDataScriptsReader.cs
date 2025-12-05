using FirebirdSql.Data.FirebirdClient;
using System.Text;

namespace Firebird
{
    public class MetaDataScriptsReader
    {
        public static void ReadMetadata(string connString, string scriptsDirectory)
        {
            using (var conn = new FbConnection(connString))
            {
                conn.Open();

                foreach (var scriptFile in Directory.GetFiles(scriptsDirectory, "*.sql"))
                {
                    var lines = File.ReadAllLines(scriptFile);
                    var currentCommand = new StringBuilder();
                    var insideProcedure = false;

                    foreach (var rawLine in lines)
                    {
                        var line = rawLine.Trim();

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (line.StartsWith("CREATE PROCEDURE", StringComparison.OrdinalIgnoreCase))
                        {
                            insideProcedure = true;
                            currentCommand.Clear();
                        }

                        currentCommand.AppendLine(line);

                        if (insideProcedure && line.Equals("END;", StringComparison.OrdinalIgnoreCase))
                        {
                            string sql = currentCommand.ToString();

                            using (var cmd = new FbCommand(sql, conn))
                                cmd.ExecuteNonQuery();

                            insideProcedure = false;
                            currentCommand.Clear();
                        }

                        else if (!insideProcedure && line.EndsWith(";"))
                        {
                            string sql = currentCommand.ToString();

                            using (var cmd = new FbCommand(sql, conn))
                                cmd.ExecuteNonQuery();

                            currentCommand.Clear();
                        }
                    }
                }
            }
        }
    }
}

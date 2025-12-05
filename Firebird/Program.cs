using Firebird;
using FirebirdSql.Data.FirebirdClient;

namespace DbMetaTool
{
    public static class Program
    {
        // Przykładowe wywołania:
        // DbMetaTool build-db --db-dir "C:\db\fb5" --scripts-dir "C:\scripts"
        // DbMetaTool export-scripts --connection-string "..." --output-dir "C:\out"
        // DbMetaTool update-db --connection-string "..." --scripts-dir "C:\scripts"
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Użycie:");
                Console.WriteLine("  build-db --db-dir <ścieżka> --scripts-dir <ścieżka>");
                Console.WriteLine("  export-scripts --connection-string <connStr> --output-dir <ścieżka>");
                Console.WriteLine("  update-db --connection-string <connStr> --scripts-dir <ścieżka>");
                return 1;
            }

            try
            {
                var command = args[0].ToLowerInvariant();

                switch (command)
                {
                    case "build-db":
                        {
                            string dbDir = GetArgValue(args, "--db-dir");
                            string scriptsDir = GetArgValue(args, "--scripts-dir");

                            BuildDatabase(dbDir, scriptsDir);
                            Console.WriteLine("Baza danych została zbudowana pomyślnie.");
                            return 0;
                        }

                    case "export-scripts":
                        {
                            string connStr = GetArgValue(args, "--connection-string");
                            string outputDir = GetArgValue(args, "--output-dir");

                            ExportScripts(connStr, outputDir);
                            Console.WriteLine("Skrypty zostały wyeksportowane pomyślnie.");
                            return 0;
                        }

                    case "update-db":
                        {
                            string connStr = GetArgValue(args, "--connection-string");
                            string scriptsDir = GetArgValue(args, "--scripts-dir");

                            UpdateDatabase(connStr, scriptsDir);
                            Console.WriteLine("Baza danych została zaktualizowana pomyślnie.");
                            return 0;
                        }

                    default:
                        Console.WriteLine($"Nieznane polecenie: {command}");
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: " + ex.Message);
                return -1;
            }
        }

        private static string GetArgValue(string[] args, string name)
        {
            int idx = Array.IndexOf(args, name);
            if (idx == -1 || idx + 1 >= args.Length)
                throw new ArgumentException($"Brak wymaganego parametru {name}");
            return args[idx + 1];
        }

        /// <summary>
        /// Buduje nową bazę danych Firebird 5.0 na podstawie skryptów.
        /// </summary>
        public static void BuildDatabase(string databaseDirectory, string scriptsDirectory)
        {
            // TODO:
            // 1) Ograniczyć odczytywanie skryptu tylko do tabel, procedur oraz domen

            try
            {
                var databasePath = Path.Combine(databaseDirectory, DatabaseConsts.IMPORT_DB_FILENAME);

                if (!Directory.Exists(databaseDirectory))
                {
                    Directory.CreateDirectory(databaseDirectory);
                }

                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }

                if (!Directory.Exists(scriptsDirectory))
                {
                    Console.WriteLine("Nie można znaleźć folderu ze skryptami");
                    return;
                }

                FbConnection.CreateDatabase(string.Format(DatabaseConsts.BUILD_DATABASE_CONNECTION_STRING, databasePath));

                Console.WriteLine("Baza danych została utworzona pomyślnie!");

                MetaDataScriptsReader.ReadMetadata(string.Format(DatabaseConsts.BUILD_DATABASE_CONNECTION_STRING, databasePath), scriptsDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas tworzenia bazy: " + ex.Message);
            }
        }

        /// <summary>
        /// Generuje skrypty metadanych z istniejącej bazy danych Firebird 5.0.
        /// </summary>
        public static void ExportScripts(string connectionString, string outputDirectory)
        {
            try
            {
                // TODO:
                // 1) Ograniczyć odczytywanie bazy danych tylko do tabel, procedur oraz domen

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                MetaDataScriptsWriter.WriteMetadata(connectionString, outputDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas tworzenia skryptów: " + ex.Message);
            }
            
        }

        /// <summary>
        /// Aktualizuje istniejącą bazę danych Firebird 5.0 na podstawie skryptów.
        /// </summary>
        public static void UpdateDatabase(string connectionString, string scriptsDirectory)
        {
            // TODO:
            // 1) Aktualizacja bazy danch działa w tej chwili podobnie jak tworzenie początkowych obiektów w bazie - należy 
            //    dostosować kod do np. aktualizacji procedur

            try
            {
                if (!Directory.Exists(scriptsDirectory))
                {
                    Console.WriteLine("Nie można znaleźć folderu ze skryptami");
                    return;
                }

                MetaDataScriptsReader.ReadMetadata(connectionString, scriptsDirectory);

                Console.WriteLine("Baza danych została utworzona pomyślnie!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas aktualizowania bazy: " + ex.Message);
            }
        }
    }
}

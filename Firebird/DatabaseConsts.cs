namespace Firebird
{
    public class DatabaseConsts
    {
        public const string IMPORT_DB_FILENAME = "import_fb.fdb";
        public const string EXPORT_DB_FILENAME = "export_fb.sql";
        public const string BUILD_DATABASE_CONNECTION_STRING = @"User=SYSDBA;Password=masterkey;Database={0};DataSource=localhost;Charset=UTF8;ServerType=Default;";
        public const string ISQL_FILEPATH = @"C:\Program Files\Firebird\Firebird_5_0\isql.exe";
    }
}

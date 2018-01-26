using Oakton;

namespace specsv
{
    public class CommandInput
    {
        [FlagAlias("dbtype",'t')]
        [Description("The type of database you are querying from")]
        public DatabaseTypes DatabaseTypeFlag { get; set; }

        [FlagAlias("conn",'c')]
        [Description("Database connection string?")]
        public string DatabaseConnectionStringFlag { get; set; }

        [FlagAlias("outfolder",'o')]
        [Description("The output directory, default is current")]
        public string OutputFolderFlag { get; set; }
    }
}
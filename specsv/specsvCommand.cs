using System;
using Oakton;

namespace specsv
{
    [Description("Export all tables from a database to CSV files")]
    public class specsvCommand : OaktonCommand<CommandInput>
    {
        public specsvCommand()
        {
            
        }

        public override bool Execute(CommandInput input)
        {
            try
            {

                switch (input.DatabaseTypeFlag)
                {
                    case DatabaseTypes.SqlServer:
                        var sql = new SqlServer(input.DatabaseConnectionStringFlag);
                        sql.Process(input);
                        return true;
                    default:
                        ConsoleWriter.Write($"Unable to handle database type '{input.DatabaseTypeFlag}'");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception: {ex.Message}");
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace dblisterner
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var InfoMapper = new ModelToTableMapper<Customer>();
            InfoMapper.AddMapping(c => c.Surname, "Surname");
            InfoMapper.AddMapping(c => c.Name, "Name");

            var connectionString = "Server=localhost\\MSSQLSERVER02;Database=Stock;User Id=sa;Password=##admin00@";
            using (var tableDependency = new SqlTableDependency<Customer>(connectionString, tableName: "Customer", schemaName: "dbo", mapper: InfoMapper, executeUserPermissionCheck: false, includeOldValues: true))
            {
                tableDependency.OnChanged += TableDependency_Changed;
                tableDependency.Start();

                Console.WriteLine("Press a key to exit");
                Console.Read();

                tableDependency.Stop();
            }
        }

        private static void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            throw e.Error;
        }

        private static void TableDependency_Changed(object sender, RecordChangedEventArgs<Customer> e)
        {
            Console.WriteLine(Environment.NewLine);
            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                Console.WriteLine("DML operation: " + e.ChangeType);
                Console.WriteLine("ID: " + changedEntity.Id);
                Console.WriteLine("Name: " + changedEntity.Name);
                Console.WriteLine("Surname: " + changedEntity.Surname);
            }
        
        }
    }
}

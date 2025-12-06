using Infrastructure.Dal;
using Infrastructure.Dal.Repositories;
using Infrastructure.Entities;
using Utilities;
using Utilities.enums;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var r = new UserRepository();
           
            foreach (var item in r.All()) {
                Console.WriteLine(item);   
            }
            new DatabaseCreation().createDatabase();
            //string s = PasswordUtility.HashPassword("#Tarea123");
           //Console.WriteLine(s);
           //string ss = QueryUtility.GetUpdateString<UserTask>(["duedate"], "");
           //Console.WriteLine(ss);
        }
    }
}

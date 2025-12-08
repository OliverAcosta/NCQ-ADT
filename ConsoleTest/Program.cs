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
            var db = new DbContext();

            foreach (var item in db.statusRepository.All())
            {
                Console.WriteLine(item);
            }

            db.userTaskRepository.Add(new UserTask { Name = "2do Bug en produccion", Description = "Se ha encontrado un bug en produccion el mismo no se ha podido reproducir en ambiente de prueba",
            Created = DateTime.Now, DueDate = DateTime.Now.AddMonths(2), PriorityId = 3, StatusId = 1, UserId = 1, Active = true });
            foreach (var item in db.userTaskRepository.getDTOs(1))
            {
                Console.WriteLine(item);
            }
           
            //string s = PasswordUtility.HashPassword("#Tarea123");
           //Console.WriteLine(s);
           //string ss = QueryUtility.GetUpdateString<UserTask>(["duedate"], "");
           //Console.WriteLine(ss);
        }
    }
}

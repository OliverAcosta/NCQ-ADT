
using Infrastructure.Dal.Repositories;

namespace Infrastructure.Dal
{
    public class DbContext
    {
        public DbContext()
        {
            new DatabaseCreation().createDatabase(); // prueba la correcta creacion de la base de datos.
        }

        public readonly NotesRepository notesRepository = new NotesRepository();
        public readonly PrioritiesRepository prioritiesRepository = new PrioritiesRepository();
        public readonly StatusRepository statusRepository = new StatusRepository();
        public readonly UserRepository userRepository = new UserRepository();
        public readonly UserTaskRepository userTaskRepository = new UserTaskRepository();
    }
}

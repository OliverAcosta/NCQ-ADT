

using Infrastructure.Dal;
using Infrastructure.Entities;
using System.Linq;

namespace NCQ_ADT
{
    public partial class TaskEditor : Form
    {
        public TaskEditor()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-DO");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-DO");
            InitializeComponent();
            InitUI();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InitUI()
        {
            var db = new DbContext();
            var combos = GetAllCombos(this);
            foreach (var item in combos)
            {
                item.DisplayMember = "Name";
                item.ValueMember = "Id";
            }
            //usuarios  
            var users = new List<User>(){ new User { Id = 0, Name = "Selecciona un usuario"} };
            users.AddRange(db.userRepository.All().Where(m=> m.UserType > 2));
            this.cbUsers.DataSource = users;
            //status
            var status = new List<Status>() { new Status { Id = 0, Name = "Selecciona un estado" } };
            status.AddRange(db.statusRepository.All());
            this.cbStatus.DataSource = status;
            //prioridad
            var priorities = new List<Priorities>() { new Priorities { Id = 0, Name = "Selecciona una prioridad" } };
            priorities.AddRange(db.prioritiesRepository.All());
            this.cbPriority.DataSource = priorities;
            //fecha
            this.datepicker.MinDate = DateTime.Now.AddDays(1);
            this.datepicker.Format = DateTimePickerFormat.Short;
        }

        public IEnumerable<ComboBox> GetAllCombos(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ComboBox combo)
                    yield return combo;

                foreach (var child in GetAllCombos(c))
                    yield return child;
            }
        }
    }
}

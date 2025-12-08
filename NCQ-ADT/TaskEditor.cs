

using Infrastructure.Dal;

namespace NCQ_ADT
{
    public partial class TaskEditor : Form
    {
        public TaskEditor()
        {
            InitializeComponent();
            initUI();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void initUI()
        {
            var db = new DbContext();
            var combos = GetAllCombos(this);
            foreach (var item in combos)
            {
                item.DisplayMember = "Name";
                item.ValueMember = "Id";
            }
            //usuarios
            this.cbUsers.DataSource = db.userRepository.All();
            //status
            this.cbStatus.DataSource = db.statusRepository.All();
            //prioridad
            this.cbPriority.DataSource = db.prioritiesRepository.All();
            //fecha
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

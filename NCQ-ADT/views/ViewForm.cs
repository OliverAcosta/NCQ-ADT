using Infrastructure.Dal;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using NCQ_ADT.Helpers;
using System.Data;

namespace NCQ_ADT.views
{
    public partial class ViewForm : Form
    {
        private DbContext db = new DbContext();
        private int userTaskId = 0;
        private List<UserTaskDto> userTaskCollection;
        private bool enabledGridUpdate = true;
        private TextBoxPlaceholder txtsearchDescPlaceholder;
        private TextBoxPlaceholder notesPlaceholder;

        private List<int> statusLastValue = new List<int>();
        private UserTask userTask = new UserTask();
        public ViewForm()
        {
            InitializeComponent();
            this.InitUI();
            this.SetDataGrid();
        }
        private void InitUI()
        {
            Enable(false);
            var combos = GetControls<ComboBox>(this);
            foreach (var item in combos)
            {
                item.DisplayMember = "Name";
                item.ValueMember = "Id";
            }

            this.userTaskCollection = db.userTaskRepository.getDTOs().ToList();
            //usuarios  
            var users = new List<User>() { new User { Id = 0, Name = "Selecciona un usuario" } };
            users.AddRange(db.userRepository.All().Where(m => m.UserType > 2));
            this.cbUsers.DataSource = users;

            //status
            var status = new List<Status>() { new Status { Id = 0, Name = "Selecciona un estado" } };
            status.AddRange(db.statusRepository.All());
            this.cbStatus.DataSource = status;
            this.cbStatus.SelectedValueChanged += cbValueChanged;

            //prioridad
            var priorities = new List<Priorities>() { new Priorities { Id = 0, Name = "Selecciona una prioridad" } };
            priorities.AddRange(db.prioritiesRepository.All());
            this.cbPriority.DataSource = priorities;

            //fecha
            this.datepicker.MinDate = DateTime.Now.AddHours(1);
            this.datepicker.Format = DateTimePickerFormat.Custom;
            this.datepicker.CustomFormat = "dd/MM/yyyy HH:mm";
            //listbox
            this.listNotes.HorizontalScrollbar = true;
            notesPlaceholder = new TextBoxPlaceholder(txtAddNotes, "Agrega una nota aqui!");
            //filters
            this.cbSearchStatus.DataSource = new List<Status>(status);
            this.cbSearchUser.DataSource = new List<User>(users);
            this.cbSearchPriority.DataSource = new List<Priorities>(priorities);
            this.cbSearchStatus.SelectedValueChanged += Filter;
            this.cbSearchUser.SelectedValueChanged += Filter;
            this.cbSearchPriority.SelectedValueChanged += Filter;
            this.txtsearchDescPlaceholder = new TextBoxPlaceholder(this.txtSearchDescription, "Buscar elementos por su descripcion");
            //dates
            this.cbSearchDates.DisplayMember = "Date";
            this.cbSearchDates.ValueMember = "Id";
            this.cbSearchDates.DataSource = getDates();
            this.cbSearchDates.SelectedValueChanged += Filter;

            this.btnSave.Click += (object sender, EventArgs e) =>
            {
                bool added = false;
                if (IsFormValid())
                {
                    var entity = new UserTask
                    {
                        Id = userTaskId,
                        Name = string.Empty,
                        Description = this.txtDescription.Text.Trim(),
                        UserId = (int)cbUsers.SelectedValue,
                        StatusId = (int)cbStatus.SelectedValue,
                        PriorityId = (int)cbPriority.SelectedValue,
                        Created = DateTime.Now,
                        DueDate = datepicker.Value,
                        Active = true
                    };

                    if (userTask.Equals(entity))
                    {
                        setErrors(true, "No existen cambio en esta tarea para ser guardada nuevamente!");
                        return;
                    }
                    
                    enabledGridUpdate = false;
                    setErrors();
                    

                    if (userTaskId == 0) 
                    { 
                        entity = db.userTaskRepository.Add(entity); 
                        userTask = entity;
                        added = true;
                    }
                    else {
                        db.userTaskRepository.Update(entity);
                        added = false;
                    }
                    
                    Enable(false);

                    UpdateDatagrid();
                    int index = FindIndex(entity.Id);
                    this.datagrid.Rows[index].Selected = true;
                    this.userTaskId = entity.Id;
                    enabledGridUpdate = true;
                    ShowData();
                    setErrors(false, added ? "Tarea agregada con exito!" : "Tarea actualizada!");
                }
                else 
                { this.setErrors(true, "Debes llenar los controles correctamente, verifica cada uno de ellos!"); }
            };

            this.btnNew.Click += (object sender, EventArgs e) =>
            {
                if (!HasFormChanged())
                {
                    statusLastValue.Clear();
                    setErrors(false);
                    Clear();
                    Enable(true);
                    txtAddNotes.Enabled = false;
                    btnAddNotes.Enabled = false;
                    btnDelete.Enabled = false;
                    cbStatus.Enabled = false;
                    statusLastValue.Add(0);
                    cbStatus.SelectedValue = 1;// pone el estado como pendiente.
                    EnableStatus(false);
                }
            };

            this.btnDelete.Click += (object sender, EventArgs e) =>
            {
                if (this.userTaskId == 0) { MessageBox.Show("Selecciona un elemento del datagrid", "Error", MessageBoxButtons.OK); }
                 if (Convert.ToInt32(cbStatus.SelectedValue)  != 2)
                {
                    db.notesRepository.Delete(this.userTaskId);
                    db.userTaskRepository.Delete(this.userTaskId);
                    userTask = new UserTask();
                    statusLastValue.Clear();
                    Clear();
                    Enable(false);
                    this.setErrors(false, string.Format("Se ha eliminado un registro con la Descripcion:{0}, Prioridad:{1}", this.txtDescription.Text, this.cbPriority.Text));
                    UpdateDatagrid();
                    
                } 
                else if(Convert.ToInt32(cbStatus.SelectedValue) == 2)
                {
                    setErrors(true, "No se puede eliminar una tarea en Proceso");
                }
            };

            this.btnAddNotes.Click += (object sender, EventArgs e) =>
            {
                if (this.userTaskId != 0)
                {
                    var entity = this.AddNote();
                    if (entity != null)
                    {
                        listNotes.Items.Add(entity);
                    }
                }
            };

            this.btnSearchClear.Click += (object sender, EventArgs e) => {
                this.enabledGridUpdate = false;
                this.clearSearch();
                this.enabledGridUpdate = true;
                this.UpdateDatagrid();
            };

            this.txtSearchDescription.TextChanged += (object sender, EventArgs e) => {
                if (!this.txtsearchDescPlaceholder.Has && txtSearchDescription.Text.Length >= 2)
                {
                    Filter(sender, e);
                }
                else if (datagrid.Rows.Count != this.userTaskCollection.Count && this.enabledGridUpdate && !HasFilterAppliyed())
                {
                    UpdateDatagrid();
                }
            };
            //form
            this.Load += Form1_Load;
        }
        public void cbValueChanged(object sender, EventArgs e)
        {
            setErrors(false);

            int selectedValue = ((int)this.cbStatus.SelectedValue);
            int lastValue = statusLastValue[0];
            if (statusLastValue.Count == 1 && (lastValue+1) == selectedValue)
            {
                statusLastValue.Add(selectedValue);
            }
            else
            {
                this.changeStatusOverload(false);
                this.cbStatus.SelectedValue = statusLastValue[statusLastValue.Count - 1];
                this.setErrors(true, "EL FLUJO DE LAS TAREAS ES:\nPENDIENTE > EN PROCESO > TERMINADA");
                this.changeStatusOverload(true);
            }

        }
        private List<DateDto> getDates()
        {
            var dates = new List<DateDto>() { new DateDto{ Id = 0, Date = "Selecciona un fecha" } };
            dates.AddRange( db.userTaskRepository.getDates().
            Select((m, i) => new DateDto { Id = i + 1, Date = m.Date }).ToList());
            return dates;
        }
        private void Filter(object sender, EventArgs e)
        {
            var dtos = db.userTaskRepository.getDTOs().AsQueryable();
            var query = dtos.AsQueryable();
            int userid = Convert.ToInt32(this.cbSearchUser.SelectedValue);
            int statusId = Convert.ToInt32(this.cbSearchStatus.SelectedValue);
            int pId = Convert.ToInt32(this.cbSearchPriority.SelectedValue);
            int dateId = Convert.ToInt32(this.cbSearchDates.SelectedValue);

            if (userid > 0) { query = query.Where(m => m.UserId == userid); }
            if (statusId > 0) { query = query.Where(m => m.StatusId == statusId); }
            if (pId > 0) { query = query.Where(m => m.PriorityId == pId); }
            if(dateId > 0) { query = query.Where(m => m.DueDate.Date == DateTime.Parse(this.cbSearchDates.Text)); }
            if (!this.txtsearchDescPlaceholder.Has && txtSearchDescription.Text.Length >= 2 ) 
            { query = query.Where(m=> m.Description.Contains(txtSearchDescription.Text, StringComparison.OrdinalIgnoreCase)); }
            var values = query.ToList();
            this.datagrid.DataSource = values;
        }
        private bool HasFilterAppliyed()
        {
            return Convert.ToInt32(this.cbSearchUser.SelectedValue) > 0 ||
            Convert.ToInt32(this.cbSearchStatus.SelectedValue) > 0 ||
            Convert.ToInt32(this.cbSearchPriority.SelectedValue) > 0 ||
            Convert.ToInt32(this.cbSearchDates.SelectedValue) > 0;
        }
        public IEnumerable<T> GetControls<T>(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T control)
                    yield return control;

                foreach (var child in GetControls<T>(c))
                    yield return child;
            }
        }
        public void Enable(bool state)
        {
            Action<Control> verifySearch = (Control item) => {
                if (item.Name.Contains("Search")) { item.Enabled = true; }
            };
            var textboxes = GetControls<TextBox>(this);
            foreach (var item in textboxes) { item.Enabled = state; verifySearch(item); }
            var combos = GetControls<ComboBox>(this);
            foreach (var item in combos) { item.Enabled = state; verifySearch(item); }
            var buttons = GetControls<Button>(this);
            foreach (var item in buttons) { item.Enabled = state; verifySearch(item); }
            btnNew.Enabled = true;
            datepicker.Enabled = state;
        }
        public void EnableStatus(bool state) { this.cbStatus.Enabled = state; }
        public void AllowStateChange()
        {
            this.cbStatus.Enabled = true;
            this.btnSave.Enabled = true;
        }
        public void AllowDelete(bool state) { this.btnDelete.Enabled = state; }
        public void Clear()
        {
            this.changeStatusOverload(false);
            var textboxes = GetControls<TextBox>(this).Where(c=> !c.Name.Contains("Search"));
            foreach (var item in textboxes) { item.Text = string.Empty; }
            var combos = GetControls<ComboBox>(this);
            foreach(var item in combos) { item.SelectedValue = 0; }
            listNotes.Items.Clear();
            this.userTaskId = 0;
            this.datepicker.MinDate = DateTime.Now.AddHours(1);
            this.changeStatusOverload(true);
        }
        public void changeStatusOverload(bool overload)
        {
            if (overload)
            {
                this.cbStatus.SelectedValueChanged += cbValueChanged;
            }
            else
            {
                this.cbStatus.SelectedValueChanged -= cbValueChanged;
            }
        }
        public void clearSearch()
        {
            this.cbSearchDates.SelectedIndex = 0;
            this.cbSearchPriority.SelectedIndex = 0;
            this.cbSearchStatus.SelectedIndex = 0;
            this.cbSearchUser.SelectedIndex = 0;
            this.txtsearchDescPlaceholder.SetPlaceHolder("Buscar elementos por su descripcion");
        }
        public void SetDataGrid()
        {
            this.datagrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            this.datagrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.datagrid.AllowUserToAddRows = false;
            this.datagrid.AutoGenerateColumns = false;
            this.datagrid.MultiSelect = false;
            this.datagrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id"   
            });

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Description",
                HeaderText = "Descripción",
                DataPropertyName = "Description"
            });

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "User",
                HeaderText = "Usuario",
                DataPropertyName = "User"
            });

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Estado",
                HeaderText = "Estado",
                DataPropertyName = "Status"
            });

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Priority",
                HeaderText = "Prioridad",
                DataPropertyName = "Priority"
            });

            this.datagrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DueDate",
                HeaderText = "Fecha de compromiso",
                DataPropertyName = "DueDate"
            });
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDatagrid();
            this.datagrid.ClearSelection();
            this.datagrid.SelectionChanged += DataGrid_Selection;
        }
        private void DataGrid_Selection(object sender, EventArgs e)
        {
            if (this.datagrid.CurrentRow != null)
            {
                var value = this.datagrid.CurrentRow.Cells[0].Value;
                if(value != null)
                {
                    if (enabledGridUpdate)
                    {
                        this.userTaskId = Convert.ToInt32(value);
                        ShowData();
                    }
                }
            }
        }
        private void ShowData()
        {
            setErrors(false);
            var entity = this.db.userTaskRepository.Get(userTaskId);
            if (entity != null) {
                this.changeStatusOverload(false); // removiendo la detencion del status
                userTask = entity;
                statusLastValue.Clear();
                statusLastValue.Add(entity.StatusId);
                this.cbStatus.SelectedValue = entity.StatusId;
                this.changeStatusOverload(true); //

                this.txtDescription.Text = entity.Description;
                this.cbUsers.SelectedValue = entity.UserId;
                this.cbPriority.SelectedValue = entity.PriorityId;
                this.datepicker.MinDate = new DateTime(1975,1,1);
                this.datepicker.Value = entity.DueDate;
                this.txtAddNotes.Enabled = true;
                this.listNotes.Enabled = true;
                this.btnAddNotes.Enabled = true;
                ShowNotes();
                if (DateTime.Now > entity.DueDate)
                {
                    Enable(false);
                    setErrors(true, "Esta tarea se encuentra expirada!");
                }
                else
                {
                    bool allow = entity.StatusId == 1;
                    Enable(allow);
                    this.AllowAddNotes(entity.StatusId != 3);
                    if (entity.StatusId == 2) { AllowStateChange(); }
                    if (entity.StatusId != 2) AllowDelete(true);
                    
                }
            }
        }
        private void UpdateDatagrid() { 
            this.userTaskCollection =  db.userTaskRepository.getDTOs().OrderBy(m=> m.DueDate).ToList<UserTaskDto>();
            this.datagrid.DataSource = userTaskCollection;
        }
        public int FindIndex(int id) { return userTaskCollection.FindIndex(m=> m.Id == id); }
        public bool IsFormValid()
        {
            return (txtDescription.Text.Length > 0 && ((User) cbUsers.SelectedItem).Id > 0 && ((Status)cbStatus.SelectedItem).Id > 0 
                && ((Priorities)cbPriority.SelectedItem).Id > 0);
        }

        public bool HasFormChanged()
        {
            return (txtDescription.Text.Length > 0 || ((User)cbUsers.SelectedItem).Id > 0 || ((Status)cbStatus.SelectedItem).Id > 0
                || ((Priorities)cbPriority.SelectedItem).Id > 0);
        }
        public void setErrors(bool error = false, string msg = "")
        {
            if (!error)
            {
                lblError.ForeColor = Color.Green;
                lblError.Text = msg;

            }
            else
            {
                lblError.ForeColor = Color.IndianRed;
                lblError.Text = msg;
            }
        }
        public void AllowAddNotes(bool state = true)
        {
            this.txtAddNotes.Enabled = state;
            this.btnAddNotes.Enabled = state;
        }
        public void ShowNotes()
        {
            listNotes.Items.Clear();
            var range = db.notesRepository.GetRange(this.userTaskId).Select(m=> m.ToString()).ToArray();
            if (range.Any()) {
                foreach (var item in range) { this.listNotes.Items.Add(item); }
            }
        }
        public Notes AddNote()
        {
            bool hasText = txtAddNotes.Text.Length > 0;
            if (!hasText) { setErrors(true, "Agrega algun texto en el campo de nota"); }
            if (userTaskId != 0 && hasText)
            {
                var entity = new Notes { UserTaskId = this.userTaskId, Note = this.txtAddNotes.Text, Active = true, Created = DateTime.Now };
                entity = this.db.notesRepository.Add(entity);
                txtAddNotes.Text = string.Empty;
                return entity;
            }
            return null;
        }
    }
}

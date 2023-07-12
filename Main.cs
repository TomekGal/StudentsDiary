using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace StudentsDiary
{
    public partial class Main : Form
    {

        private delegate void DisplayMessage(string message);

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.filePath);

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;

            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();

            SetColumnHeader();

            if (IsMaximize)

                WindowState = FormWindowState.Maximized;

        }

       
        private void RefreshDiary()
        {

            var students = _fileHelper.DeserializerFromFile();
            dgvDiary.DataSource = students;
        }

        private void SetColumnHeader()
        {

            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Matematyka";
            dgvDiary.Columns[4].HeaderText = "Technologia";
            dgvDiary.Columns[5].HeaderText = "Fizyka";
            dgvDiary.Columns[6].HeaderText = "jPolski";
            dgvDiary.Columns[7].HeaderText = "jAngielski";
            dgvDiary.Columns[8].HeaderText = "Komentarz";
            dgvDiary.Columns[9].HeaderText = "Dodatkowe Zajęcia";

        }


        private void btnAdd_Click(object sender, EventArgs e)
        {

            var addEditStudent = new AddEditStudents();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        

        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia do edycji");
                return;
            }

            var addEditStudent = new AddEditStudents
                (Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia");
                return;

            }


            var selectedStudent = dgvDiary.SelectedRows[0];
            var confirmDelete =
                MessageBox.Show($"Czy chcesz usunąć ucznia",

                "Usuwanie ucznia",
            MessageBoxButtons.OKCancel);
            if (confirmDelete == DialogResult.OK)
            {

                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }


        }

        private void DeleteStudent(int id)
        {

            var students = _fileHelper.DeserializerFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
            dgvDiary.DataSource = students;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializerFromFile();

            if (comFilter.Text != "Wszystko")
            {

                students.RemoveAll(x => x.IdClass != comFilter.Text);
                dgvDiary.DataSource = students;
            }
            dgvDiary.DataSource = students;
            //RefreshDiary();

        }

      private void Main_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }

        //private void comFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
            
        //    //var students = _fileHelper.DeserializerFromFile();
          
        //    //if (comFilter.Text != "All")
        //    //{
               
        //    //    students.RemoveAll(x => x.IdClass != comFilter.Text);
        //    //    dgvDiary.DataSource = students;
        //    //}
        //    //dgvDiary.DataSource = students;
        //}
    }
}

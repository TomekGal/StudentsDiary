using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StudentsDiary
{
    public partial class AddEditStudents : Form
    {
        

        private int _studentId;
        private Student _student;
       

        private FileHelper<List<Student>> _fileHelper =
           new FileHelper<List<Student>>(Program.filePath);

        public AddEditStudents(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
           
            GetStudentData();
            tbFirstName.Select();

        }

        private void GetStudentData()
        {

            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializerFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym ID");
             
                FillTextBoxes();

            }

        }

        private void FillTextBoxes()
            {
                tbID.Text = _student.Id.ToString();
                tbFirstName.Text = _student.FirstName;
                tbLastName.Text = _student.LastName;
                tbMat.Text = _student.Math;
                tbPhysic.Text = _student.Physics;
                tbTechnology.Text = _student.Technology;
                tbPolishLang.Text = _student.PolishLang;
                tbForeginLang.Text = _student.ForeignLang;
                tbComments.Text = _student.Comments;
                checkActivities.Checked = _student.Activities;
                comIdClass.Text = _student.IdClass;
            
        }
 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudents = new AddEditStudents();
            addEditStudents.ShowDialog();
            
        }

       

        private  void btnConfirm_Click(object sender, EventArgs e)
            {
            var students = _fileHelper.DeserializerFromFile();

            if (_studentId != 0)
               students.RemoveAll(x => x.Id == _studentId);
            else
               AssignIdToNewStudent(students);
           
            AddNewuserToList(students);
                       
            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewuserToList(List<Student > students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = tbComments.Text,
                ForeignLang = tbForeginLang.Text,
                Math = tbMat.Text,
                Physics = tbPhysic.Text,
                PolishLang = tbPolishLang.Text,
                Technology = tbTechnology.Text,
                Activities = checkActivities.Checked,
                IdClass = comIdClass.Text,

            };
            students.Add(student);
        }

            private void btnCancel_Click(object sender, EventArgs e)
            {
            Close();
            }

        private void AssignIdToNewStudent(List <Student> students)
        {
            var studentWithHighestId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 :
               studentWithHighestId.Id + 1;

        }

        private void IdClass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

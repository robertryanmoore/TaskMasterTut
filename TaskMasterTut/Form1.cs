using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskMasterTut.Model;

namespace TaskMasterTut
{
    public partial class Form1 : Form
    {
        private tmDBcontext tmContext;
        public Form1()
        {
            InitializeComponent();
            tmContext = new tmDBcontext();

            var statuses = tmContext.Statuses.ToList();

            foreach (Status s in statuses)
            {
                cbxStatus.Items.Add(s);
            }
            refreshData();
        }

        private void refreshData()
        {
            BindingSource bi = new BindingSource();
            var query = from t in tmContext.Tasks
                        orderby t.Due
                        select new { t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.Due };

            bi.DataSource = query.ToList();
            dataGridView1.DataSource = bi;
            dataGridView1.Refresh();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cbxStatus.SelectedItem != null && txtTask.Text != String.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cbxStatus.SelectedItem as Model.Status).Id,
                    Due = dateTimePicker1.Value
                };

                tmContext.Tasks.Add(newTask);

                tmContext.SaveChanges();
                refreshData();
            }
            else
                MessageBox.Show("Please make sure data has been entered");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
            tmContext.Tasks.Remove(t);
            tmContext.SaveChanges();
            refreshData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdate.Text == "Update") {
                txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString();
                dateTimePicker1.Value =(DateTime)dataGridView1.SelectedCells[3].Value;

                foreach(Status s in cbxStatus.Items)
                {
                    if (s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                    {
                        cbxStatus.SelectedItem = s;
                    }
                    btnUpdate.Text = "Save";
                }
            }
            else if (btnUpdate.Text == "Save")
            {
                var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

                t.Name = txtTask.Text;
                t.StatusId = (cbxStatus.SelectedItem as Status).Id;
                t.Due = dateTimePicker1.Value;

                tmContext.SaveChanges();

                refreshData();

                btnUpdate.Text = "Update";
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cbxStatus.Text = "Please Select...";

            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnUpdate.Text = "Update";
            txtTask.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cbxStatus.Text = "Please Select...";
        }
    }
}

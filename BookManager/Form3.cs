using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookManager
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            if (DataManager.Users.Count > 0)
                dataGridView_Users.DataSource = DataManager.Users;

            dataGridView_Users.CellClick += user_cellclick;

            button_Add.Click += (sender, e) =>
            {
                if (DataManager.Users.Exists((x) => x.Id == int.Parse(textBox_ID.Text)))
                    MessageBox.Show("이미 있음");
                else
                {
                    User user = new User() { Id = int.Parse(textBox_ID.Text), Name = textBox_NAME.Text };
                    DataManager.Users.Add(user);

                    dataGridView_Users.DataSource = null;
                    dataGridView_Users.DataSource = DataManager.Users;
                    DataManager.Save();
                }
            };

            button_Modify.Click += delegate (object sender, EventArgs e)
            {
                try
                {
                    User user = DataManager.Users.Single((x) => x.Id == int.Parse(textBox_ID.Text));
                    user.Name = textBox_NAME.Text;

                    try
                    {
                        Book book = DataManager.Books.Single((x) => x.UserId == int.Parse(textBox_ID.Text));
                        book.UserName = textBox_NAME.Text;
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(textBox_ID.Text+"는 없는 아이디에요.");
                }
                dataGridView_Users.DataSource = null;
                dataGridView_Users.DataSource = DataManager.Users;
                DataManager.Save();
            };
            button_Delete.Click += btn_delete;

        }

        private void btn_delete(object sender, EventArgs e)
        {
            try
            {
                User user = DataManager.Users.Single((x) => x.Id == int.Parse(textBox_ID.Text));
                DataManager.Users.Remove(user);

                dataGridView_Users.DataSource = null;
                if (DataManager.Users.Count > 0)
                    dataGridView_Users.DataSource = DataManager.Users;

                DataManager.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("해당 아이디의 사용자는 없습니다.");
            }
        }

        private void user_cellclick(object sender, DataGridViewCellEventArgs e)
        {
            User user = dataGridView_Users.CurrentRow.DataBoundItem as User;
            textBox_ID.Text = user.Id.ToString();
            textBox_NAME.Text = user.Name;
        }
    }
}

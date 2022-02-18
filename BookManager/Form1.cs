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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label_allBookCount.Text = DataManager.Books.Count.ToString();
            label_allUserCount.Text = DataManager.Users.Count.ToString();
            label_allBorrowedBook.Text = DataManager.Books.Where(x => x.isBorrowed).Count().ToString();
            label_allDelayedBook.Text = DataManager.Books.Where(
                delegate (Book x) { return x.isBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now; }).Count().ToString();

            if (DataManager.Users.Count > 0)
                dataGridView_UserManager.DataSource = DataManager.Users;

            if(DataManager.Users.Count > 0)
                dataGridView_BookManager.DataSource = DataManager.Books;

            dataGridView_BookManager.CellClick += Book_GridView_Cellclick;

        }

        private void Book_GridView_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
            Book book = dataGridView_BookManager.CurrentRow.DataBoundItem as Book;
            textBox_isbn.Text = book.Isbn;
            textBox_bookName.Text = book.Name;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
            DataManager.Load();
            dataGridView_BookManager.DataSource = null;
            if (DataManager.Books.Count > 0)
                dataGridView_BookManager.DataSource = DataManager.Books;
        }

        private void 회원관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
            DataManager.Load();
            dataGridView_UserManager.DataSource = null;
            if (DataManager.Users.Count > 0)
                dataGridView_UserManager.DataSource = DataManager.Users;
        }

        private void timer_now_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel_now.Text = DateTime.Now.ToString("yyyy년 MM월 dd일 HH:mm:ss");
        }

        private void dataGridView_UserManager_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            User user = dataGridView_UserManager.CurrentRow.DataBoundItem as User;
            textBox_id.Text = user.Id.ToString();
        }

        private void button_Borrow_Click(object sender, EventArgs e)
        {
            if (textBox_isbn.Text == "")
                MessageBox.Show("isbn입력하세요.");
            else if (textBox_id.Text.Trim() == "")
                MessageBox.Show("사용자 id  입력하세요.");
            else
            {
                try
                {
                    Book book = DataManager.Books.Single((x) => x.Isbn == textBox_isbn.Text);
                    if(book.isBorrowed)
                        MessageBox.Show("이미 빌렸다.");
                    else
                    {
                        User user = DataManager.Users.Single((x) => x.Id.ToString() == textBox_id.Text);
                        book.UserId = user.Id;
                        book.UserName = user.Name;
                        book.isBorrowed = true;
                        book.BorrowedAt = DateTime.Now;

                        dataGridView_BookManager.DataSource = null;
                        dataGridView_BookManager.DataSource = DataManager.Books;
                        DataManager.Save();
                        MessageBox.Show($"{book.Name}이/가 {user.Name}님께 대여되었습니다.");

                        label_allBookCount.Text = DataManager.Books.Count.ToString();
                        label_allUserCount.Text = DataManager.Users.Count.ToString();
                        label_allBorrowedBook.Text = DataManager.Books.Where(x => x.isBorrowed).Count().ToString();
                        label_allDelayedBook.Text = DataManager.Books.Where(
                            delegate (Book x) { return x.isBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now; }).Count().ToString();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("존재하지 않는 도서 혹은 사용자입니다.");
                }
            }
        }

        private void button_Return_Click(object sender, EventArgs e)
        {
            if(textBox_isbn.Text.Trim()=="")
                MessageBox.Show("isbn입력하세요.");
            else
            {
                try
                {
                    Book book = DataManager.Books.Single((x) => x.Isbn == textBox_isbn.Text);
                    if(book.isBorrowed)
                    {
                        DateTime oldDay = book.BorrowedAt;
                        book.UserId = 0;
                        book.UserName = "";
                        book.isBorrowed = false;
                        book.BorrowedAt = new DateTime();

                        dataGridView_BookManager.DataSource = null;
                        dataGridView_BookManager.DataSource = DataManager.Books;
                        DataManager.Save();

                        TimeSpan timeDiff = DateTime.Now - oldDay;
                        if (timeDiff.Days > 7)
                            MessageBox.Show(book.Name+"은 연체상태로 반납");
                        else
                            MessageBox.Show(book.Name+" 정상 반납");

                        label_allBookCount.Text = DataManager.Books.Count.ToString();
                        label_allUserCount.Text = DataManager.Users.Count.ToString();
                        label_allBorrowedBook.Text = DataManager.Books.Where(x => x.isBorrowed).Count().ToString();
                        label_allDelayedBook.Text = DataManager.Books.Where(
                            delegate (Book x) { return x.isBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now; }).Count().ToString();
                    }
                    else
                    {
                        MessageBox.Show("대여 상태가 아닙니다.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("존재하지 않는 도서입니다.");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}

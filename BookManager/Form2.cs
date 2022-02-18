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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if(DataManager.Books.Count>0)
                dataGridView_book.DataSource = DataManager.Books;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            bool existBook = false;

            foreach (var item in DataManager.Books)
            {
                if(item.Isbn == textBox_Isbn.Text)
                {
                    existBook = true;
                    break;
                }
            }
            if(existBook)
                MessageBox.Show("이미 존재하는 도서!");
            else
            {
                Book book = new Book();
                book.Isbn = textBox_Isbn.Text;
                book.Name = textBox_bookName.Text;
                book.Publisher = textBox_publisher.Text;
                book.Page = int.Parse(textBox_page.Text);
                DataManager.Books.Add(book);

                dataGridView_book.DataSource = null;
                dataGridView_book.DataSource = DataManager.Books;
                DataManager.Save();

            }
        }

        private void button_modify_Click(object sender, EventArgs e)
        {
            Book book = null;
            for(int i = 0; i < DataManager.Books.Count; i++)
            {
                if (DataManager.Books[i].Isbn == textBox_Isbn.Text) 
                {
                    book = DataManager.Books[i];
                    book.Name = textBox_bookName.Text;
                    book.Publisher = textBox_publisher.Text;
                    book.Page = int.Parse(textBox_page.Text);

                    dataGridView_book.DataSource = null;
                    dataGridView_book.DataSource = DataManager.Books;
                    DataManager.Save();
                }
            }
            if (book == null)
                MessageBox.Show("존재하지 않는 도서입니다.");
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            bool existBook = false;
            for (int i = 0; i < DataManager.Books.Count; i++)
            {
                if(DataManager.Books[i].Isbn == textBox_Isbn.Text)
                {
                    DataManager.Books.RemoveAt(i);
                    existBook = true;
                }
            }
            if(existBook == false)
                MessageBox.Show("없는 책입니다.");
            else
            {
                dataGridView_book.DataSource = null;
                if (DataManager.Books.Count > 0)
                    dataGridView_book.DataSource = DataManager.Books;
                DataManager.Save();
            }
        }

        private void dataGridView_book_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Book book = dataGridView_book.CurrentRow.DataBoundItem as Book;
            textBox_Isbn.Text = book.Isbn;
            textBox_bookName.Text = book.Name;
            textBox_publisher.Text = book.Publisher;
            textBox_page.Text = book.Page.ToString();
        }
    }
}

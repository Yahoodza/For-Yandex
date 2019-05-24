/*  Форма "Арендаторы".
 *  Название: RenterForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма выводит информацию об арендаторах.
 *  Переменные, используемые в форме:
 *      ds - информация из БД;
 *      Adapter - подключение к БД;
 *      Connection - подключение к SQL-серверу;
 *      k - номер строки datagridview;
 *      kod - первичный ключ строки таблицы БД;
 *      sqlMain - SQL-запрос.
 *  Функции, используемые в форме:
 *      DataGridCellBlock - блокировка редактирования ячеек dataGridView;
 *      RenterForm - конструктор формы;
 *      button1_Click - добавление новой строки в таблицу;
 *      button2_Click - сохранение данных в БД;
 *      button3_Click - изменение строки таблицы;
 *      button4_Click - удаление строки в таблице;
 *      button5_Click - отмена действия.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Rent_car
{
    public partial class RenterForm : Form
    {
        DataSet ds;
        SqlDataAdapter Adapter;
        SqlConnection Connection = new SqlConnection(Helper.ConnectionString);
        public int k;
        string kod;
        string sqlMain = "SELECT ID_Renter AS 'Идентификатор', RTRIM(Surname) AS 'Фамилия', RTRIM(Name) AS 'Имя', RTRIM(Fullname) AS 'Отчество', " +
                         "RTRIM(Adres) AS 'Адрес клиента', RTRIM(Phone) AS 'Телефоен клиента' FROM Renter";

        /*  DataGridCellBlock - блокировка редактирования ячеек dataGridView. 
         *  Локальная переменная:
         *      i - счётчик для цикла.
         */
        public void DataGridCellBlock()
        {
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)                           //Цикл блокировки
            {                                                                               //редактирования ячеек
                dataGridView1.Rows[i].ReadOnly = true;                                      //dataGridView
            }
        }

        /*  RenterForm - конструктор формы. 
         *  Локальные переменные:
         *      i - счётчик для цикла;
         *      Connection - подключение к SQL-серверу.
         */
        public RenterForm()
        {
            InitializeComponent();

            button1.BringToFront();
            button1.Tag = 0;
            button3.Tag = 0;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            using (SqlConnection Connection = new SqlConnection(Helper.ConnectionString))   //Настройка SQL-соединения
            {
                Connection.Open();

                Adapter = new SqlDataAdapter(sqlMain, Connection);

                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.ReadOnly = true;
            }
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
            for (int i = 0; i <= dataGridView1.ColumnCount - 1; i++)                        //Цикл блокировки сортировки
            {                                                                               //столбцов dataGridView
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /*  button1_Click - добавление новой строки в таблицу.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      row - новая строка dataGridView.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            k = dataGridView1.RowCount;
            dataGridView1.ReadOnly = false;
            DataGridCellBlock();
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);
            dataGridView1.Columns[0].ReadOnly = true;

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button1.Tag = 1;

            ID_Renter.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }

        /*  button2_Click - сохранение данных в БД.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;

            try
            {
                if (Convert.ToInt32(button1.Tag) == 1)                                      //Условие сохранения добавленной строки        
                {
                    if (dataGridView1.Rows[k].Cells[1].Value.ToString() != "" && dataGridView1.Rows[k].Cells[2].Value.ToString() != "" && 
                        dataGridView1.Rows[k].Cells[3].Value.ToString() != "" && dataGridView1.Rows[k].Cells[4].Value.ToString() != "" && 
                        dataGridView1.Rows[k].Cells[5].Value.ToString() != "")              //Условие проверки правильности ввода данных
                    {
                        Helper.Query("INSERT INTO Renter VALUES ('" + dataGridView1.Rows[k].Cells[1].Value + "', '" + dataGridView1.Rows[k].Cells[2].Value + "', '" +
                                     dataGridView1.Rows[k].Cells[3].Value + "', '" + dataGridView1.Rows[k].Cells[4].Value.ToString() + "', '" + 
                                     dataGridView1.Rows[k].Cells[5].Value + "')");
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    if (Convert.ToInt32(button3.Tag) == 1)                                  //Условие сохранения изменённой строки
                    {
                        if (dataGridView1.Rows[k].Cells[1].Value.ToString() != "" && dataGridView1.Rows[k].Cells[2].Value.ToString() != "" && 
                            dataGridView1.Rows[k].Cells[3].Value.ToString() != "" && dataGridView1.Rows[k].Cells[4].Value.ToString() != "" && 
                            dataGridView1.Rows[k].Cells[5].Value.ToString() != "")          //Условие проверки правильности ввода данных
                        {
                            Helper.Query("UPDATE Renter SET Surname = '" + dataGridView1.Rows[k].Cells[1].Value + "', Name = '" + dataGridView1.Rows[k].Cells[2].Value +
                                         "', Fullname = '" + dataGridView1.Rows[k].Cells[3].Value + "', Adres = '" + dataGridView1.Rows[k].Cells[4].Value +
                                         "', Phone = '" + dataGridView1.Rows[k].Cells[5].Value + "' WHERE ID_Renter = " + kod);
                        }
                        else
                            throw new Exception();
                    }
                }

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                ID_Renter.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));

                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
                button1.Tag = 0;
                button3.Tag = 0;
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникли неполадки, убедитесь, что вы ввели данные верно!\n" +
                                "Возможная ошибка:\n" +
                                "Незаполненные поля.", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button3_Click - изменение строки таблицы.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            k = dataGridView1.CurrentRow.Index;
            DataGridCellBlock();
            dataGridView1.Rows[k].ReadOnly = false;
            dataGridView1.Columns[0].ReadOnly = true;

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            button3.Tag = 1;
            kod = ID_Renter.Text;

            ID_Renter.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }

        /*  button4_Click - удаление строки в таблице.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ReadOnly = false;
                Helper.Query("DELETE Renter WHERE ID_Renter = " + ID_Renter.Text);

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                ID_Renter.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Для начала заполните БД для возможности удаления записей!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button5_Click - отмена действия.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = false;
            button1.Tag = 0;
            button3.Tag = 0;

            Adapter = new SqlDataAdapter(sqlMain, Connection);
            ds = new DataSet();
            Adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            ID_Renter.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            ID_Renter.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            ID_Renter.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }
    }
}

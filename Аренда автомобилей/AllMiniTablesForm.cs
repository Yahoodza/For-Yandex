/*  Форма всех параметров автомобиля.
 *  Название: AllMiniTablesForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма выводит информацию о параметрах автомобиля.
 *  Переменные, используемые в форме:
 *      ds - информация из БД;
 *      Adapter - подключение к БД;
 *      Connection - подключение к SQL-серверу;
 *      k - номер строки datagridview;
 *      kod - первичный ключ строки таблицы БД;
 *      sqlMain - SQL-запрос.
 *  Функции, используемые в форме:
 *      DataGridCellBlock - блокировка редактирования ячеек dataGridView;
 *      AllMiniTablesForm - конструктор формы;
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
    public partial class AllMiniTablesForm : Form
    {
        DataSet ds;
        SqlDataAdapter Adapter;
        SqlConnection Connection = new SqlConnection(Helper.ConnectionString);
        public int k;
        string kod;
        string sqlMain = "SELECT RTRIM(" + Helper.sqlstr[1] + ") AS 'Идентификатор', RTRIM(" + Helper.sqlstr[2] + ") AS '" + Helper.sqlstr[3] + 
                         "', RTRIM(" + Helper.sqlstr[4] + ") AS '" + Helper.sqlstr[5] + "' FROM " + Helper.sqlstr[0];

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

        /*  AllMiniTablesForm - конструктор формы. 
         *  Локальные переменные:
         *      i - счётчик для цикла;
         *      Connection - подключение к SQL-серверу.
         */
        public AllMiniTablesForm()
        {
            InitializeComponent();

            this.Text = Helper.sqlstr[3];
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
            AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
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

            AllMiniTables.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
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
                    if (Convert.ToInt32(dataGridView1.Rows[k].Cells[2].Value) >= 0 && Convert.ToInt32(dataGridView1.Rows[k].Cells[2].Value) <= 10 && 
                        dataGridView1.Rows[k].Cells[1].Value.ToString() != "")              //Условие проверки правильности ввода данных
                    {
                        Helper.Query("INSERT INTO " + Helper.sqlstr[0] + " VALUES ('" + dataGridView1.Rows[k].Cells[1].Value + "', " + 
                                     Convert.ToInt32(dataGridView1.Rows[k].Cells[2].Value) + ")");
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    if (Convert.ToInt32(button3.Tag) == 1)                                  //Условие сохранения изменённой строки
                    {
                        if (Convert.ToInt32(dataGridView1.Rows[k].Cells[2].Value) >= 0 && Convert.ToInt32(dataGridView1.Rows[k].Cells[2].Value) <= 10 && 
                            dataGridView1.Rows[k].Cells[1].Value.ToString() != "")          //Условие проверки правильности ввода данных
                        {
                            Helper.Query("UPDATE " + Helper.sqlstr[0] + " SET " + Helper.sqlstr[2] + " = '" + dataGridView1.Rows[k].Cells[1].Value + "', " + 
                                         Helper.sqlstr[4] + " = " + dataGridView1.Rows[k].Cells[2].Value + " WHERE " + Helper.sqlstr[1] + " = " + kod);
                        }
                        else
                            throw new Exception();
                    }
                }

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                AllMiniTables.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));

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
                                "Возможные ошибки:\n" +
                                "1 - Незаполненные поля;\n" +
                                "2 - Шкала оценивания задана не по 10-бальной шкале;\n" +
                                "3 - Некорректный тип данных;\n" +
                                "4 - Такое значение поля уже есть.", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            kod = AllMiniTables.Text;

            AllMiniTables.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
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
                Helper.Query("DELETE " + Helper.sqlstr[0] + " WHERE " + Helper.sqlstr[1] + " = " + AllMiniTables.Text);

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                AllMiniTables.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch
            {
                MessageBox.Show("Данное поле нельзя удалить, так как оно используется в другой таблице!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            AllMiniTables.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            AllMiniTables.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            AllMiniTables.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }
    }
}

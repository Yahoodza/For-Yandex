/*  Форма "Автомобили".
 *  Название: AutomobileForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма выводит данные из SQL-таблицы "Automobile".
 *  Переменные, используемые в форме:
 *      ds - информация из БД;
 *      Adapter - подключение к БД;
 *      Connection - подключение к SQL-серверу;
 *      k - номер строки datagridview;
 *      kod - первичный ключ строки таблицы БД;
 *      index[] - массив первичных ключей записей таблиц БД;
 *      sqlMain - SQL-запрос.
 *  Функции, используемые в форме:
 *      DataGridCellBlock - блокировка редактирования ячеек dataGridView;
 *      AutomobileForm - конструктор формы;
 *      DataGridComboBox - создание элемента управления ComboBox в ячейке dataGridView;
 *      button1_Click - добавление новой строки в таблицу;
 *      button2_Click - сохранение данных в БД;
 *      button3_Click - изменение строки таблицы;
 *      button4_Click - удаление строки в таблице;
 *      button5_Click - открытие формы AllMiniTablesForm с таблицей "Марка автомобиля";
 *      button6_Click - открытие формы AllMiniTablesForm с таблицей "Тип кузова";
 *      button7_Click - открытие формы AllMiniTablesForm с таблицей "Цвет автомобиля";
 *      button8_Click - открытие формы AllMiniTablesForm с таблицей "Тип фар";
 *      button9_Click - открытие формы AllMiniTablesForm с таблицей "Тип привода";
 *      button10_Click - открытие формы AllMiniTablesForm с таблицей "Коробка передач";
 *      button11_Click - открытие формы AllMiniTablesForm с таблицей "Тип двигателя";
 *      button12_Click - открытие формы AllMiniTablesForm с таблицей "Климат-контроль";
 *      button13_Click - отмена действия.
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
    public partial class AutomobileForm : Form
    {
        DataSet ds;
        SqlDataAdapter Adapter;
        SqlConnection Connection = new SqlConnection(Helper.ConnectionString);
        public int k;
        string kod;
        int[] index = new int[8];
        string sqlMain = "SELECT VIN AS 'VIN-код', Name_brand AS 'Марка автомобиля', Name_body AS 'Тип кузова', Name_color AS 'Цвет автомобиля', " +
                         "Name_headlights AS 'Тип фар', Name_drive AS 'Тип привода', Name_transmission AS 'Коробка передач', " +
                         "Name_engine AS 'Тип двигателя', Name_climate AS 'Климат-контроль', Number_of_seats AS 'Количество мест', " +
                         "Cost_of_rent AS 'Цена аренды' FROM Car_brand JOIN Automobile ON Car_brand.Brand_number = Automobile.Car_brand " +
                         "JOIN Body_type ON Body_type.Body_number = Automobile.Body_type JOIN Color ON Color.Color_number = Automobile.Color " +
                         "JOIN Type_of_headlights ON Type_of_headlights.Headlights_number = Automobile.Type_of_headlights JOIN Type_of_drive ON " +
                         "Type_of_drive.Drive_number = Automobile.Type_of_drive JOIN Transmission ON Transmission.Transmission_number " +
                         "= Automobile.Transmission JOIN Engine_type ON Engine_type.Engine_number = Automobile.Engine_type JOIN " +
                         "Climate_control ON Climate_control.Climate_number = Automobile.Climate_control";

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

        /*  AutomobileForm - конструктор формы. 
         *  Локальные переменные:
         *      i - счётчик для цикла;
         *      Connection - подключение к SQL-серверу.
         */
        public AutomobileForm()
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
            VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));
            for (int i = 0; i <= dataGridView1.ColumnCount - 1; i++)                        //Цикл блокировки сортировки
            {                                                                               //столбцов dataGridView
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /*  DataGridComboBox - создание элемента управления ComboBox в ячейке dataGridView.
        *  Формальные параметры:
        *      str1 - название таблицы БД; 
        *      str2 - поле таблицы БД;
        *      i - индекс строки dataGridView;
        *      j - индекс ячейки dataGridView.
        *  Локальные переменные:
        *      Combo - элемент управления ComboBox;
        *      tb1 - SQL таблица.
        */
        public void DataGridComboBox(string str1, string str2, int i, int j)
        {
            Connection.Open();
            DataGridViewComboBoxCell Combo = new DataGridViewComboBoxCell();
            Adapter = new SqlDataAdapter("SELECT * FROM " + str1, Connection);
            DataTable tb1 = new DataTable();
            Adapter.Fill(tb1);
            Combo.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            Combo.FlatStyle = FlatStyle.Standard;
            Combo.DataSource = tb1;
            Combo.DisplayMember = str2;
            dataGridView1.Rows[i].Cells[j] = Combo;
            Connection.Close();
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

            DataGridComboBox("Car_brand", "Name_brand", k, 1);
            DataGridComboBox("Body_type", "Name_body", k, 2);
            DataGridComboBox("Color", "Name_color", k, 3);
            DataGridComboBox("Type_of_headlights", "Name_headlights", k, 4);
            DataGridComboBox("Type_of_drive", "Name_drive", k, 5);
            DataGridComboBox("Transmission", "Name_transmission", k, 6);
            DataGridComboBox("Engine_type", "Name_engine", k, 7);
            DataGridComboBox("Climate_control", "Name_climate", k, 8);
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = true;
            button1.Tag = 1;

            VIN.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));
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
                    index[0] = Helper.DataGridComboBoxPK("SELECT Brand_number FROM Car_brand WHERE Name_brand ='" + dataGridView1.Rows[k].Cells[1].Value.ToString() + "'");
                    index[1] = Helper.DataGridComboBoxPK("SELECT Body_number FROM Body_type WHERE Name_body ='" + dataGridView1.Rows[k].Cells[2].Value.ToString() + "'");
                    index[2] = Helper.DataGridComboBoxPK("SELECT Color_number FROM Color WHERE Name_color ='" + dataGridView1.Rows[k].Cells[3].Value.ToString() + "'");
                    index[3] = Helper.DataGridComboBoxPK("SELECT Headlights_number FROM Type_of_headlights WHERE Name_headlights ='" + dataGridView1.Rows[k].Cells[4].Value.ToString() + "'");
                    index[4] = Helper.DataGridComboBoxPK("SELECT Drive_number FROM Type_of_drive WHERE Name_drive ='" + dataGridView1.Rows[k].Cells[5].Value.ToString() + "'");
                    index[5] = Helper.DataGridComboBoxPK("SELECT Transmission_number FROM Transmission WHERE Name_transmission ='" + dataGridView1.Rows[k].Cells[6].Value.ToString() + "'");
                    index[6] = Helper.DataGridComboBoxPK("SELECT Engine_number FROM Engine_type WHERE Name_engine ='" + dataGridView1.Rows[k].Cells[7].Value.ToString() + "'");
                    index[7] = Helper.DataGridComboBoxPK("SELECT Climate_number FROM Climate_control WHERE Name_climate ='" + dataGridView1.Rows[k].Cells[8].Value.ToString() + "'");

                    if (Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 2 || Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 4 || 
                        Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 5 || Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 7 || 
                        Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 8)         //Условие проверки правильности ввода данных
                    {
                        Helper.Query("INSERT INTO Automobile VALUES (" + index[0] + ", " + index[1] + ", " + index[2] + ", " + index[3] + ", " + index[4] + ", "
                                        + index[5] + ", " + index[6] + ", " + index[7] + ", " + dataGridView1.Rows[k].Cells[9].Value + ", " + dataGridView1.Rows[k].Cells[10].Value + ", 1)");
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    if (Convert.ToInt32(button3.Tag) == 1)                                  //Условие сохранения изменённой строки
                    {
                        index[0] = Helper.DataGridComboBoxPK("SELECT Brand_number FROM Car_brand WHERE Name_brand ='" + dataGridView1.Rows[k].Cells[1].Value.ToString() + "'");
                        index[1] = Helper.DataGridComboBoxPK("SELECT Body_number FROM Body_type WHERE Name_body ='" + dataGridView1.Rows[k].Cells[2].Value.ToString() + "'");
                        index[2] = Helper.DataGridComboBoxPK("SELECT Color_number FROM Color WHERE Name_color ='" + dataGridView1.Rows[k].Cells[3].Value.ToString() + "'");
                        index[3] = Helper.DataGridComboBoxPK("SELECT Headlights_number FROM Type_of_headlights WHERE Name_headlights ='" + dataGridView1.Rows[k].Cells[4].Value.ToString() + "'");
                        index[4] = Helper.DataGridComboBoxPK("SELECT Drive_number FROM Type_of_drive WHERE Name_drive ='" + dataGridView1.Rows[k].Cells[5].Value.ToString() + "'");
                        index[5] = Helper.DataGridComboBoxPK("SELECT Transmission_number FROM Transmission WHERE Name_transmission ='" + dataGridView1.Rows[k].Cells[6].Value.ToString() + "'");
                        index[6] = Helper.DataGridComboBoxPK("SELECT Engine_number FROM Engine_type WHERE Name_engine ='" + dataGridView1.Rows[k].Cells[7].Value.ToString() + "'");
                        index[7] = Helper.DataGridComboBoxPK("SELECT Climate_number FROM Climate_control WHERE Name_climate ='" + dataGridView1.Rows[k].Cells[8].Value.ToString() + "'");

                        if (Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 2 || Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 4 || 
                            Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 5 || Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 7 || 
                            Convert.ToInt32(dataGridView1.Rows[k].Cells[9].Value) == 8)     //Условие проверки правильности ввода данных
                        {
                            Helper.Query("UPDATE Automobile SET Car_brand = " + index[0] + ", Body_type = " + index[1] + ", Color = " + index[2] + ", Type_of_headlights = "
                                            + index[3] + ", Type_of_drive = " + index[4] + ", Transmission = " + index[5] + ", Engine_type = " + index[6] + ", Climate_control = "
                                            + index[7] + ", Number_of_seats = " + dataGridView1.Rows[k].Cells[9].Value + ", Cost_of_rent = " + Convert.ToInt32(dataGridView1.Rows[k].Cells[10].Value) +
                                            " WHERE VIN = " + kod);
                        }
                        else
                            throw new Exception();
                    }
                }

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                VIN.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));

                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
                button10.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                button1.Tag = 0;
                button3.Tag = 0;
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникли неполадки, убедитесь, что вы ввели данные верно!\n" +
                                "Возможная ошибка:\n" +
                                "1 - Незаполненные поля;\n" +
                                "2 - Некорректный тип данных;\n" +
                                "3 - Неверно указанное количество мест.", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DataGridComboBox("Car_brand", "Name_brand", k, 1);
            DataGridComboBox("Body_type", "Name_body", k, 2);
            DataGridComboBox("Color", "Name_color", k, 3);
            DataGridComboBox("Type_of_headlights", "Name_headlights", k, 4);
            DataGridComboBox("Type_of_drive", "Name_drive", k, 5);
            DataGridComboBox("Transmission", "Name_transmission", k, 6);
            DataGridComboBox("Engine_type", "Name_engine", k, 7);
            DataGridComboBox("Climate_control", "Name_climate", k, 8);
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = true;
            button3.Tag = 1;
            kod = VIN.Text;

            VIN.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));
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
                Helper.Query("DELETE Automobile WHERE VIN = " + VIN.Text);

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                VIN.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Для начала заполните БД для возможности удаления записей!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button5_Click - открытие формы "Марка автомобиля".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Марка автомобиля".
         */
        private void button5_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Car_brand";
            Helper.sqlstr[1] = "Brand_number";
            Helper.sqlstr[2] = "Name_brand";
            Helper.sqlstr[3] = "Марка автомобиля";
            Helper.sqlstr[4] = "Brand_rating";
            Helper.sqlstr[5] = "Рейтинг марки";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button6_Click - открытие формы "Тип кузова".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Тип кузова".
         */
        private void button6_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Body_type";
            Helper.sqlstr[1] = "Body_number";
            Helper.sqlstr[2] = "Name_body";
            Helper.sqlstr[3] = "Тип кузова";
            Helper.sqlstr[4] = "Body_rating";
            Helper.sqlstr[5] = "Рейтинг кузова";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button7_Click - открытие формы "Цвет автомобиля".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Цвет автомобиля".
         */
        private void button7_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Color";
            Helper.sqlstr[1] = "Color_number";
            Helper.sqlstr[2] = "Name_color";
            Helper.sqlstr[3] = "Цвет автомобиля";
            Helper.sqlstr[4] = "Color_rating";
            Helper.sqlstr[5] = "Рейтинг цвета";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button8_Click - открытие формы "Тип фар".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Тип фар".
         */
        private void button8_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Type_of_headlights";
            Helper.sqlstr[1] = "Headlights_number";
            Helper.sqlstr[2] = "Name_headlights";
            Helper.sqlstr[3] = "Тип фар";
            Helper.sqlstr[4] = "Headlights_rating";
            Helper.sqlstr[5] = "Рейтинг фар";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button9_Click - открытие формы "Тип привода".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Тип привода".
         */
        private void button9_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Type_of_drive";
            Helper.sqlstr[1] = "Drive_number";
            Helper.sqlstr[2] = "Name_drive";
            Helper.sqlstr[3] = "Тип привода";
            Helper.sqlstr[4] = "Drive_rating";
            Helper.sqlstr[5] = "Рейтинг привода";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button10_Click - открытие формы "Коробка передач".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Коробка передач".
         */
        private void button10_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Transmission";
            Helper.sqlstr[1] = "Transmission_number";
            Helper.sqlstr[2] = "Name_transmission";
            Helper.sqlstr[3] = "Коробка передач";
            Helper.sqlstr[4] = "Transmission_rating";
            Helper.sqlstr[5] = "Рейтинг коробки передач";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button11_Click - открытие формы "Тип двигателя".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Тип двигателя".
         */
        private void button11_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Engine_type";
            Helper.sqlstr[1] = "Engine_number";
            Helper.sqlstr[2] = "Name_engine";
            Helper.sqlstr[3] = "Тип двигателя";
            Helper.sqlstr[4] = "Engine_rating";
            Helper.sqlstr[5] = "Рейтинг двигателя";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button12_Click - открытие формы "Климат-контроль".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Allf - форма "Климат-контроль".
         */
        private void button12_Click(object sender, EventArgs e)
        {
            Helper.sqlstr[0] = "Climate_control";
            Helper.sqlstr[1] = "Climate_number";
            Helper.sqlstr[2] = "Name_climate";
            Helper.sqlstr[3] = "Климат-контроль";
            Helper.sqlstr[4] = "Climate_rating";
            Helper.sqlstr[5] = "Рейтинг климат-контроля";

            AllMiniTablesForm Allf = new AllMiniTablesForm();
            Allf.ShowDialog();
        }

        /*  button13_Click - отмена действия.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button13.Enabled = false;
            button1.Tag = 0;
            button3.Tag = 0;

            Adapter = new SqlDataAdapter(sqlMain, Connection);
            ds = new DataSet();
            Adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            VIN.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            VIN.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            VIN.DataBindings.Add(new Binding("Text", bindingSource1, "VIN-код", true, DataSourceUpdateMode.Never));
        }
    }
}

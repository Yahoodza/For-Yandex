/*  Форма "Договор аренды".
 *  Название: Rent_contractForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма выводит информацию о договорах аренды.
 *  Переменные, используемые в форме:
 *      dtp - элемент управления DateTimePicker;
 *      dtp1 - элемент управления DateTimePicker;
 *      DtpRectangle - расположение и размер элемента управления DateTimePicker;
 *      ds - информация из БД;
 *      Adapter - подключение к БД;
 *      Connection - подключение к SQL-серверу;
 *      P[] - динамический массив занятых на парковке мест;
 *      k - номер строки datagridview;
 *      Men[] - массив для вывода ID арендатора;
 *      Car[] - массив для вывода ID автомобиля;
 *      Cost - стоимость аренды автомобиля;
 *      sqlMain - SQL-запрос;
 *      sqlN - SQL-запрос;
 *      sqlC - SQL-запрос;
 *      sqlB - SQL-запрос;
 *      sqlEn - SQL-запрос.
 *  Функции, используемые в форме:
 *      DataGridCellBlock - блокировка редактирования ячеек dataGridView;
 *      ContLowerLetter - определение вхождения прописной буквы в строку;
 *      ContUpperLetter - определение вхождения заглавной буквы в строку;
 *      ContDigit - определение вхождения цифры в строку;
 *      Rent_contractForm - конструктор формы;
 *      DataGridComboBox - создание элемента управления ComboBox в ячейке dataGridView;
 *      button1_Click - добавление новой строки в таблицу;
 *      dtp_ValueChanged - изменение значения в элементе управления DateTimePicker;
 *      dtp1_ValueChanged - изменение значения в элементе управления DateTimePicker;
 *      dataGridView1_EditingControlShowing - отображение элемента управления в ячейке dataGridView;
 *      Combo2_SelectedIndexChanged - изменение индекса значения в элементе управления ComboBox;
 *      dataGridView1_CellClick - щелчок мышью по ячейке dataGridView;
 *      checkBox1_CheckedChanged - изменение состояния элемента управления CheckBox;
 *      button2_Click - сохранение данных в БД;
 *      button3_Click - удаление строки в таблице;
 *      button4_Click - поиск автомобиля по параметрам;
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
    public partial class Rent_contractForm : Form
    {
        DateTimePicker dtp = new DateTimePicker();
        DateTimePicker dtp1 = new DateTimePicker();
        Rectangle DtpRectangle;
        DataSet ds;
        SqlDataAdapter Adapter;
        SqlConnection Connection = new SqlConnection(Helper.ConnectionString);
        public List<int> P = new List<int> { };
        public int k;
        string[] Men, Car;
        int Cost;
        string sqlMain = "SELECT RTRIM(Rent_contract.ID_rent_contract) AS 'Идентификатор',  (RTRIM(Renter.Surname) + ' ' + RTRIM(Renter.Name) + ' ' + RTRIM(Renter.Fullname)) AS 'ФИО', " +
                         "RTRIM(Car_brand.Name_brand) AS 'Марка автомобиля', RTRIM(Body_type.Name_body) AS 'Тип кузова', RTRIM(Engine_type.Name_engine) AS 'Тип двигателя', " +
                         "RTRIM(Rent_contract.Cost) AS 'Цена аренды', RTRIM(Rent_contract.Start_date_rent) AS 'Дата начала аренды', " +
                         "RTRIM(Rent_contract.End_date_rent) AS 'Дата окончания аренды', RTRIM(Rent_contract.Number_for_parking) AS 'Номер парковки', " +
                         "RTRIM(Rent_contract.Password_for_parking) AS 'Пароль' From Rent_contract JOIN Renter ON Renter.ID_Renter = Rent_contract.Renter " +
                         "JOIN Automobile ON Automobile.VIN = Rent_contract.Car JOIN Car_brand ON Car_brand.Brand_number = Automobile.Car_brand " +
                         "JOIN Body_type ON Body_type.Body_number = Automobile.Body_type JOIN Engine_type ON Engine_type.Engine_number = Automobile.Engine_type;";
        string sqlN = "SELECT (RTRIM(ID_Renter) + ' - ' + RTRIM(Surname) + ' ' + RTRIM(Name) + ' ' + RTRIM(Fullname)) AS 'Арендатор' FROM Renter;";
        string sqlC = "SELECT (RTRIM(Automobile.VIN) + ' - ' + RTRIM(Car_brand.Name_brand) + ' - ' + RTRIM(Body_type.Name_body) + ' - ' + RTRIM(Engine_type.Name_engine)) AS 'Автомобиль' " +
                      "FROM Automobile JOIN Car_brand ON Automobile.Car_brand = Car_brand.Brand_number " +
                      "JOIN Body_type ON Automobile.Body_type = Body_type.Body_number " +
                      "JOIN Engine_type ON Automobile.Engine_type = Engine_type.Engine_number";
        string sqlB = "SELECT RTRIM(Name_body) AS 'Тип кузова' FROM Body_type;";
        string sqlEn = "SELECT RTRIM(Name_engine) AS 'Тип двигателя' FROM Engine_type;";

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

        /*  ContLowerLetter - определение вхождения прописной буквы в строку.
         *  Формальный параметр:
         *      str - строка, которую требуется проверить.
         *  Локальная переменная:
         *      c - символ строки.
         */
        static bool ContLowerLetter(string str)
        {
            foreach (char c in str)                                                         //Цикл для проверки символа
            {
                if ((Char.IsLetter(c)) && (Char.IsLower(c)))                                //Условие проверки символа
                    return true;
            }

            return false;
        }

        /*  ContUpperLetter - определение вхождения заглавной буквы в строку.
         *  Формальный параметр:
         *      str - строка, которую требуется проверить.
         *  Локальная переменная:
         *      c - символ строки.
         */
        static bool ContUpperLetter(string str)
        {
            foreach (char c in str)                                                         //Цикл для проверки символа
            {
                if ((Char.IsLetter(c)) && (Char.IsUpper(c)))                                //Условие проверки символа
                    return true;
            }

            return false;
        }

        /*  ContDigit - определение вхождения цифры в строку.
         *  Формальный параметр:
         *      str - строка, которую требуется проверить.
         *  Локальная переменная:
         *      c - символ строки.
         */
        static bool ContDigit(string str)
        {
            foreach (char c in str)                                                         //Цикл для проверки символа
            {
                if (Char.IsDigit(c))                                                        //Условие проверки символа
                    return true;
            }

            return false;
        }

        /*  Rent_contractForm - конструктор формы. 
         *  Локальные переменные:
         *      i - счётчик для цикла;
         *      Connection - подключение к SQL-серверу.
         */
        public Rent_contractForm()
        {
            InitializeComponent();

            button1.BringToFront();
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
            Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
            for (int i = 0; i <= dataGridView1.ColumnCount - 1; i++)                        //Цикл блокировки сортировки
            {                                                                               //столбцов dataGridView
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /*  DataGridComboBox - создание элемента управления ComboBox в ячейке dataGridView.
        *  Формальные параметры:
        *      sql - SQL-запрос; 
        *      str2 - поле таблицы БД;
        *      i - индекс строки dataGridView;
        *      j - индекс ячейки dataGridView.
        *  Локальные переменные:
        *      Combo - элемент управления ComboBox;
        *      tb1 - SQL таблица.
        */
        public void DataGridComboBox(string sql, string str2, int i, int j)
        {
            Connection.Open();
            DataGridViewComboBoxCell Combo = new DataGridViewComboBoxCell();
            Adapter = new SqlDataAdapter(sql, Connection);
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
         *  Локальные переменные:
         *      i - счётчик для цикла;
         *      row - новая строка dataGridView;
         *      Combo5 - элемент управления ComboBox.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            P.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)                                //Цикл для записи занятых
            {                                                                               //мест парковки в массив
                P.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value));
            }

            k = dataGridView1.RowCount;
            dataGridView1.ReadOnly = false;
            DataGridCellBlock();
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;

            DataGridComboBox(sqlN, "Арендатор", k, 1);;
            DataGridComboBox(sqlC + " WHERE Automobile.Available = 1", "Автомобиль", k, 2);
            DataGridComboBox(sqlB, "Тип кузова", k, 3);
            dataGridView1.Rows[k].Cells[3].ReadOnly = true;
            DataGridComboBox(sqlEn, "Тип двигателя", k, 4);
            dataGridView1.Rows[k].Cells[4].ReadOnly = true;

            dataGridView1.Controls.Add(dtp);
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.MinDate = DateTime.Now;
            dtp.TextChanged += new EventHandler(dtp_ValueChanged);

            dataGridView1.Controls.Add(dtp1);
            dtp1.Format = DateTimePickerFormat.Custom;
            dtp1.MinDate = dtp.MinDate.AddDays(1);
            dtp1.TextChanged += new EventHandler(dtp1_ValueChanged);

            DtpRectangle = dataGridView1.GetCellDisplayRectangle(6, k, true);
            dtp.Size = new Size(DtpRectangle.Width, DtpRectangle.Height);
            dtp.Location = new Point(DtpRectangle.X, DtpRectangle.Y);
            dtp.Enabled = false;

            DtpRectangle = dataGridView1.GetCellDisplayRectangle(7, k, true);
            dtp1.Size = new Size(DtpRectangle.Width, DtpRectangle.Height);
            dtp1.Location = new Point(DtpRectangle.X, DtpRectangle.Y);
            dtp1.Enabled = false;

            DataGridViewComboBoxCell Combo5 = new DataGridViewComboBoxCell();
            Combo5.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            Combo5.FlatStyle = FlatStyle.Standard;
            Combo5.Items.Clear();
            Combo5.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18" });
            dataGridView1.Rows[k].Cells[8] = Combo5;

            for (int i = 0; i < P.Count; i++)                                               //Цикл для удаления занятых
            {                                                                               //мест парковки из элемента
                Combo5.Items.Remove(P[i].ToString());                                       //управления ComboBox
            }

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = true;
            checkBox1.Enabled = true;

            Rent_contract.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }

        /*  dtp_ValueChanged - изменение значения в элементе управления DateTimePicker.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            dtp1.MinDate = dtp.Value.AddDays(1);
            dtp1.Value = dtp1.MinDate;
        }

        /*  dtp1_ValueChanged - изменение значения в элементе управления DateTimePicker.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void dtp1_ValueChanged(object sender, EventArgs e)
        {
            Cost = Helper.DataGridComboBoxPK("SELECT Cost_of_rent FROM Automobile WHERE VIN = " + 
                                             Car[0]) * (dtp1.Value.Day - dtp.Value.Day);
            dataGridView1.Rows[k].Cells[5].Value = Cost;
        }

        /*  dataGridView1_EditingControlShowing - отображение элемента управления в ячейке dataGridView.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 2)                                 //Условие выбора ячейки dataGridView
            {
                (e.Control as ComboBox).SelectedIndexChanged -= new EventHandler(Combo2_SelectedIndexChanged);
                (e.Control as ComboBox).SelectedIndexChanged += new EventHandler(Combo2_SelectedIndexChanged);
            }
        }

        /*  Combo2_SelectedIndexChanged - изменение индекса значения в элементе управления ComboBox.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void Combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 2 &&                               //Условие проверки выбора
                (sender as ComboBox).SelectedIndex > -1 &&                                  //нужного элемента из ComboBox
                ((sender as ComboBox).Items[(sender as ComboBox).SelectedIndex] as DataRowView).Row["Автомобиль"].Equals((sender as ComboBox).Text))
            {
                Car = (sender as ComboBox).Text.Split(' ');
                Cost = Helper.DataGridComboBoxPK("SELECT Cost_of_rent FROM Automobile WHERE VIN = " + Car[0]) * (dtp1.Value.Day - dtp.Value.Day);
                dtp.Enabled = true;
                dtp1.Enabled = true;
            }

            (sender as ComboBox).SelectedIndexChanged -= new EventHandler(Combo2_SelectedIndexChanged);
        }

        /*  dataGridView1_CellClick - щелчок по ячейке dataGridView.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 5)                                 //Условие выбора ячейки dataGridView
            {
                dataGridView1.Rows[k].Cells[5].Value = Cost;
            }
        }

        /*  checkBox1_CheckedChanged - изменение состояния элемента управления CheckBox.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)                                                  //Условие проверки состояния элемента
            {                                                                               //управления CheckBox
                button4.Enabled = true;
                dataGridView1.Rows[k].Cells[3].ReadOnly = false;
                dataGridView1.Rows[k].Cells[4].ReadOnly = false;
            }
            else
            {
                dataGridView1.Rows[k].Cells[2].Value = null;
                DataGridComboBox(sqlC + " WHERE Automobile.Available = 1", "Автомобиль", k, 2);
                button4.Enabled = false;
                dataGridView1.Rows[k].Cells[3].ReadOnly = true;
                dataGridView1.Rows[k].Cells[4].ReadOnly = true;
                dataGridView1.Rows[k].Cells[3].Value = null;
                dataGridView1.Rows[k].Cells[4].Value = null;
            }

            Rent_contract.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }

        /*  button2_Click - сохранение данных в БД.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Men = dataGridView1.Rows[k].Cells[1].Value.ToString().Split(' ');
                if (dataGridView1.Rows[k].Cells[8].Value.ToString() != "" && (ContDigit(dataGridView1.Rows[k].Cells[9].Value.ToString()) == true) && 
                    (ContUpperLetter(dataGridView1.Rows[k].Cells[9].Value.ToString()) == true) && (ContLowerLetter(dataGridView1.Rows[k].Cells[9].Value.ToString()) == true) && 
                    dataGridView1.Rows[k].Cells[9].Value.ToString().Length > 6 && (dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("!") || 
                    dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("@") || dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("#") || 
                    dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("$") || dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("%") || 
                    dataGridView1.Rows[k].Cells[9].Value.ToString().Contains("^")))         //Условие проверки правильности ввода данных
                {
                    Helper.Query("INSERT INTO Rent_contract VALUES (" + Men[0] + ", " + Car[0] + ", '" + Cost + "', '" + dtp.Value + "', '" + dtp1.Value + "', '" + 
                                 dataGridView1.Rows[k].Cells[8].Value + "', '" + dataGridView1.Rows[k].Cells[9].Value + "')");
                    Helper.Query("UPDATE Automobile SET Available = 0 WHERE VIN = " + Car[0]);
                }
                else
                    throw new Exception();

                dataGridView1.Controls.Clear();
                dataGridView1.Controls.Clear();
                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                Rent_contract.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));

                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
                checkBox1.Enabled = false;
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникли неполадки, убедитесь, что вы ввели данные верно!\n" +
                                "Возможные ошибки:\n" +
                                "1 - Незаполненные поля;\n" +
                                "2 - Пароль введён неверно.\n" +
                                "Пароль должен содержать минимум 7 символов, минимум 1 прописную букву, минимум 1 цифру, " +
                                "минимум один символ из следующего списка (! @ # $ % ^)", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button3_Click - удаление строки в таблице.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.ReadOnly = false;
                Helper.Query("UPDATE Automobile SET Available = 1 WHERE (SELECT ID_rent_contract FROM Rent_contract WHERE ID_rent_contract = " + 
                             Rent_contract.Text + ") = " + Rent_contract.Text + " AND  VIN = (SELECT Car FROM Rent_contract WHERE ID_rent_contract = " + 
                             Rent_contract.Text + ")");
                Helper.Query("DELETE Rent_contract WHERE ID_rent_contract = " + Rent_contract.Text);

                Adapter = new SqlDataAdapter(sqlMain, Connection);
                ds = new DataSet();
                Adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                Rent_contract.DataBindings.Clear();
                bindingSource1.DataSource = ds.Tables[0];
                dataGridView1.DataSource = bindingSource1;
                Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
                Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
                DataGridCellBlock();
                dataGridView1.Columns[0].ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Для начала заполните БД для возможности удаления записей!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button4_Click - поиск автомобиля по параметрам.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows[k].Cells[3].Value.ToString() != "" && dataGridView1.Rows[k].Cells[4].Value.ToString() == "")
            {                                                                               //Условие выбора параметров
                dataGridView1.Rows[k].Cells[2].Value = null;                                //поиска автомобиля
                DataGridComboBox(sqlC + " WHERE Body_type.Name_body = '" + dataGridView1.Rows[k].Cells[3].Value + 
                                 "' AND Automobile.Available = 1", "Автомобиль", k, 2);
            }
            else
            {
                if (dataGridView1.Rows[k].Cells[3].Value.ToString() == "" && dataGridView1.Rows[k].Cells[4].Value.ToString() != "")
                {
                    dataGridView1.Rows[k].Cells[2].Value = null;
                    DataGridComboBox(sqlC + " WHERE Engine_type.Name_engine = '" + dataGridView1.Rows[k].Cells[4].Value + 
                                     "' AND Automobile.Available = 1", "Автомобиль", k, 2);
                }
                else
                    if (dataGridView1.Rows[k].Cells[3].Value.ToString() != "" && dataGridView1.Rows[k].Cells[4].Value.ToString() != "")
                    {
                        dataGridView1.Rows[k].Cells[2].Value = null;
                        DataGridComboBox(sqlC + " WHERE Body_type.Name_body = '" + dataGridView1.Rows[k].Cells[3].Value + "' AND Engine_type.Name_engine = '" + 
                                         dataGridView1.Rows[k].Cells[4].Value + "' AND Automobile.Available = 1", "Автомобиль", k, 2);
                    }
            }

            Rent_contract.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
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
            button4.Enabled = false;
            button5.Enabled = false;
            checkBox1.Enabled = false;

            dataGridView1.Controls.Clear();
            dataGridView1.Controls.Clear();
            Adapter = new SqlDataAdapter(sqlMain, Connection);
            ds = new DataSet();
            Adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Rent_contract.DataBindings.Clear();
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            Rent_contract.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
            Rent_contract.DataBindings.Add(new Binding("Text", bindingSource1, "Идентификатор", true, DataSourceUpdateMode.Never));
        }
    }
}

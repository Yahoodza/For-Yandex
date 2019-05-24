/*  Форма "Личный кабинет".
 *  Название: AccountForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма выводит информацию о конкретном договоре аренды.
 *  Переменные, используемые в форме:
 *      ds - информация из БД;
 *      Adapter - подключение к БД;
 *      sqlMain - SQL-запрос.
 *  Функции, используемые в форме:
 *      AccountForm - конструктор формы;
 *      button1_Click - вывод информации о выбранном договоре аренды;
 *      AccountForm_KeyUp - срабатывание кнопки "Войти" при нажатии клавиши Enter.
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
    public partial class AccountForm : Form
    {
        DataSet ds;
        SqlDataAdapter Adapter;
        string sqlMain = "SELECT RTRIM(Rent_contract.ID_rent_contract) AS 'Идентификатор',  (RTRIM(Renter.Surname) + ' ' + RTRIM(Renter.Name) + ' ' + RTRIM(Renter.Fullname)) AS 'ФИО', " +
                         "RTRIM(Car_brand.Name_brand) AS 'Марка автомобиля', RTRIM(Body_type.Name_body) AS 'Тип кузова', RTRIM(Engine_type.Name_engine) AS 'Тип двигателя', " +
                         "RTRIM(Rent_contract.Cost) AS 'Цена аренды', RTRIM(Rent_contract.Start_date_rent) AS 'Дата начала аренды', " +
                         "RTRIM(Rent_contract.End_date_rent) AS 'Дата окончания аренды', RTRIM(Rent_contract.Number_for_parking) AS 'Номер парковки', " +
                         "RTRIM(Rent_contract.Password_for_parking) AS 'Пароль' From Rent_contract JOIN Renter ON Renter.ID_Renter = Rent_contract.Renter " +
                         "JOIN Automobile ON Automobile.VIN = Rent_contract.Car JOIN Car_brand ON Car_brand.Brand_number = Automobile.Car_brand " +
                         "JOIN Body_type ON Body_type.Body_number = Automobile.Body_type JOIN Engine_type ON Engine_type.Engine_number = Automobile.Engine_type";

        /*  AccountForm - конструктор формы. */
        public AccountForm()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        /*  button1_Click - вывод информации о выбранном договоре аренды.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальные переменные:
         *      Connection - подключение к SQL-серверу;
         *      i - счетчик для цикла;
         *      j - счетчик для цикла;
         *      k - переменная для выделения занятого арендатором места на парковке.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int j = 1; j <= 18; j++)
                {
                    Controls["Park" + j].BackColor = Color.FromArgb(255, 193, 37);
                    Controls["Park" + j].ForeColor = DefaultForeColor;
                }

                using (SqlConnection Connection = new SqlConnection(Helper.ConnectionString))   //Настройка SQL-соединения
                {
                    Connection.Open();
                    Adapter = new SqlDataAdapter(sqlMain + " WHERE Rent_contract.Password_for_parking = '" + textBox1.Text + "'", Connection);
                    ds = new DataSet();
                    Adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                for (int i = 0; i <= dataGridView1.ColumnCount - 1; i++)                        //Цикл блокировки сортировки
                {                                                                               //столбцов dataGridView
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                int k = Convert.ToInt32(dataGridView1.Rows[0].Cells[8].Value);

                Controls["Park" + k].BackColor = Color.FromArgb(160, 0, 0);                     //Выделение занятого места
                Controls["Park" + k].ForeColor = Color.White;                                   //на парковке
            }
            catch (Exception)
            {
                MessageBox.Show("Пароль неверный либо не введён", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  AccountForm_KeyUp - срабатывание кнопки "Войти" при нажатии клавиши Enter.
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         */
        private void AccountForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)                                                        //Условие выбора клавиши
                button1.PerformClick();
        }
    }
}

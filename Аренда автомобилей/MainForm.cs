/*  Форма "Аренда".
 *  Название: MainForm.
 *  Язык: C#
 *  Краткое описание:
 *      Данная форма является основной и предоставляет возможность работы с основными таблицами БД.
 *  Функции, используемые в форме:
 *      MainForm - конструктор формы;
 *      button1_Click - открытие формы "Автомобили";
 *      button2_Click - открытие формы "Арендаторы";
 *      button3_Click - открытие формы "Договор аренды";
 *      button4_Click - открытие формы "Личный кабинет".
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
    public partial class MainForm : Form
    {
        /*  MainForm - конструктор формы. */
        public MainForm()
        {
            InitializeComponent();
        }

        /*  button1_Click - открытие формы "Автомобили".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Autof - форма "Автомобили".
         */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AutomobileForm Autof = new AutomobileForm();
                Autof.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Проверьте подключение к SQL-серверу!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button2_Click - открытие формы "Арендаторы".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Arf - форма "Арендаторы".
         */
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RenterForm Arf = new RenterForm();
                Arf.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Проверьте подключение к SQL-серверу!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button3_Click - открытие формы "Договор аренды".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Rc - форма "Договор аренды".
         */
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Rent_contractForm Rc = new Rent_contractForm();
                Rc.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Проверьте подключение к SQL-серверу!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*  button4_Click - открытие формы "Личный кабинет".
         *  Формальные параметры:
         *      sender - элемент управления, вызывающий эту функцию; 
         *      e - аргументы события.
         *  Локальная переменная:
         *      Af - форма "Личный кабинет".
         */
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                AccountForm Af = new AccountForm();
                Af.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к SQL-серверу!", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

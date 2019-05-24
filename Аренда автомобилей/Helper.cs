/*  Вспомогательный класс для обработки действий с базой данных.
 *  Название: Helper.
 *  Язык: C#
 *  Краткое описание:   
 *      Данный класс предназначен для работы с базой данных.
 *  Глобальные переменные в классе:
 *      sqlstr[] - массив полей базы данных;
 *      ConnectionString - строка подключения к базе данных.
 *  Функции, используемые в классе:
 *      DataGridComboBoxPK - обработка SQL-команды;
 *      Query - выполнение SQL-команды.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Rent_car
{
    static class Helper
    {
        public static string ConnectionString = @"Data Source = .\SQLEXPRESS;INITIAL Catalog = Rent_a_car;Integrated Security = True";
        public static string[] sqlstr = new string[6];

        /*  DataGridComboBoxPK - обработка SQL-команды.
         *  Формальный параметр:
         *      sqlPK - SQL-запрос.
         *  Локальные переменные:
         *      Connection - подключение к SQL-серверу;
         *      Read - чтение SQL-запроса;
         *      Index - результат SQL-запроса;
         *      ID - результат SQL-запроса.
         */
        public static int DataGridComboBoxPK(string sqlPK)
        {
            int ID = 0;
            SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            SqlCommand cmd = new SqlCommand(sqlPK, Connection);
            SqlDataReader Read = cmd.ExecuteReader();
            while (Read.Read())                                                     //Цикл чтения SQL-запроса
            {
                Decimal Index = Convert.ToDecimal(Read.GetValue(0));
                ID = Convert.ToInt32(Index);
            }
            Read.Close();
            Connection.Close();
            return ID;
        }

        /*  Query - выполнение SQL-команды.
         *  Формальный параметр:
         *      sqlQ - SQL-команда.
         *  Локальные переменные:
         *      Connection - подключение к SQL-серверу;
         *      cmd - SQL-команда.
         */
        public static void Query(string sqlQ)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))  //Настройка SQL-соединения
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQ;
                cmd.ExecuteNonQuery();
                Connection.Close();
            }
        }
    }
}

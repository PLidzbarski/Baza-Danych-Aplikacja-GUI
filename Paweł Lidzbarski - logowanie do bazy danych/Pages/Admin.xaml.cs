using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych.Pages
{
	/// <summary>
	/// Logika interakcji dla klasy Admin.xaml
	/// </summary>
	public partial class Admin : Page
	{
		Database databaseObject;
		public Admin()
		{
			InitializeComponent();
			databaseObject = new Database();
		}

		

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Logowanie());
		}
		void ShowDataBaseInConsole() //wyswietlanie bazy danych w konsoli
		{
			databaseObject.OpenConnection();
			string stm = "SELECT * FROM User";
			var cmd = new SQLiteCommand(stm, databaseObject.myConnection);
			SQLiteDataReader rdr = cmd.ExecuteReader();
			Console.WriteLine($"{rdr.GetName(0),-8} {rdr.GetName(1),-10} {rdr.GetName(2),-15} {rdr.GetName(3),-15} {rdr.GetName(4),-10} {rdr.GetName(5),-30} {rdr.GetName(6),-15} {rdr.GetName(7),-15} {rdr.GetName(8),-16} {rdr.GetName(9),-20} {rdr.GetName(10),-5}");

			while (rdr.Read())
			{
				Console.WriteLine($@"{rdr.GetInt32(0),-8} {rdr.GetString(1),-10} {rdr.GetString(2),-15} {rdr.GetString(3),-15} {rdr.GetString(4),-10} {rdr.GetString(5),-30} {rdr.GetString(6),-15} {rdr.GetString(7),-15} {rdr.GetString(8),-16} {rdr.GetString(9),-20} {rdr.GetString(10),-5}");
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ShowDataBaseInConsole();
		}
	}
}

using System;
using System.Data.SQLite;
using System.Windows.Controls;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych.Pages
{
	/// <summary>
	/// Logika interakcji dla klasy User.xaml
	/// </summary>
	public partial class User : Page
	{
		Database databaseObject;
		string[] comboboxWydarzenia=new string[2];
		string[] agenda = new string[2];
		public User()
		{
			InitializeComponent();
			databaseObject = new Database();
			ComboboxWydarzenia();
			Agenda();
		}

		void ComboboxWydarzenia()
		{
			int i=0;
			SQLiteCommand myCommand1 = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand1.CommandText = "SELECT * FROM Wydarzenia";
			myCommand1.ExecuteNonQuery();
			SQLiteDataReader rdr = myCommand1.ExecuteReader();
			while (rdr.Read())
			{
				comboboxWydarzenia[i] = rdr.GetString(1);
				i++;
			}
			databaseObject.CloseConnection();

			nazwaWydarzenia.Items.Add(comboboxWydarzenia[0]);
			nazwaWydarzenia.Items.Add(comboboxWydarzenia[1]);

		}
		void Agenda()
		{
			int i = 0;
			SQLiteCommand myCommand1 = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand1.CommandText = "SELECT * FROM Wydarzenia";
			myCommand1.ExecuteNonQuery();
			SQLiteDataReader rdr = myCommand1.ExecuteReader();
			while (rdr.Read())
			{
				agenda[i] = rdr.GetString(2);
				i++;
			}
			databaseObject.CloseConnection();
			

		}

		private void lostfocus(object sender, System.Windows.RoutedEventArgs e)
		{
			int indeks = nazwaWydarzenia.SelectedIndex;
			indeks = indeks * indeks;
			if ((nazwaWydarzenia.SelectedItem as string) == comboboxWydarzenia[indeks])
			{
				textboxAgenda.Text = agenda[indeks];
			}
		}

		private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Logowanie());
		}
	}
}

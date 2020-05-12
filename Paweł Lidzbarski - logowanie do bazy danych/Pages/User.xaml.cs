using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych.Pages
{
	/// <summary>
	/// Logika interakcji dla klasy User.xaml
	/// </summary>
	public partial class User : Page
	{
		Database databaseObject;
		Dane dane;
		string[] comboboxWydarzenia=new string[2];
		string[] agenda = new string[2];
		string[] termin = new string[2];
		public User(Dane dane)
		{
			InitializeComponent();
			this.dane = dane;
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
		void DataWydarzenia()
		{
			int i = 0;
			SQLiteCommand myCommand1 = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand1.CommandText = "SELECT * FROM Wydarzenia";
			myCommand1.ExecuteNonQuery();
			SQLiteDataReader rdr = myCommand1.ExecuteReader();
			while (rdr.Read())
			{
				termin[i] = rdr.GetString(3);
				i++;
			}
			databaseObject.CloseConnection();
		}
		void AddToDatabase() //dodawanie obecnie wypelnionych pol w GUI
		{
			string query = "INSERT INTO ZapisNaWydarzenia (`NazwaWydarzenia`, `Login`,`TerminWydarzenia`, `TypUczestnictwa`, `Wyżywienie`) VALUES (@NazwaWydarzenia, @Login, @TerminWydarzenia, @TypUczestnictwa, @Wyżywienie)";
			SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.Parameters.AddWithValue("@NazwaWydarzenia", nazwaWydarzenia.SelectedItem);
			myCommand.Parameters.AddWithValue("@Login", dane._login);
			myCommand.Parameters.AddWithValue("@TerminWydarzenia", terminWydarzenia.SelectedDate);
			myCommand.Parameters.AddWithValue("@TypUczestnictwa", TypUczestnictwa.Text); 
			myCommand.Parameters.AddWithValue("@Wyżywienie", Wyzywienie.SelectedItem);
			var result = myCommand.ExecuteNonQuery();
			databaseObject.CloseConnection();
		}
		
		private void lostfocus(object sender, System.Windows.RoutedEventArgs e)
		{
			int indeks1 = nazwaWydarzenia.SelectedIndex;
			indeks1 = indeks1 * indeks1;
			if ((nazwaWydarzenia.SelectedItem as string) == comboboxWydarzenia[indeks1])
			{
				textboxAgenda.Text = agenda[indeks1];
			}
		}

		private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Logowanie());
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			AddToDatabase();
			MessageBox.Show("Zapisano pomyślnie");
		}
	}
}

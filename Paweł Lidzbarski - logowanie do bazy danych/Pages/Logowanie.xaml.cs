using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Timers;


namespace Paweł_Lidzbarski___logowanie_do_bazy_danych.Pages
{
	/// <summary>
	/// Logika interakcji dla klasy Logowanie.xaml
	/// </summary>
	public partial class Logowanie : Page
	{
		Database databaseObject;
		Dane dane = new Dane();
		bool succesLogin = false;
		bool succesHaslo = false;
		int probaLogowania = 3;
		bool stworzAdmin = false;
		bool stworzUser = false;
		public Logowanie()
		{
			InitializeComponent();
			databaseObject = new Database();
			ResetIdInDataBase();
			ClearDataBase();
			createAdmin();
			createUser();
		}
		void ResetIdInDataBase() //resetowanie licznika id w obu istniejacych tablicach bazy danych
		{
			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.CommandText = "UPDATE sqlite_sequence SET seq = 0 WHERE name = 'User'";
			myCommand.ExecuteNonQuery();
			myCommand.CommandText = "UPDATE sqlite_sequence SET seq = 0 WHERE name = 'Wydarzenia'";
			myCommand.ExecuteNonQuery();
			myCommand.CommandText = "UPDATE sqlite_sequence SET seq = 0 WHERE name = 'ZapisNaWydarzenia'";
			myCommand.ExecuteNonQuery();
			databaseObject.CloseConnection();
		}
		void ClearDataBase() //czyszczenie rekordow w dwoch tablicach
		{
			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.CommandText = "DELETE FROM User";
			myCommand.ExecuteNonQuery();
			myCommand.CommandText = "DELETE FROM Wydarzenia";
			myCommand.ExecuteNonQuery();
			myCommand.CommandText = "DELETE FROM ZapisNaWydarzenia";
			myCommand.ExecuteNonQuery();
			databaseObject.CloseConnection();
		}
		void createAdmin()
		{

			SQLiteCommand myCommand1 = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand1.CommandText = "SELECT Login, Hasło FROM User WHERE Login='admin' AND Hasło='admin'";
			myCommand1.ExecuteNonQuery();
			var count1 = myCommand1.ExecuteScalar();
			if (count1 != null)
			{

				stworzAdmin = true;

			}
			databaseObject.CloseConnection();
		}
		void createUser()
		{
			if (stworzAdmin == false)
			{
				string query = "INSERT INTO User (`Imię`, `Nazwisko`, `Stanowisko`, `Płeć`, `Email`, `Login`, `Hasło`, `AktywnośćKonta`, `DataRejestracji`,`Uprawnienia`) VALUES (@Imię, @Nazwisko, @Stanowisko, @Płeć, @Email, @Login, @Hasło, @AktywnośćKonta, @DataRejestracji, @Uprawnienia)";
				SQLiteCommand myCommand2 = new SQLiteCommand(query, databaseObject.myConnection);
				databaseObject.OpenConnection();
				myCommand2.Parameters.AddWithValue("@Imię", "Admin");
				myCommand2.Parameters.AddWithValue("@Nazwisko", "Admin");
				myCommand2.Parameters.AddWithValue("@Stanowisko", "Admin");
				myCommand2.Parameters.AddWithValue("@Płeć", "Admin");
				myCommand2.Parameters.AddWithValue("@Email", "Admin@admin.admin");
				myCommand2.Parameters.AddWithValue("@Login", "admin");
				myCommand2.Parameters.AddWithValue("@Hasło", "admin");
				myCommand2.Parameters.AddWithValue("@AktywnośćKonta", "true");
				myCommand2.Parameters.AddWithValue("@DataRejestracji", "01.01.01");
				myCommand2.Parameters.AddWithValue("@Uprawnienia", "Admin");
				var result = myCommand2.ExecuteNonQuery();
				databaseObject.CloseConnection();
			}
			SQLiteCommand myCommand3 = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand3.CommandText = "SELECT Login, Hasło FROM User WHERE Login='user' AND Hasło='user'";
			myCommand3.ExecuteNonQuery();
			var count2 = myCommand3.ExecuteScalar();
			if (count2 != null)
			{

				stworzUser = true;

			}
			databaseObject.CloseConnection();
			if (stworzUser == false)
			{
				string query = "INSERT INTO User (`Imię`, `Nazwisko`, `Stanowisko`, `Płeć`, `Email`, `Login`, `Hasło`, `AktywnośćKonta`, `DataRejestracji`,`Uprawnienia`) VALUES (@Imię, @Nazwisko, @Stanowisko, @Płeć, @Email, @Login, @Hasło, @AktywnośćKonta, @DataRejestracji, @Uprawnienia)";
				SQLiteCommand myCommand4 = new SQLiteCommand(query, databaseObject.myConnection);
				databaseObject.OpenConnection();
				myCommand4.Parameters.AddWithValue("@Imię", "User");
				myCommand4.Parameters.AddWithValue("@Nazwisko", "User");
				myCommand4.Parameters.AddWithValue("@Stanowisko", "User");
				myCommand4.Parameters.AddWithValue("@Płeć", "Mężczyzna");
				myCommand4.Parameters.AddWithValue("@Email", "User@user.user");
				myCommand4.Parameters.AddWithValue("@Login", "user");
				myCommand4.Parameters.AddWithValue("@Hasło", "user");
				myCommand4.Parameters.AddWithValue("@AktywnośćKonta", "true");
				myCommand4.Parameters.AddWithValue("@DataRejestracji", "01.01.2020");
				myCommand4.Parameters.AddWithValue("@Uprawnienia", "User");
				var result = myCommand4.ExecuteNonQuery();
				databaseObject.CloseConnection();
			}

		}
		void checkingLogin()
		{
			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.CommandText = "SELECT Login FROM User WHERE Login='" + login.Text+"'";
			myCommand.ExecuteNonQuery();
			var count = myCommand.ExecuteScalar();
			if (count != null)
			{

				succesLogin = true;

			}
			else
			{
				MessageBox.Show("Podany Login jest nieprawidłowy!");
			}
			databaseObject.CloseConnection();
		}
		void checkingPassword()
		{
			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.CommandText = "SELECT Hasło FROM User WHERE Hasło='" + password.Password + "'";
			myCommand.ExecuteNonQuery();
			var count = myCommand.ExecuteScalar();
			if (count != null)
			{

				succesHaslo = true;

			}
			else
			{
				MessageBox.Show("Podane Hasło jest nieprawidłowe!");
			}
			databaseObject.CloseConnection();
		}
		bool checkingAdmin()
		{
			if (login.Text == "admin" && password.Password == "admin")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			licznikLogowania.Text = Convert.ToString(probaLogowania);
			if (checkingAdmin())
			{
				(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Admin());
				gridLogowawnie.Visibility = Visibility.Hidden;
				main_grid.Visibility = Visibility.Hidden;
			}
			else
			{

				if (probaLogowania <= 0)
				{
					MessageBox.Show("Wykorzystałeś 3 próby logowania. Zrestartuj aplikację aby zresetować licznik.");
				}
				else if (probaLogowania > 0)
				{
					checkingLogin();
					checkingPassword();
					if (succesLogin == true && succesHaslo == true)
					{
						dane._login = login.Text;
						dane._haslo = password.Password;
						(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.User(dane));
						gridLogowawnie.Visibility = Visibility.Hidden;
						main_grid.Visibility = Visibility.Hidden;


					}
				}

			}
			if (probaLogowania > 0) { probaLogowania--; }

		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			textboxPassword.Text = password.Password;
			password.Visibility = Visibility.Hidden;
			textboxPassword.Visibility = Visibility.Visible;
		}

		private void revealModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			textboxPassword.Visibility = Visibility.Hidden;
			password.Password = textboxPassword.Text;
			password.Visibility = Visibility.Visible;
		}

		private void rejestruj_Click(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.RejestracjaUser());
			gridLogowawnie.Visibility = Visibility.Hidden;
			main_grid.Visibility = Visibility.Hidden;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Stworzone już są dwa konta: User(user, user); Admin(admin,admin)");

		}
	}
}

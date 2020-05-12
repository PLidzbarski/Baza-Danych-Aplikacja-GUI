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
using System.Net.Mail;
using System.Net;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych.Pages
{
	/// <summary>
	/// Logika interakcji dla klasy RejestracjaUser.xaml
	/// </summary>
	public partial class RejestracjaUser : Page
	{
		Database databaseObject; //obiekt klasy database
		bool poprawny_email = false;
		bool poprawne_haslo = false;
		bool powtarzajaceDane = false;
		public RejestracjaUser()
		{
			InitializeComponent();
			InitializeComponent();
			databaseObject = new Database();
			
			
		}
		public static void emailSending(string mail)
		{
			string to = mail;
			string from = "automail.ppiu@gmail.com";
			MailMessage wiadomosc = new MailMessage(from, to);
			wiadomosc.Subject = "Potwierdzenie";
			wiadomosc.Body = "Mail weryfikacyjny rejestrację :)";

			SmtpClient client = new SmtpClient();
			client.Credentials = new NetworkCredential("automail.ppiu@gmail.com", "automail1");
			client.Host = "smtp.gmail.com";
			client.Port = 587;
			client.EnableSsl = true;
			client.Send(wiadomosc);
		}
		static string UppercaseFirst(string s) //metoda poprawiajaca pierwszą litere na dużą litere
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}
		void IsDigit(string imie, Label label) //sprawdzanie czy sa cyfry
		{
			imie = Imie.Text;
			for (int i = 0; i < imie.Length; i++)
			{
				char znak = Convert.ToChar(imie[i]);
				if (char.IsDigit(znak))
				{
					label.Foreground = Brushes.Red;
					label.Content = "Imie nie moze zawierac cyfr";
				}
				else label.Content = "";
			}
		}

		

		

		void CheckingEmail() //sprawdzanie czy obecny email widnieje w bazie
		{

			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.CommandText = "SELECT Email FROM User WHERE Email='" + Email.Text + "'";
			myCommand.ExecuteNonQuery();
			var count = myCommand.ExecuteScalar();
			if (count != null)
			{
				grid_label1.Visibility = Visibility.Visible;
				grid_label2.Visibility = Visibility.Hidden;
				grid_label3.Visibility = Visibility.Visible;
				grid_label3.Content = "Podany Email juz istnieje!";
				error_grid.Visibility = Visibility.Visible;
				powtarzajaceDane = true;
			}
			else
			{
				powtarzajaceDane = false;
			}
			databaseObject.CloseConnection();

		}

		void CheckingLogin() // -||- login
		{
			SQLiteCommand myCommand = new SQLiteCommand(databaseObject.myConnection);
			//databaseObject.OpenConnection();
			databaseObject.OpenConnection();
			myCommand.CommandText = "SELECT Login FROM User WHERE Login='" + Login.Text + "'";
			myCommand.ExecuteNonQuery();
			var count = myCommand.ExecuteScalar();
			if (count != null)
			{
				grid_label1.Visibility = Visibility.Visible;
				grid_label2.Visibility = Visibility.Hidden;
				grid_label3.Visibility = Visibility.Visible;
				grid_label3.Content = "Podany Login juz istnieje!";
				error_grid.Visibility = Visibility.Visible;
				powtarzajaceDane = true;
			}
			else
			{
				powtarzajaceDane = false;
			}
			databaseObject.CloseConnection();
		}

		void AddToDatabase() //dodawanie obecnie wypelnionych pol w GUI
		{
			string query = "INSERT INTO User (`Imię`, `Nazwisko`, `Stanowisko`, `Płeć`, `Email`, `Login`, `Hasło`, `AktywnośćKonta`, `DataRejestracji`) VALUES (@Imię, @Nazwisko, @Stanowisko, @Płeć, @Email, @Login, @Hasło, @AktywnośćKonta, @DataRejestracji)";
			SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
			databaseObject.OpenConnection();
			myCommand.Parameters.AddWithValue("@Imię", Imie.Text);
			myCommand.Parameters.AddWithValue("@Nazwisko", Nazwisko.Text);
			myCommand.Parameters.AddWithValue("@Stanowisko", Stanowisko.Text);
			myCommand.Parameters.AddWithValue("@Płeć", Plec.Text);
			myCommand.Parameters.AddWithValue("@Email", Email.Text);
			myCommand.Parameters.AddWithValue("@Login", Login.Text);
			myCommand.Parameters.AddWithValue("@Hasło", Haslo.Password);
			myCommand.Parameters.AddWithValue("@AktywnośćKonta", "true");
			myCommand.Parameters.AddWithValue("@DataRejestracji", Convert.ToString(DateTime.Now));
			var result = myCommand.ExecuteNonQuery();
			databaseObject.CloseConnection();
		}



		private void Imie_LostFocus(object sender, RoutedEventArgs e)
		{
			if (Imie.Text == "")
			{
				Imie.BorderBrush = Brushes.Red;
			}
			else
			{
				Imie.BorderBrush = Brushes.Green;
			}
			Imie.Text = UppercaseFirst(Imie.Text);
			IsDigit(Imie.Text, label_imie);
		}

		private void Naziwsko_LostFocus(object sender, RoutedEventArgs e)
		{
			if (Nazwisko.Text == "")
			{
				Nazwisko.BorderBrush = Brushes.Red;
			}
			else
			{
				Nazwisko.BorderBrush = Brushes.Green;
			}
			Nazwisko.Text = UppercaseFirst(Nazwisko.Text);
			IsDigit(Nazwisko.Text, label_nazwisko);
		}
		private void Email_LostFocus(object sender, RoutedEventArgs e)
		{

			if (Email.Text == "")
			{
				Email.BorderBrush = Brushes.Red;
			}
			Regex rx = new Regex(@"\w*@{1}\w+\.{1}\w+");
			bool ret = rx.IsMatch(Email.Text);
			if (ret)
			{
				Email.BorderBrush = Brushes.Green;
				label_email.Visibility = Visibility.Hidden;
				poprawny_email = true;
			}
			else
			{
				Email.BorderBrush = Brushes.Red;
				label_email.Foreground = Brushes.Red;
				label_email.Content = "Ups! Tak nie wygląda Email!";
				label_email.Visibility = Visibility.Visible;
				poprawny_email = false;
			}
		}

		private void Login_LostFocus(object sender, RoutedEventArgs e)
		{

			if (Login.Text == "")
			{
				Login.BorderBrush = Brushes.Red;
			}
			else
			{
				Login.BorderBrush = Brushes.Green;
			}
		}

		private void Haslo_LostFocus_1(object sender, RoutedEventArgs e)
		{
			if (Haslo.Password == "")
			{
				Haslo.BorderBrush = Brushes.Red;
			}
			else
			{
				Haslo.BorderBrush = Brushes.Green;
			}
		}

		private void Powtorzhaslo_LostFocus(object sender, RoutedEventArgs e)
		{

			if (Powtorzhaslo.Password != Haslo.Password || Powtorzhaslo.Password == "")
			{
				Powtorzhaslo.BorderBrush = Brushes.Red;
				label_powtorzhaslo.Foreground = Brushes.Red;
				label_powtorzhaslo.Content = "Hasła muszą być takie same!";
				poprawne_haslo = false;
			}
			else
			{
				Powtorzhaslo.BorderBrush = Brushes.Green;
				label_powtorzhaslo.Foreground = Brushes.Green;
				label_powtorzhaslo.Content = "";
				poprawne_haslo = true;
			}
		}

		private void Stanowisko_LostFocus(object sender, RoutedEventArgs e)
		{

			if (Stanowisko.Text == "")
			{
				Stanowisko.BorderBrush = Brushes.Red;
			}
			else
			{
				Stanowisko.BorderBrush = Brushes.Green;
			}
		}

		private void Rejestruj_Click(object sender, RoutedEventArgs e)
		{
			bool result = true;
			foreach (var main_children in main_grid.Children)
			{
				if (main_children.GetType() == typeof(TextBox))
				{
					if (((TextBox)main_children).Text == string.Empty)
					{
						grid_label1.Visibility = Visibility.Visible;
						grid_label3.Visibility = Visibility.Hidden;
						grid_label2.Visibility = Visibility.Visible;
						string nazwa = (main_children as TextBox).Name;
						error_grid.Visibility = Visibility.Visible;
						Rejestruj.IsEnabled = false;
						result = false;
					}
				}
			}
			CheckingEmail();
			CheckingLogin();
			if (result == true && poprawne_haslo == true && poprawny_email == true && powtarzajaceDane == false)
			{
				AddToDatabase();
				emailSending(Email.Text);
				grid_label2.Visibility = Visibility.Hidden;
				grid_label3.Visibility = Visibility.Visible;
				grid_label3.Content = "Zarejestrowano!";
				grid_label1.Visibility = Visibility.Hidden;
				error_grid.Visibility = Visibility.Visible;
			}
		}

		private void grid_error_click(object sender, RoutedEventArgs e)
		{
			error_grid.Visibility = Visibility.Hidden;
			Rejestruj.IsEnabled = true;
		}

		

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			(App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Logowanie());
		}
	}
}

using System.Data.SQLite;
using System.IO;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych
{
	class Database
	{
		public SQLiteConnection myConnection;

		public Database()
		{
			// Tworzenie polaczenia z baza danych, sprawdzanie czy owy plik juz istnieje, w przeciwnym przypadku, bylby tworzony przy kazdym polaczeniu
			myConnection = new SQLiteConnection("Data Source = database.sqlite3");
			if (!File.Exists("./database.sqlite3"))
			{
				SQLiteConnection.CreateFile("database.sqlite3");
			}
		}
		public void OpenConnection()
		{
			if(myConnection.State != System.Data.ConnectionState.Open)
			{
				myConnection.Open();
			}
		}
		public void CloseConnection()
		{
			if(myConnection.State != System.Data.ConnectionState.Closed)
			{
				myConnection.Close();
			}
		}
	}
}

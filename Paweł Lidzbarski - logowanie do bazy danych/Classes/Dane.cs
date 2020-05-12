using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paweł_Lidzbarski___logowanie_do_bazy_danych
{
	public class Dane
	{
		string login;
		string haslo;
		public string _login {get => login; set => login = value;}
		public string _haslo { get => haslo; set => haslo = value; }
	}
}

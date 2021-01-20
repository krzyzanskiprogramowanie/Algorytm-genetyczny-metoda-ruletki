using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Si_projekt
{
    class MenadzerPol
    {
        public static bool sprawdzPola(TextBox pole, Label etykieta, string wartosc, string nazwaPola, bool czyUlamkowe, bool zZerem)
        {
            etykieta.Text = "";
            bool decyzja = true;
            try
            {

                if (czyUlamkowe == false)
                {
                    int wartoscLiczbowa;
                    wartoscLiczbowa = Convert.ToInt32(wartosc);
                    if (zZerem == false)
                    {
                        if (wartoscLiczbowa <= 0)
                        {
                            etykieta.Text += $"{nazwaPola} musi być większa od zera\n";
                            decyzja = false;
                        }
                    }
                    
                }
                else if(czyUlamkowe == true)
                {
                    double wartoscLiczbowa;
                    wartoscLiczbowa = Convert.ToDouble(wartosc);
                    if (wartoscLiczbowa < 0 || wartoscLiczbowa > 1)
                    {
                        etykieta.Text += $"{nazwaPola} musi być między 0 a 1\n";
                        decyzja = false;
                    }
                }
            }
            catch
            {
                etykieta.Text += $"{nazwaPola} musi być liczbą \n";
                decyzja = false;
            }
            
            return decyzja;
        }
        public static bool sprawdzCzyParzyste(TextBox pole, Label etykieta, string wartosc, string nazwaPola)
        {
            etykieta.Text = "";
            bool decyzja = true;
            int watroscDoSprawdzenia = Convert.ToInt32(wartosc);
            if(watroscDoSprawdzenia % 2 != 0)
            {
                etykieta.Text += $"{nazwaPola} musi być parzyta. \n";
                decyzja = false;
            }
            return decyzja;
            
        }
    }
}

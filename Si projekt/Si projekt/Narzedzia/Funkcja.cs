using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Si_projekt
{
    class Funkcja
    {
        public static double obliczFunkcje(double x, double y, string[] pola, decimal[] potegi)
        {
            
                       double wyniki;
            wyniki = (((Math.Pow(x, (double)potegi[0])) * (Convert.ToInt32(pola[0])))
                    * ((Math.Pow(y, (double)potegi[1])) * (Convert.ToInt32(pola[1]))))
                +
                    (((Math.Pow(x, (double)potegi[2])) * (Convert.ToInt32(pola[2])))
                   *((Math.Pow(y, (double)potegi[3])) * (Convert.ToInt32(pola[3]))))
            +
            (((Math.Pow(x, (double)potegi[4])) * (Convert.ToInt32(pola[4])))
           * ((Math.Pow(y, (double)potegi[5])) * (Convert.ToInt32(pola[5]))))

            + 
                     ((Math.Pow(x, (double)potegi[6])) * (Convert.ToInt32(pola[6])))
                    +
                    ((Math.Pow(y, (double)potegi[7])) * (Convert.ToInt32(pola[7])))

                    + (Convert.ToInt32(pola[8]));
            ;

            return wyniki;
        }

        public static double przeliczDokladnosc(string znakiLicz)
        {
            try
            {
                int licznikZer = 0;
                string liczbaJakoText = "";
                double liczba = 1;
                for (int i = 0; i < znakiLicz.Length; i++)
                {
                    if (znakiLicz[i] == '0')
                        licznikZer++;
                    else if (znakiLicz[i] != ',' || znakiLicz[i] != '.')
                        liczbaJakoText += znakiLicz[i];
                }

                // Kolejny błąd gdy wpiszemy dokładność jako 0,0 lub 0

                liczba = Convert.ToDouble(liczbaJakoText);


                liczba = liczba * Math.Pow(10, licznikZer + 1);

                return liczba;
            }
            catch 
            {

                return 2;
                    }
        }

        public static int liczbaBitowa(int liczba1, int liczba2)
        {

            if (liczba1 < 0)
                return liczba2 + (-1 * liczba1);
            else
                return liczba2 - liczba1;
        }

        public static int obliczIloscBitow(double liczba)
        {
            if (liczba < 2)
            {
                return 1;
            }
            else if (liczba < 4 && liczba > 2)
            {
                return 2;
            }
            else if (liczba < 8 && liczba > 4)
            {
                return 3;
            }
            else if (liczba < 16 && liczba > 8)
            {
                return 4;
            }
            else if (liczba < 32 && liczba > 16)
            {
                return 5;
            }
            else if (liczba < 64 && liczba > 32)
            {
                return 6;
            }
            else if (liczba < 128 && liczba > 64)
            {
                return 7;
            }
            else if (liczba < 256 && liczba > 128)
            {
                return 8;
            }
            else if (liczba < 512 && liczba > 256)
            {
                return 9;
            }
            else if (liczba < 1024 && liczba > 512)
            {
                return 10;
            }
            else if (liczba < 2048 && liczba > 1024)
            {
                return 11;
            }
            else if (liczba < 4096 && liczba > 2048)
            {
                return 12;
            }
            else if (liczba < 8192 && liczba > 4096)
            {
                return 13;
            }
            else if (liczba < 16384 && liczba > 8192)
            {
                return 14;
            }
            else if (liczba < 32768 && liczba > 16384)
            {
                return 15;
            }
            else if (liczba < 65536 && liczba > 32678)
            {
                return 16;
            }
            else if (liczba < 131072 && liczba > 65536)
            {
                return 17;
            }
            else if (liczba < 262144 && liczba > 131072)
            {
                return 18;
            }
            else if (liczba < 524288 && liczba > 262144)
            {
                return 19;
            }
            else if (liczba < 1048576 && liczba > 524288)
            {
                return 20;
            }
            else
            {
                return 0;
            }
        }


    }
}

using LiveCharts;
using LiveCharts.Wpf;
using Si_projekt.ObektyAlgorytmu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Si_projekt
{
    public partial class Form1 : Form
    {

        private bool decyzjaNR1, decyzjaNR2, decyzjaNR3, decyzjaNR4, decyzjaNR5, decyzjaNR6, decyzjaNR7, decyzjaNR8, decyzjaNR9, decyzjaNR10, decyzjaNR11, decyzjaNR12, decyzjaNR13, decyzjaNR14;
        private List<Populacja> populacjaDoOdczytu = new List<Populacja>();
        public SeriesCollection series2 { get; set; }
        ChartValues<double> val1 = new ChartValues<double>();
        ChartValues<double> val2 = new ChartValues<double>();
        ChartPointLabelConverter chart = new ChartPointLabelConverter();
        List<string>listaCh = new List<string>();        
        private List<KolaRuletokowe> koloRuletkowe = new List<KolaRuletokowe>();

        Func<ChartPoint, string> labelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
        int liczbChrom;
        public Form1()
        {
            InitializeComponent();
            zamknijKontrolki(true);
        }

        private void buttonOblicz_Click(object sender, EventArgs e)
        {
            //deklarowanie ziemnnych 
            string[] pola;
            decimal[] potegi;
            pola = new []{textBoxX1.Text, textBoxY1.Text, textBoxX2.Text, textBoxY2.Text, textBoxX3.Text, textBoxY3.Text, textBoxX4.Text, textBoxY4.Text, textBoxC1.Text};
            potegi = new[] { numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value, numericUpDown4.Value, numericUpDown5.Value, numericUpDown6.Value, numericUpDown7.Value, numericUpDown8.Value };
            zamknijKontrolki(false);
            populacjaDoOdczytu = new List<Populacja>();
            int poputacja, liczbaChromosomów, zakresOd, zakresDo, liczbaBitowa, iloscBitow;
            double dokladnosc;
            string dokladnoscText;

            //zczytanie wartosci z pól
            poputacja = Convert.ToInt32(textBoxILPOP.Text);
            liczbaChromosomów = Convert.ToInt32(textBoxILCH.Text);
            zakresOd = Convert.ToInt32(textBoxOd.Text);
            zakresDo = Convert.ToInt32(textBoxDo.Text);
            dokladnoscText = textBox1.Text;
            liczbaBitowa = Funkcja.liczbaBitowa(zakresOd, zakresDo);
            liczbChrom = liczbaChromosomów;
            //Tworzenie losowej populacji
            Random rzut = new Random();
            string liczbaXBin = "", liczbaYBin = "";
            double[] liczbyX = new double[liczbaChromosomów];
            double[] liczbyY = new double[liczbaChromosomów];
            string[] chromosomy = new string[liczbaChromosomów];
            double[] wyniki = new double[liczbaChromosomów];
            dokladnosc = Funkcja.przeliczDokladnosc(dokladnoscText);
            iloscBitow = Funkcja.obliczIloscBitow(dokladnosc);
            string[] TabPomX = new string[liczbaChromosomów];
            string[] TabPomY = new string[liczbaChromosomów];
            int[] TabPomXx = new int[liczbaChromosomów];
            int[] TabPomYy = new int[liczbaChromosomów];

            for (int idCH = 0; idCH < liczbaChromosomów; idCH++)
            {                
                for (int i = 0; i < iloscBitow; i++)
                {
                    liczbaXBin += rzut.Next(0, 2);
                    liczbaYBin += rzut.Next(0, 2);
                }
                TabPomX[idCH] = liczbaXBin;
                TabPomY[idCH] = liczbaYBin;
                TabPomXx[idCH] = Convert.ToInt32(TabPomX[idCH], 2);
                TabPomYy[idCH] = Convert.ToInt32(TabPomY[idCH], 2);
                chromosomy[idCH] = liczbaXBin + liczbaYBin;
                
                //x = a +((d * x)/(2^n - 1))
                liczbyX[idCH] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaXBin, 2))/((Math.Pow(2,iloscBitow)-1)));
                liczbyY[idCH] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaYBin, 2))/((Math.Pow(2,iloscBitow)-1)));
                liczbaYBin = "";
                liczbaXBin = "";
            }

            //Obliczenie wartosci makymalnej i wypisanie jej na wykres
            string liczbaXBinMax = "", liczbaYBinMax = "";
            for (int i = 0; i < iloscBitow; i++)
            {
                liczbaXBinMax += "1";
                liczbaYBinMax += "1";
            }
          
            int[] TabPomXxx = new int[liczbaChromosomów];
            int[] TabPomYyy = new int[liczbaChromosomów];
            for (int i = 0; i < liczbaChromosomów; i++)
            {
                TabPomXxx[i] = Convert.ToInt32(liczbaXBinMax,2);

            }
            for (int i = 0; i < liczbaChromosomów; i++)
            {
                TabPomYyy[i] = Convert.ToInt32(liczbaYBinMax, 2);

            }
            double najwiekszaWartoscFunkcji = Funkcja.obliczFunkcje(TabPomXxx[0], TabPomYyy[0], pola, potegi);

            val2.Clear();
            for (int i = 0; i < liczbaChromosomów; i++)
            {
                val2.Add(najwiekszaWartoscFunkcji);
            }
            labelNajWWartosc.Visible = true;
            labelNajWWartosc.Text = $"Wykres funkcji przystosowania: {najwiekszaWartoscFunkcji.ToString()}";

            //Obieg populacji
            for (int idPopulacji = 0; idPopulacji < poputacja; idPopulacji++)
            {

                //wyliczenie wartosci osobików w populacji
                wyniki = new double[liczbaChromosomów];
                for (int idCH = 0; idCH < liczbaChromosomów; idCH++)
                {
                    wyniki[idCH] = Funkcja.obliczFunkcje(TabPomXx[idCH], TabPomYy[idCH], pola, potegi);
                }

                //wyliczenie przystosowania osobników w populacji  
                double mianownik = 0;
                for (int i = 0; i < liczbaChromosomów; i++)
                {
                    mianownik += wyniki[i];
                }
                double[] wycinkiKola = new double[liczbaChromosomów];
                for (int i = 0; i < liczbaChromosomów; i++)
                {
                    wycinkiKola[i] = (wyniki[i] / mianownik) * 100.0;
                }

                //wyliczenie zakresów przystosowania osobników
                List<Przedzialy> zakresWycinka = new List<Przedzialy>();
                double first = 0, last = 0;
                for (int i = 0; i < liczbaChromosomów; i++)
                {
                    last += wycinkiKola[i];
                    zakresWycinka.Add(new Przedzialy { min = first, max = last });
                    first += wycinkiKola[i] + 0.01;
                }
                
                //zapisanie informacji o populacji
                populacjaDoOdczytu.Add(new Populacja { numerPopulacji = idPopulacji, X = liczbyX, Y = liczbyY, chromosom = chromosomy, wartosc = wyniki });
                
                //Losowanie chromosomów ruletką
                int losoweLiczby;
                string poomocnik = "";
                string[] wybraneChromosomy = new string[liczbaChromosomów];
                for (int i = 0; i < liczbaChromosomów; i++)
                {
                    losoweLiczby = rzut.Next(0, 101);
                    for (int j = 0; j < liczbaChromosomów; j++)
                    {
                        if (losoweLiczby >= zakresWycinka[j].min && losoweLiczby <= zakresWycinka[j].max)
                        {
                            poomocnik = chromosomy[j];
                            break;
                        }
                    }
                    wybraneChromosomy[i] = poomocnik;
                }

                //mutacja wybranych fomosomów przy szansie podanej od użytkownika 
                double szansaMutacji = 0.0;
                for (int i = 0; i < wybraneChromosomy.Length; i++)
                {
                    szansaMutacji = rzut.NextDouble();
                    if (szansaMutacji <= Convert.ToDouble(numericUpDownMutacja.Value))
                        wybraneChromosomy[i] = OperatoryAlgorytmuGenetycznego.mutacje(wybraneChromosomy[i]);
                }

                //Tworzenie losowych par rodziców.
                rzut.Shuffle(wybraneChromosomy);
                List<Rodzice> para = new List<Rodzice>();
                for (int i = 0; i < wybraneChromosomy.Length; i += 2)
                {
                    para.Add(new Rodzice { rodzic1 = wybraneChromosomy[i], rodzic2 = wybraneChromosomy[i + 1] });
                }

                //Krzyżowanie rodziców
                List<Dzieci> potomkowie = new List<Dzieci>();
                for (int i = 0; i < para.Count; i++)
                {
                    potomkowie.Add(OperatoryAlgorytmuGenetycznego.krzyzowanie(para[i].rodzic1, para[i].rodzic2, iloscBitow));
                }

                //Zapisanie szans na wylosowanie osobnika w populacji
                koloRuletkowe.Add(new KolaRuletokowe { nrPopulacji = idPopulacji, wycinki = wycinkiKola, chromosomy = chromosomy });

                //wyciągnięcie wyrazów x i y z potomków
                liczbyY = new double[liczbaChromosomów];
                liczbyX = new double[liczbaChromosomów];
                for (int i = 0, idTab = 0; i < potomkowie.Count; i++, idTab += 2)
                {
                    //x y z potomka pierwszego
                    liczbaXBin = "";
                    liczbaYBin = "";
                    for (int j = 0; j < potomkowie[i].dziecko1.Length; j++)
                    {
                        string pomocne = potomkowie[i].dziecko1;
                        if (j <= potomkowie[i].punktPrzeciecia)
                            liczbaXBin += pomocne[j];
                        else
                            liczbaYBin += pomocne[j];
                    }
                    TabPomXx[idTab] = Convert.ToInt32(liczbaXBin, 2);
                    TabPomYy[idTab] = Convert.ToInt32(liczbaYBin, 2);
                    liczbyY[idTab] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaYBin, 2)) / ((Math.Pow(2, iloscBitow) - 1)));
                    liczbyX[idTab] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaXBin, 2)) / ((Math.Pow(2, iloscBitow) - 1)));

                    //x y z potomka drugiego
                    liczbaXBin = "";
                    liczbaYBin = "";
                    for (int j = 0; j < potomkowie[i].dziecko2.Length; j++)
                    {
                        string pomocne = potomkowie[i].dziecko2;
                        if (j <= potomkowie[i].punktPrzeciecia)
                            liczbaXBin += pomocne[j];
                        else
                            liczbaYBin += pomocne[j];
                    }
                    TabPomXx[idTab + 1] = Convert.ToInt32(liczbaXBin, 2);
                    TabPomYy[idTab + 1] = Convert.ToInt32(liczbaYBin, 2);
                    liczbyX[idTab + 1] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaXBin, 2)) / ((Math.Pow(2, iloscBitow) - 1)));
                    liczbyY[idTab + 1] = zakresOd + ((liczbaBitowa * Convert.ToInt32(liczbaYBin, 2)) / ((Math.Pow(2, iloscBitow) - 1)));
                }

                //tworzenie nowej populacji z potomków
                int licznikTablicy = 0;
                chromosomy = new string[liczbaChromosomów];
                foreach (var element in potomkowie)
                {
                    chromosomy[licznikTablicy] = element.dziecko1;
                    licznikTablicy++;
                    chromosomy[licznikTablicy] = element.dziecko2;
                    licznikTablicy++;
                }
                

            }

            //koniec wykresu
            comboBox1.Enabled = true;
            label1.Text = "Wybierz Populacje";
            //zapis danych do kontrolek
            foreach (var item in populacjaDoOdczytu)
            {
                comboBox1.Items.Add(item.numerPopulacji);
            }
           
            //ustawienie parametów wykresu
            series2 = new SeriesCollection {
                new LineSeries
                {
                    Title = "Watrosci",
                    LineSmoothness = 0,
                    StrokeThickness = 3,
                    Values = val1,
                    
                },
                new LineSeries
                {
                    Title = "Największa wartosc",
                    LineSmoothness = 0,
                    StrokeThickness = 3,
                    Values = val2, 
                }
            };

        }
        //*************************************************************************************


        

        //akcje po wybraniu wartości z listy
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //deklaracja i ustawianie parametów
            int wybraneID = comboBox1.SelectedIndex;
            SeriesCollection series = new SeriesCollection();
            int licznika = 0;
            string zmPomA="";
            label24.Visible = true;
            pieChart1.Visible = true;
            labelNajWOsobnik.Visible = true;

            //dodanie danych do koła wykresowego.
            for (int i = 0; i < koloRuletkowe.Count; i++)
            {
                if (wybraneID == i)
                {
                    for (int j = 0; j < koloRuletkowe[i].wycinki.Length; j++)
                    {
                        series.Add(new PieSeries() {  Title =koloRuletkowe[i].chromosomy[j], Values = new ChartValues<double> { koloRuletkowe[i].wycinki[j] }, DataLabels = true, LabelPoint = labelPoint});
                        pieChart1.Series = series;
                    }
                    for (int id = 0; id < populacjaDoOdczytu[i].chromosom.Length; id++)
                    {
                        zmPomA = populacjaDoOdczytu[i].chromosom[id].ToString();
                        for (int k = 0; k < zmPomA.Length; k++)
                        {
                            if (zmPomA[k] == '1')
                                licznika++;
                        }
                        licznika = 0;
                        zmPomA = "";
                    }
                }    
            }

            //Znajdowanie najlepszego osobnika w populacji
            double najlepszaWartosc = 0;
            string najlepszyosobnik = "";
            for (int idPop = 0; idPop < populacjaDoOdczytu.Count; idPop++)
            {
                if(wybraneID == idPop)
                {
                    for (int idElementu = 0; idElementu < populacjaDoOdczytu[idPop].chromosom.Length; idElementu++)
                    {
                        if (najlepszaWartosc < populacjaDoOdczytu[idPop].wartosc[idElementu])
                        {
                            najlepszaWartosc = populacjaDoOdczytu[idPop].wartosc[idElementu];
                            najlepszyosobnik = populacjaDoOdczytu[idPop].chromosom[idElementu];
                        }
                    }
                }
            }
            labelNajWOsobnik.Text = $"Najlepszy osobnik to: {najlepszyosobnik} \nO wartość: {najlepszaWartosc}";
            najlepszyOsobnik = najlepszyosobnik;

            //wproawadzenie wartosci do wykresu 
            val1.Clear();
            labelLegendCh.Text = "";
            
            string pomocniczaLegenda="";
            int licznik=1;
            for (int i = 0; i < populacjaDoOdczytu.Count; i++)
            {
                if (i==wybraneID)
                {                 
                    for (int j = 0; j < populacjaDoOdczytu[i].chromosom.Length; j++)
                    {
                       // val1.Add(populacjaDoOdczytu[i].wartosc[j]);
                        pomocniczaLegenda += $"{populacjaDoOdczytu[i].chromosom[j]} ";
                        labelLegendCh.Text += $"Id {j} chromosom: {populacjaDoOdczytu[i].chromosom[j]} \n";
                        licznik++;

                    }
                } 
            }

            //to do końca ma budować tworzyć wykres
            string[] rozdzieloneWartosciPopulacji =new string[licznik];
            rozdzieloneWartosciPopulacji=legenda(pomocniczaLegenda,licznik);
            WartosciXY wartosciXY=wartosci(rozdzieloneWartosciPopulacji, licznik);
  
            double[] wyniki = new double[liczbChrom];
            string[] pola;
            decimal[] potegi;
            pola = new[] { textBoxX1.Text, textBoxY1.Text, textBoxX2.Text, textBoxY2.Text, textBoxX3.Text, textBoxY3.Text, textBoxX4.Text, textBoxY4.Text, textBoxC1.Text };
            potegi = new[] { numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value, numericUpDown4.Value, numericUpDown5.Value, numericUpDown6.Value, numericUpDown7.Value, numericUpDown8.Value };
            for (int idCH = 0; idCH < liczbChrom; idCH++)
            {
                wyniki[idCH] = Funkcja.obliczFunkcje(wartosciXY.wartosciX[idCH], wartosciXY.wartosciY[idCH], pola, potegi);
            }
            for (int i = 0; i < wyniki.Length; i++)
            {
                val1.Add(wyniki[i]);
            }
            
            cartesianChart1.Series = series2;
            //dotąd
        }

        //odseparowanie wartosci z danej populacji i wrzucenie jej w inną tablice
        public string [] legenda(string legenda, int j)
        {
            string pomocLegenda = "", pomocLeg="";
            int k = 0;
            string[] kolejnaPomLeg = new string[j];
            for (int i = 0; i < legenda.Length; i++)
            {
                if (legenda[i] == ' ')
                {
                    pomocLeg = pomocLegenda;
                    kolejnaPomLeg[k] = pomocLeg;
                    pomocLeg = "";
                    pomocLegenda = "";
                    k++;
                   
                }
                else
                    pomocLegenda += legenda[i];

            }
            return kolejnaPomLeg;
                
        }
  //odseparowanie z chromosoma X i Y (potrzebne do obliczenia wyników w funkcji)
        public  WartosciXY wartosci(string[] legenda,int licz)
        {
            int[] wartosciX=new int[licz];
            int[] wartosciY = new int[licz];
            string zmPom = "";
            int k;
            for (int i = 0; i < licz; i++)
            {
                string zmPom2 = "";
                string zmPom3 = "";
                try
                {
                    zmPom = legenda[i];
                    int j = zmPom.Length;
                    k = j / 2;
                }
                catch (Exception) { break; }
                
                for (int l = 0; l < zmPom.Length; l++)
                {
                    if (l < k)
                    {
                        zmPom2 += zmPom[l];
                    }
                    else
                    {
                        zmPom3 += zmPom[l];
                    }
                }
                wartosciX[i] = Convert.ToInt32(zmPom2, 2);
                wartosciY[i] = Convert.ToInt32(zmPom3, 2);
            }
            WartosciXY wartosc = new WartosciXY();
            wartosc.wartosciX = wartosciX;
            wartosc.wartosciY = wartosciY;
            return wartosc;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //potrzebne do wykresu      
            pieChart1.LegendLocation = LegendLocation.Bottom;
            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }

        

        //zarzedzanie kontrolkami 
        private void zamknijKontrolki(bool guzik)
        {
            if(guzik == true)
                buttonOblicz.Enabled = false;
            comboBox1.Enabled = false;
            label1.Text = "";
            label24.Visible = false;
            labelNajWWartosc.Visible = false;
            labelNajWOsobnik.Visible = false;
            comboBox1.Items.Clear();
            comboBox1.SelectedIndex = -1;
            populacjaDoOdczytu.Clear();
            labelCHErrorr.Text = "";
            labelPoPErrorr.Text = "";
            koloRuletkowe.Clear();
            pieChart1.Visible = false;
            labelLegendCh.Text = "";
            cartesianChart1.Series.Clear();
        }


        //kontorla wprowadzonych wartości do pola z populacją
        private void textBoxILPOP_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR1 = MenadzerPol.sprawdzPola(textBoxILPOP, labelPoPErrorr, textBoxILPOP.Text, "Populacja", false, false);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        string najlepszyOsobnik = "";
        private void button1_zapiszDaneDoPliku_Click(object sender, EventArgs e)
        {

            string tekst = "Najlepszy osobnik z populacji " + comboBox1.Text + " to: " + najlepszyOsobnik + "\n" + "Nasza funkcja prezentuję się w ten sposób: "+
                "\n" + $" {textBoxX1.Text}*X^({numericUpDown1.Value.ToString()})*{textBoxY1.Text}*Y^({numericUpDown2.Value.ToString()})" +
              $"{textBoxX2.Text}*X^({numericUpDown3.Value.ToString()})*{textBoxY2.Text}*Y^({numericUpDown4.Value.ToString()})"
              + $"{textBoxX3.Text}*X^({numericUpDown5.Value.ToString()})*{textBoxY3.Text}*Y^({numericUpDown6.Value.ToString()})"
              + $"{textBoxX4.Text}*X^({numericUpDown7.Value.ToString()})+{textBoxY4.Text}*Y^({numericUpDown8.Value.ToString()})"
              + $"{textBoxC1.Text}";
            File.WriteAllText(@"d:\x.txt", tekst);
            File.AppendAllText(@"d:\x.txt", Environment.NewLine);
     
     


        }

        //kontorla wprowadzonych wartości do pola z liczbą chromosomów 
        private void textBoxILCH_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR2 = MenadzerPol.sprawdzPola(textBoxILCH, labelCHErrorr, textBoxILCH.Text, "Ilość chromosomów", false, false);
            if (decyzjaNR2 == true)
                decyzjaNR2 = MenadzerPol.sprawdzCzyParzyste(textBoxILCH, labelCHErrorr, textBoxILCH.Text, "Ilość chromosomów");
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }

        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR3 = MenadzerPol.sprawdzPola(textBoxX1, labelErrorP1, textBoxX1.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }

        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxY1_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR4 = MenadzerPol.sprawdzPola(textBoxY1, labelErrorP2, textBoxY1.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }

        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR5 = MenadzerPol.sprawdzPola(textBoxX2, labelErrorP3, textBoxX2.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxY2_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR6 = MenadzerPol.sprawdzPola(textBoxY2, labelErrorP4, textBoxY2.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxX3_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR7 = MenadzerPol.sprawdzPola(textBoxX3, labelErrorP5, textBoxX3.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z zakresem
        private void textBoxOd_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR12 = MenadzerPol.sprawdzPola(textBoxOd, labelPrzedErrorr, textBoxOd.Text, "Pola przedziału",false,true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z zakresem
        private void textBoxDo_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR13 = MenadzerPol.sprawdzPola(textBoxDo, labelPrzedErrorr, textBoxDo.Text, "Pola przedziału",false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z szansa mutacji
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR14 = MenadzerPol.sprawdzPola(textBox1, labelDoklaErrorr, textBox1.Text, "Pola dokładności",true, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxY3_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR8 = MenadzerPol.sprawdzPola(textBoxY3, labelErrorP6, textBoxY3.Text, "Pola w funkcji",false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxX4_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR9 = MenadzerPol.sprawdzPola(textBoxX4, labelErrorP7, textBoxX4.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxY4_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR10 = MenadzerPol.sprawdzPola(textBoxY4, labelErrorP8, textBoxY4.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }
        //kontorla wprowadzonych wartości do pola z funkcję
        private void textBoxC1_TextChanged(object sender, EventArgs e)
        {
            decyzjaNR11 = MenadzerPol.sprawdzPola(textBoxC1, labelErrorP9, textBoxC1.Text, "Pola w funkcji", false, true);
            if (decyzjaNR1 == true && decyzjaNR2 == true && decyzjaNR3 == true && decyzjaNR4 == true && decyzjaNR5 == true && decyzjaNR6 == true && decyzjaNR7 == true && decyzjaNR8 == true && decyzjaNR9 == true && decyzjaNR10 == true && decyzjaNR11 == true && decyzjaNR12 == true && decyzjaNR13 == true && decyzjaNR14 == true)
                buttonOblicz.Enabled = true;
            else
            {
                buttonOblicz.Enabled = false;
                comboBox1.Enabled = false;
                label1.Text = "";
            }
        }


    }
}

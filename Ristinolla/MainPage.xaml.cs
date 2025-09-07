using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using CommunityToolkit.Maui;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ristinolla
{

    public partial class MainPage : ContentPage
    {
        
        Pelaajien_tiedotcs tiedot = new();
        // määritetään tiedoston tallesosoite tiedostoifin
        public static readonly string TallennusOsoite = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ristinolla");
        
        public ObservableCollection<Pelaajien_tiedotcs> PelaajaLista = new ObservableCollection<Pelaajien_tiedotcs>();

        public DateTime alkamisaika;

        public DateTime loppumisAika;
        int count = 0;
        // random
        Random rnd = new Random();
        int tauko;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;
            alkamisaika = DateTime.Now;
            vuorottaja();
            // asettaa taustavärin
            var OmaVari = Color.FromArgb("FF210034"); 
            BackgroundColor = OmaVari;

            TiedotListaan();
            ListaKurkkaaja.ItemsSource = PelaajaLista;
            
            LoadTiedotAsync();


        }
        int lukko, lukko1, lukko2, lukko3, lukko4, lukko5, lukko6, lukko7, lukko8 = 0;
        // takaisin meno nappi palauttaa Aloitus sivulle ja nollaa sivun
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new AloitusSivu());
            AloitusSivu.tekoälypeli = 0;
            return true;
            AlustaAloittaja_Clicked( null,EventArgs.Empty);
            
        }


        // jos voittaa niin lukitsee napit ja kertoo voittajan  x:lle ja o:lle
        public void XVoittaa()
        {
            loppumisAika = DateTime.Now;
            lukko = 1;
            lukko1 = 1;
            lukko2 = 1;
            lukko3 = 1;
            lukko4 = 1;
            lukko5 = 1;
            lukko6 = 1;
            lukko7 = 1;
            lukko8 = 1;
            DisplayAlert("Voittaja", "X voittaa", "ok");
            VuoronKertoja.Text = string.Empty;
            Ajanlaskija(TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1, TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2);
            Voittokirjoittaja(TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1, TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2);
        }

        public void OVoittaa()
        {

            lukko = 1;
            lukko1 = 1;
            lukko2 = 1;
            lukko3 = 1;
            lukko4 = 1;
            lukko5 = 1;
            lukko6 = 1;
            lukko7 = 1;
            lukko8 = 1;
            DisplayAlert("Voittaja", "O voittaa", "ok");
            VuoronKertoja.Text = string.Empty;
            loppumisAika = DateTime.Now;
            Ajanlaskija(TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1, TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2);
            Voittokirjoittaja(TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2, TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1);
        }

        // laskee kummanko vuoro on ja pyörittää tekoälyn mikäli pelaaja haluaa pelata sitä vastaan
        public async void vuorottaja()
        {
            if (count % 2 == 0)
            {
                VuoronKertoja.Text = "X:n vuoro";
                if (AloitusSivu.tekoälypeli == 1 && TietoKysyminen.TekoälynMerkki == "X")
                {
                    
                    MietintaAika();
                    await Task.Delay(tauko);
                    AiLogikka();

                }
            }
            else
            {
                VuoronKertoja.Text = "O:n vuoro";
                if (AloitusSivu.tekoälypeli == 1 && TietoKysyminen.TekoälynMerkki == "O")
                {
                    MietintaAika();
                    await Task.Delay(tauko);
                    AiLogikkaO();
                }
            }
        }
        // jokaisessa napissa sama toimivuus eli jos painat niin saat x tai y riippuen vuorosta ja niissä on myös funtio joka tarkistaa ja vaihtaa vuoron ja tekoälyn ja tarkistaa voiton
        public void Nappi00_Clicked(object sender, EventArgs e)
        {
            if (count % 2 == 0 && lukko == 0)
            {
                Nappi00.Text = "X";
                kuva00.Source = "x_nappi.png";
                count++;
                lukko = 1;
            }
            else if (lukko == 0)
            {
                Nappi00.Text = "O";
                kuva00.Source = "o_nappi.png";
                count++;
                lukko = 1;
            }
            VoittoTarkistaja();
            vuorottaja();

        }

        public void Nappi01_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko1 == 0)
            {
                kuva01.Source = "x_nappi.png";
                count++;
                lukko1 = 1;
                Nappi01.Text = "X";

            }
            else if (lukko1 == 0)
            {
                kuva01.Source = "o_nappi.png";
                count++;
                lukko1 = 1;
                Nappi01.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi02_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko2 == 0)
            {
                kuva02.Source = "x_nappi.png";
                count++;
                lukko2 = 1;
                Nappi02.Text = "X";
            }
            else if (lukko2 == 0)
            {
                kuva02.Source = "o_nappi.png";
                count++;
                lukko2 = 1;
                Nappi02.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi10_Clicked(object sender, EventArgs e)
        {
            if (count % 2 == 0 && lukko3 == 0)
            {
                kuva10.Source = "x_nappi.png";
                count++;
                lukko3 = 1;
                Nappi10.Text = "X";
            }
            else if (lukko3 == 0)
            {
                kuva10.Source = "o_nappi.png";
                count++;
                lukko3 = 1;
                Nappi10.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi11_Clicked(object sender, EventArgs e)
        {
            if (count % 2 == 0 && lukko4 == 0)
            {
                kuva11.Source = "X_nappi.png";
                count++;
                lukko4 = 1;
                Nappi11.Text = "X";
            }
            else if (lukko4 == 0)
            {
                kuva11.Source = "o_nappi.png";
                count++;
                lukko4 = 1;
                Nappi11.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi12_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko5 == 0)
            {
                kuva12.Source = "x_nappi.png";
                count++;
                lukko5 = 1;
                Nappi12.Text = "X";
            }
            else if (lukko5 == 0)
            {
                kuva12.Source = "o_nappi.png";
                count++;
                lukko5 = 1;
                Nappi12.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi20_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko6 == 0)
            {
                kuva20.Source = "x_nappi.png";
                count++;
                lukko6 = 1;
                Nappi20.Text = "X";
            }
            else if (lukko6 == 0)
            {
                kuva20.Source = "o_nappi.png";
                count++;
                lukko6 = 1;
                Nappi20.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi21_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko7 == 0)
            {
                kuva21.Source = "x_nappi.png";
                count++;
                lukko7 = 1;
                Nappi21.Text = "X";
            }
            else if (lukko7 == 0)
            {
                kuva21.Source = "o_nappi.png";
                count++;
                lukko7 = 1;
                Nappi21.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        public void Nappi22_Clicked(object sender, EventArgs e)
        {

            if (count % 2 == 0 && lukko8 == 0)
            {
                kuva22.Source = "x_nappi.png";
                count++;
                lukko8 = 1;
                Nappi22.Text = "X";
            }
            else if (lukko8 == 0)
            {
                kuva22.Source = "o_nappi.png";
                count++;
                lukko8 = 1;
                Nappi22.Text = "O";
            }
            VoittoTarkistaja();
            vuorottaja();
        }

        // tarkistaa jokaisen mahdollisen voitto rivin / tasapelin ja kutsuu sitten functiota, joka keroo käyttäjälle, että joku voitti, tai pelasi tasapelin
        public void VoittoTarkistaja()
        {

            if (Nappi00.Text == "X" && Nappi01.Text == "X" && Nappi02.Text == "X" || Nappi10.Text == "X" && Nappi11.Text == "X" && Nappi12.Text == "X" || Nappi20.Text == "X" && Nappi21.Text == "X" && Nappi22.Text == "X"
                || Nappi00.Text == "X" && Nappi10.Text == "X" && Nappi20.Text == "X" || Nappi01.Text == "X" && Nappi11.Text == "X" && Nappi21.Text == "X" || Nappi02.Text == "X" && Nappi12.Text == "X" && Nappi22.Text == "X"
                || Nappi00.Text == "X" && Nappi11.Text == "X" && Nappi22.Text == "X" || Nappi02.Text == "X" && Nappi11.Text == "X" && Nappi20.Text == "X")
            {
                XVoittaa();
                count = 9;
            }

            else if (Nappi00.Text == "O" && Nappi01.Text == "O" && Nappi02.Text == "O" || Nappi10.Text == "O" && Nappi11.Text == "O" && Nappi12.Text == "O" || Nappi20.Text == "O" && Nappi21.Text == "O" && Nappi22.Text == "O"
                || Nappi00.Text == "O" && Nappi10.Text == "O" && Nappi20.Text == "O" || Nappi01.Text == "O" && Nappi11.Text == "O" && Nappi21.Text == "O" || Nappi02.Text == "O" && Nappi12.Text == "O" && Nappi22.Text == "O"
                || Nappi00.Text == "O" && Nappi11.Text == "O" && Nappi22.Text == "O" || Nappi02.Text == "O" && Nappi11.Text == "O" && Nappi20.Text == "O")
            {
                OVoittaa();
                count = 9;

            }
            else if (count == 9)
            {
                loppumisAika = DateTime.Now;
                DisplayAlert("Tasapeli", "Peli päätty tasapeliin", "ok");
                Ajanlaskija(TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1, TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2);
                TasapeliKirjoittaja(TietoKysyminen.Etunimi1, TietoKysyminen.Sukunimi1, TietoKysyminen.SyntymaAika1, TietoKysyminen.Etunimi2, TietoKysyminen.Sukunimi2, TietoKysyminen.SyntymaAika2);
                count = 9;

            }
        }
        // nollaa nappien lukituksen ja pelimerkit ja aloitusajan pelinajanlaskua varten
        private void AlustaAloittaja_Clicked(object sender, EventArgs e)
        {
            lukko = 0;
            lukko1 = 0;
            lukko2 = 0;
            lukko3 = 0;
            lukko4 = 0;
            lukko5 = 0;
            lukko6 = 0;
            lukko7 = 0;
            lukko8 = 0;
            count = 0;

            Nappi00.Text = string.Empty;
            Nappi01.Text = string.Empty;
            Nappi02.Text = string.Empty;
            Nappi10.Text = string.Empty;
            Nappi11.Text = string.Empty;
            Nappi12.Text = string.Empty;
            Nappi20.Text = string.Empty;
            Nappi21.Text = string.Empty;
            Nappi22.Text = string.Empty;
            VuoronKertoja.Text = default;

            kuva00.Source = "dotnet_bot";
            kuva01.Source = "dotnet_bot";
            kuva02.Source = "dotnet_bot";
            kuva10.Source = "dotnet_bot";
            kuva11.Source = "dotnet_bot";
            kuva12.Source = "dotnet_bot";
            kuva20.Source = "dotnet_bot";
            kuva21.Source = "dotnet_bot";
            kuva22.Source = "dotnet_bot";
            alkamisaika = DateTime.Now;
            vuorottaja();
        }

        // lukee json tiedoston ja tallentaa sen listaan
        public async Task<List<Pelaajien_tiedotcs>> TiedotListaan()
        {
            if (File.Exists(TallennusOsoite))
            {
                string json = await File.ReadAllTextAsync(TallennusOsoite);
                return JsonSerializer.Deserialize<List<Pelaajien_tiedotcs>>(json) ?? new List<Pelaajien_tiedotcs>();
            }
            return new List<Pelaajien_tiedotcs>();
        }
        // Muuntaa tiedot listaan, strucktin muodossa.
        private async void LoadTiedotAsync()
        {
            var tiedot = await TiedotListaan();
            foreach (var tieto in tiedot)
            {
                Pelaajien_tiedotcs arvo = tieto;
                PelaajaLista.Add(arvo);
            }
        }

        private async void ListaKurkkaaja_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Pelaajien_tiedotcs selectedItem)
            {
                await DisplayAlert("Item Tapped", $"You tapped on {selectedItem.Etunimi}", "OK");
            }
        }
        // Pelaajan voittaessa lukee json tiedot, muuttaa ne luettavaksi ja sitten etsii pelaajan tietoja vastaavan ja kirjoittaa voitonn tai häviön ja sitten muuntaa takaisin json tiedostoon
        public void Voittokirjoittaja(string Enimi, string Snimi, string Saika, string Enimi2, string Snimi2, string Saika2)
        {
            string jsonString = File.ReadAllText(TallennusOsoite);

            JArray data = JArray.Parse(jsonString);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (item["Voitot"] != null && item["Etunimi"].ToString() == Enimi && item["Sukunimi"].ToString() == Snimi && item["Syntymavuosi"].ToString() == Saika)
                {
                    {
                        int voittohetki = (int)item["Voitot"];

                        voittohetki++;

                        item["Voitot"] = voittohetki;
                    }
                }
                if (item["Haviot"] != null && item["Etunimi"].ToString() == Enimi2 && item["Sukunimi"].ToString() == Snimi2 && item["Syntymavuosi"].ToString() == Saika2)
                {
                    {
                        int Haviohetki = (int)item["Haviot"];

                        Haviohetki++;

                        item["Haviot"] = Haviohetki;
                    }
                }
            }

            string updatedJsonString = data.ToString((Newtonsoft.Json.Formatting)Formatting.Indented);

            File.WriteAllText(TallennusOsoite, updatedJsonString);

            PelaajaLista.Clear();
            LoadTiedotAsync();
        }

        // Kirjoittaa tasapelin tilaneessa tuloksen ylos samalla logiikalla kuin ylempi
        public void TasapeliKirjoittaja(string Enimi, string Snimi, string Saika, string Enimi2, string Snimi2, string Saika2)
        {
            string jsonString = File.ReadAllText(TallennusOsoite);

            JArray data = JArray.Parse(jsonString);

            string etunimi = TietoKysyminen.Etunimi1;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (item["TasaPelit"] != null && item["Etunimi"].ToString() == Enimi)
                {
                    {
                        int voittohetki = ((int)item["TasaPelit"]);

                        voittohetki++;

                        item["TasaPelit"] = voittohetki;
                    }
                }
                if (item["TasaPelit"] != null && item["Etunimi"].ToString() == Enimi2)
                {
                    {
                        int voittohetki = ((int)item["TasaPelit"]);

                        voittohetki++;

                        item["TasaPelit"] = voittohetki;
                    }
                }
            }
            string updatedJsonString = data.ToString((Newtonsoft.Json.Formatting)Formatting.Indented);
            File.WriteAllText(TallennusOsoite, updatedJsonString);
            PelaajaLista.Clear();
            LoadTiedotAsync();
        }
        // kirjaa pelatun ajan ylös samalla logiigalla
        public void Ajanlaskija(string Enimi, string Snimi, string Saika, string Enimi2, string Snimi2, string Saika2)
        {
            string jsonString = File.ReadAllText(TallennusOsoite);

            JArray data = JArray.Parse(jsonString);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (item["Peliaika"] != null && item["Etunimi"].ToString() == Enimi && item["Sukunimi"].ToString() == Snimi && item["Syntymavuosi"].ToString() == Saika ||
                    item["Peliaika"] != null && item["Etunimi"].ToString() == Enimi2 && item["Sukunimi"].ToString() == Snimi2 && item["Syntymavuosi"].ToString() == Saika2)
                {
                    {
                        System.TimeSpan peliAika = loppumisAika - alkamisaika;
                        double peliaikatieto = peliAika.TotalSeconds;
                        double olemassaOlevaPeliaika = (double)item["Peliaika"];
                        peliaikatieto = peliaikatieto+ olemassaOlevaPeliaika;
                        peliaikatieto = peliaikatieto/ 60;
                        peliaikatieto = Math.Round(peliaikatieto, 2);
                        item["Peliaika"] = peliaikatieto;
                    }
                }
            }
            string updatedJsonString = data.ToString((Newtonsoft.Json.Formatting)Formatting.Indented);
            File.WriteAllText(TallennusOsoite, updatedJsonString);
        }
        // Tekoälyn logikka , ensiksi pyrkii laittaa keskelle, jos ei pysty, tarkistaa onko avoimia kahden merkin rivejä, laitta jos pystyy, muuten laittaa merkin satunnaisesti
        public void AiLogikkaO()
        {
            if (count % 2 != 0 && count < 9)
            {
                if (count == 0 && Nappi11.Text != "O")
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                // ylärivi vaaka
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi01.Text == "O" && Nappi02.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi01.Text == string.Empty && Nappi02.Text == "O")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi01.Text == "O" && Nappi02.Text == string.Empty)
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                // keskirivi vaaka
                else if (count % 2 != 0 && Nappi10.Text == string.Empty && Nappi11.Text == "O" && Nappi12.Text == "O")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi10.Text == "O" && Nappi11.Text == string.Empty && Nappi12.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi10.Text == "O" && Nappi11.Text == "O" && Nappi12.Text == string.Empty)
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                // alarivi vaaka
                else if (count % 2 != 0 && Nappi20.Text == "O" && Nappi21.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi20.Text == "O" && Nappi21.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi20.Text == string.Empty && Nappi21.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                // Ylhäältä alas eka rivi
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi10.Text == "O" && Nappi20.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi10.Text == string.Empty && Nappi20.Text == "O")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi10.Text == "O" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                //Toinen rivi ylhäältä
                else if (count % 2 != 0 && Nappi01.Text == string.Empty && Nappi11.Text == "O" && Nappi21.Text == "O")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi01.Text == "O" && Nappi11.Text == string.Empty && Nappi21.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi01.Text == "O" && Nappi11.Text == "O" && Nappi21.Text == string.Empty)
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                // kolmas rivi ylhäältä 
                else if (count % 2 != 0 && Nappi02.Text == string.Empty && Nappi12.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "O" && Nappi12.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "O" && Nappi12.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }

                // vinorivit
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi11.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "O" && Nappi11.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi11.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "O" && Nappi11.Text == "O" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "O" && Nappi11.Text == string.Empty && Nappi20.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == string.Empty && Nappi11.Text == "O" && Nappi20.Text == "O")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }

                // estäminen
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi01.Text == "X" && Nappi02.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi01.Text == string.Empty && Nappi02.Text == "X")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi01.Text == "X" && Nappi02.Text == string.Empty)
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi10.Text == string.Empty && Nappi11.Text == "X" && Nappi12.Text == "X")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi10.Text == "X" && Nappi11.Text == string.Empty && Nappi12.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi10.Text == "X" && Nappi11.Text == "X" && Nappi12.Text == string.Empty)
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi20.Text == "X" && Nappi21.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi20.Text == "X" && Nappi21.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi20.Text == string.Empty && Nappi21.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                // Ylhäältä alas eka rivi
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi10.Text == "X" && Nappi20.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi10.Text == string.Empty && Nappi20.Text == "X")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi10.Text == "X" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                //Toinen rivi ylhäältä
                else if (count % 2 != 0 && Nappi01.Text == string.Empty && Nappi11.Text == "X" && Nappi21.Text == "X")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi01.Text == "X" && Nappi11.Text == string.Empty && Nappi21.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi01.Text == "X" && Nappi11.Text == "X" && Nappi21.Text == string.Empty)
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                // kolmas rivi ylhäältä 
                else if (count % 2 != 0 && Nappi02.Text == string.Empty && Nappi12.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "X" && Nappi12.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "X" && Nappi12.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                // vinorivit
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi11.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == "X" && Nappi11.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi00.Text == string.Empty && Nappi11.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "X" && Nappi11.Text == "X" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "X" && Nappi11.Text == string.Empty && Nappi20.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == string.Empty && Nappi11.Text == "X" && Nappi20.Text == "X")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0)
                {
                    int satunnaisNappi = rnd.Next(0, 9);

                    if (satunnaisNappi == 0)
                    {
                        Nappi00_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 1)
                    {
                        Nappi01_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 2)
                    {
                        Nappi02_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 3)
                    {
                        Nappi10_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 4)

                    {
                        Nappi11_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 5)

                    {
                        Nappi12_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 6)
                    {
                        Nappi20_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 7)
                    {
                        Nappi21_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 8)
                    {
                        Nappi22_Clicked(null, EventArgs.Empty);

                    }
                }
            }
        }
        // Tekoälyn logikka Sama mutta x:lle , ensiksi pyrkii laittaa keskelle, jos ei pysty, tarkistaa onko avoimia kahden merkin rivejä, laitta jos pystyy, muuten laittaa merkin satunnaisesti
        public void AiLogikka()
        {
            if (count % 2 == 0 && count <9)
            {

                if (count == 0 && Nappi11.Text != "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                //Ylärivi
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi01.Text == "X" && Nappi02.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi01.Text == string.Empty && Nappi02.Text == "X")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi01.Text == "X" && Nappi02.Text == string.Empty)
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                // keskirivi
                else if (count % 2 == 0 && Nappi10.Text == string.Empty && Nappi11.Text == "X" && Nappi12.Text == "X")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi10.Text == "X" && Nappi11.Text == string.Empty && Nappi12.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi10.Text == "X" && Nappi11.Text == "X" && Nappi12.Text == string.Empty)
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                // alarivi
                else if (count % 2 == 0 && Nappi20.Text == "X" && Nappi21.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi20.Text == "X" && Nappi21.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi20.Text == string.Empty && Nappi21.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                // Ylhäältä alas eka rivi
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi10.Text == "X" && Nappi20.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi10.Text == string.Empty && Nappi20.Text == "X")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi10.Text == "X" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                //Toinen rivi ylhäältä
                else if (count % 2 == 0 && Nappi01.Text == string.Empty && Nappi11.Text == "X" && Nappi21.Text == "X")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi01.Text == "X" && Nappi11.Text == string.Empty && Nappi21.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi01.Text == "X" && Nappi11.Text == "X" && Nappi21.Text == string.Empty)
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                // kolmas rivi ylhäältä 
                else if (count % 2 == 0 && Nappi02.Text == string.Empty && Nappi12.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "X" && Nappi12.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 != 0 && Nappi02.Text == "X" && Nappi12.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }

                // vinorivit
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi11.Text == "X" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "X" && Nappi11.Text == string.Empty && Nappi22.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi11.Text == "X" && Nappi22.Text == "X")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "X" && Nappi11.Text == "X" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "X" && Nappi11.Text == string.Empty && Nappi20.Text == "X")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == string.Empty && Nappi11.Text == "X" && Nappi20.Text == "X")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                // Estäminen
                // ylärivi
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi01.Text == "O" && Nappi02.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi01.Text == string.Empty && Nappi02.Text == "O")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi01.Text == "O" && Nappi02.Text == string.Empty)
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                // keskirivi
                else if (count % 2 == 0 && Nappi10.Text == string.Empty && Nappi11.Text == "O" && Nappi12.Text == "O")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi10.Text == "O" && Nappi11.Text == string.Empty && Nappi12.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi10.Text == "O" && Nappi11.Text == "O" && Nappi12.Text == string.Empty)
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                // alarivi
                else if (count % 2 == 0 && Nappi20.Text == "O" && Nappi21.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi20.Text == "O" && Nappi21.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi20.Text == string.Empty && Nappi21.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                // Ylhäältä alas eka rivi
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi10.Text == "O" && Nappi20.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi10.Text == string.Empty && Nappi20.Text == "O")
                {
                    Nappi10_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi10.Text == "O" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                //Toinen rivi ylhäältä
                else if (count % 2 == 0 && Nappi01.Text == string.Empty && Nappi11.Text == "O" && Nappi21.Text == "O")
                {
                    Nappi01_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi01.Text == "O" && Nappi11.Text == string.Empty && Nappi21.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi01.Text == "O" && Nappi11.Text == "O" && Nappi21.Text == string.Empty)
                {
                    Nappi21_Clicked(null, EventArgs.Empty);
                }
                // kolmas rivi ylhäältä 
                else if (count % 2 == 0 && Nappi02.Text == string.Empty && Nappi12.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "O" && Nappi12.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi12_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "O" && Nappi12.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                // vinorivit
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi11.Text == "O" && Nappi22.Text == string.Empty)
                {
                    Nappi22_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == "O" && Nappi11.Text == string.Empty && Nappi22.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi00.Text == string.Empty && Nappi11.Text == "O" && Nappi22.Text == "O")
                {
                    Nappi00_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "O" && Nappi11.Text == "O" && Nappi20.Text == string.Empty)
                {
                    Nappi20_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == "O" && Nappi11.Text == string.Empty && Nappi20.Text == "O")
                {
                    Nappi11_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0 && Nappi02.Text == string.Empty && Nappi11.Text == "O" && Nappi20.Text == "O")
                {
                    Nappi02_Clicked(null, EventArgs.Empty);
                }
                else if (count % 2 == 0)
                {
                    int satunnaisNappi = rnd.Next(0, 9);

                    if (satunnaisNappi == 0)
                    {
                        Nappi00_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 1)
                    {
                        Nappi01_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 2)
                    {
                        Nappi02_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 3)
                    {
                        Nappi10_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 4)

                    {
                        Nappi11_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 5)

                    {
                        Nappi12_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 6)
                    {
                        Nappi20_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 7)
                    {
                        Nappi21_Clicked(null, EventArgs.Empty);

                    }
                    else if (satunnaisNappi == 8)
                    {
                        Nappi22_Clicked(null, EventArgs.Empty);

                    }
                }
            }

        }
        // arpoo satunnaisen pitusen "miettimis ajan" tekoälylle
        public async void MietintaAika()
        {
            tauko = rnd.Next(50, 500);

        }
    }
}

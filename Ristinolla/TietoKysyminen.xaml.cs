using CommunityToolkit.Maui.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Ristinolla;

public partial class TietoKysyminen : ContentPage

{
    public ObservableCollection<Pelaajien_tiedotcs> PelaajaLista2 = new ObservableCollection<Pelaajien_tiedotcs>();
    public int TiedotKunnossaP1 = 0;
    public int TiedotKunnossaP2 = 0;


    public static string Etunimi1;
    public static string Sukunimi1;
    public static string SyntymaAika1;

    public static string Etunimi2;
    public static string Sukunimi2;
    public static string SyntymaAika2;

    public static string Teko‰lynMerkki;


    List<Pelaajien_tiedotcs> TietoLista;


    Pelaajien_tiedotcs tiedot = new();
    public TietoKysyminen()
    {
        BindingContext = this;
        InitializeComponent();
        TiedotListaan();
        LoadTiedotAsync();
        ListaKurkkaaja2.ItemsSource = PelaajaLista2;
        ListaKurkkaaja1.ItemsSource = PelaajaLista2;

        if (AloitusSivu.teko‰lypeli == 1)
        {
            // vaihtaa ohjelman ulkoasua, jotta pelaaja voi valita vain yhden pelaajan jos pelaa teko‰ly‰ vastaan
            Etunimi2 = "Teko";
            Sukunimi2 = "ƒly";
            SyntymaAika2 = "2024";
            P2Tallentaja_Clicked(null, EventArgs.Empty);
            ListaKurkkaaja2.IsEnabled = false;
            ListaKurkkaaja2.Opacity = 0;

            Pelaaja2TiedotTeksti.Opacity = 0;

            Pelaaja2EtuNimi.IsEnabled = false;
            Pelaaja2EtuNimi.Opacity = 0;
            Pelaaja2EtuNimiTeksti.Opacity = 0;

            Pelaaja2SukuNimi.IsEnabled = false;
            Pelaaja2SukuNimi.Opacity = 0;
            Pelaaja2SukuNimiTeksti.Opacity = 0;

            Pelaaja2Syntym‰aika.IsEnabled = false;
            Pelaaja2Syntym‰aika.Opacity = 0;
            Pelaaja2Syntym‰aikateksti.Opacity = 0;

            P2Tallentaja.IsEnabled = false;
            P2Tallentaja.Opacity = 0;
            Teko‰lynMerkki = "X";

            P1Tallentaja.Text = "Haluan Pelata X:ll‰";
            OnValitsija.IsEnabled = true;
            OnValitsija.Opacity = 1;



        }
    }
    public async void SeuraavaSivu()
    {
        // jos molempien pelaajien tiedot on tallennettu, niin menee seuraavalle sivulle
        if (TiedotKunnossaP1 == 1 && TiedotKunnossaP2 == 1)
        {

            await Navigation.PushAsync(new MainPage());

        }
    }



    // talleentaa pelaaja 1 tiedot
    public async void P1Tallentaja_Clicked(object sender, EventArgs e)
    {
        if (AloitusSivu.teko‰lypeli == 1)
        {
            Teko‰lynMerkki = "O";
        }

        if (Pelaaja1EtuNimi!= null && Pelaaja1SukuNimi != null && Pelaaja1Syntym‰aika != null)
        {
            // muuntaa tiedot entryst‰ strucktiin
            Etunimi1 = Pelaaja1EtuNimi.Text;
            Sukunimi1 = Pelaaja1SukuNimi.Text;
            SyntymaAika1 = Pelaaja1Syntym‰aika.Text;
            tiedot.Etunimi = Etunimi1;
            tiedot.Sukunimi = Sukunimi1;
            tiedot.Syntymavuosi = SyntymaAika1;
            // ja tallentaa ne json tiedostoon
            string jsonString = JsonSerializer.Serialize(tiedot);


            if (File.Exists(MainPage.TallennusOsoite))
            {
                string OlevatJsonTiedot = await File.ReadAllTextAsync(MainPage.TallennusOsoite);

                try
                {
                    TietoLista = JsonSerializer.Deserialize<List<Pelaajien_tiedotcs>>(OlevatJsonTiedot) ?? new List<Pelaajien_tiedotcs>();
                }
                // jos tiedosto on olemassa, mutta tyhj‰
                catch (System.Text.Json.JsonException)
                {

                    TietoLista = new List<Pelaajien_tiedotcs>();
                }
            }
            else
            {

                TietoLista = new List<Pelaajien_tiedotcs>();
            }



            OlemassaOlevanTarkistaja(Etunimi1, Sukunimi1, SyntymaAika1);
            string UUdetJaVanhatTiedot = JsonSerializer.Serialize(TietoLista, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(MainPage.TallennusOsoite, UUdetJaVanhatTiedot);
            TiedotKunnossaP1 = 1;
            
            await DisplayAlert("Tallennettu", "Pelaaja 1 tiedot Tallennettu", "ok");
            TietoLista.Clear();
            SeuraavaSivu();
        }

    }

    // tallentaa pelaaja 2 tiedot
    public async void P2Tallentaja_Clicked(object sender, EventArgs e)
    {
        if (AloitusSivu.teko‰lypeli == 1)
        {
            // teko‰lypeliss‰ etunimi m‰‰ritell‰‰n muualla ja t‰m‰ laittaa ne structiin
            tiedot.Etunimi = Etunimi2;
            tiedot.Sukunimi = Sukunimi2;
            tiedot.Syntymavuosi = SyntymaAika2;
        }
        else
        {
            // muuntaa tiedot entryst‰ strucktiin
            Etunimi2 = Pelaaja2EtuNimi.Text;
            Sukunimi2 = Pelaaja2SukuNimi.Text;
            SyntymaAika2 = Pelaaja2Syntym‰aika.Text;
            tiedot.Etunimi = Etunimi2;
            tiedot.Sukunimi = Sukunimi2;
            tiedot.Syntymavuosi = SyntymaAika2;
        }

        if (File.Exists(MainPage.TallennusOsoite))
        {
            string OlevatJsonTiedot = await File.ReadAllTextAsync(MainPage.TallennusOsoite);

            try
            {
                TietoLista = JsonSerializer.Deserialize<List<Pelaajien_tiedotcs>>(OlevatJsonTiedot) ?? new List<Pelaajien_tiedotcs>();
            }
            // jos tiedosto on olemassa, mutta tyhj‰
            catch (System.Text.Json.JsonException)
            {

                TietoLista = new List<Pelaajien_tiedotcs>();
            }
        }
        else
        {
            TietoLista = new List<Pelaajien_tiedotcs>();
        }

        OlemassaOlevanTarkistaja(Etunimi2, Sukunimi2, SyntymaAika2);
        string UUdetJaVanhatTiedot = JsonSerializer.Serialize(TietoLista, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(MainPage.TallennusOsoite, UUdetJaVanhatTiedot);

        TiedotKunnossaP2 = 1;
        TietoLista.Clear();
        if (AloitusSivu.teko‰lypeli != 1)
        {
            await DisplayAlert("Tallennettu", "Pelaaja 2 tiedot Tallennettu", "ok");
        }
        SeuraavaSivu();

    }
    // vie tiedot listaan 
    public async Task<List<Pelaajien_tiedotcs>> TiedotListaan()
    {
        if (File.Exists(MainPage.TallennusOsoite)) 
        {
            try
            {
                string json = await File.ReadAllTextAsync(MainPage.TallennusOsoite);
                return JsonSerializer.Deserialize<List<Pelaajien_tiedotcs>>(json) ?? new List<Pelaajien_tiedotcs>();
            }
            catch (System.Text.Json.JsonException)
            {

                return new List<Pelaajien_tiedotcs>();
            }
        }
        return new List<Pelaajien_tiedotcs>();
    }
    private async void LoadTiedotAsync()
    {
        // Get data from TiedotListaan and populate the ObservableCollection
        var tiedot = await TiedotListaan();
        foreach (var tieto in tiedot)
        {
            Pelaajien_tiedotcs arvo = tieto;
            PelaajaLista2.Add(arvo);
        }
    }
    // laitaa k‰ytt‰j‰n painaman listan kohdan pelaaja 1 tiedojen entryyn
    private async void ListaKurkkaaja1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        if (e.SelectedItem is Pelaajien_tiedotcs selectedItem)
        {

            Sukunimi1 = selectedItem.Sukunimi.ToString();
            Pelaaja1SukuNimi.Text = Sukunimi1;
            Etunimi1 = selectedItem.Etunimi.ToString();
            Pelaaja1EtuNimi.Text = Etunimi1;
            SyntymaAika1 = selectedItem.Syntymavuosi.ToString();
            Pelaaja1Syntym‰aika.Text = SyntymaAika1;

            await DisplayAlert("Tallennettu", "Pelaaja 1 tiedot Tallennettu", "ok");
            if (AloitusSivu.teko‰lypeli != 1)
            {
                TiedotKunnossaP1 = 1;
            }
            else
            {

            }
            SeuraavaSivu();
        }

    }
    // laitaa k‰ytt‰j‰n painaman listan kohdan pelaaja 2 tiedojen entryyn
    private async void ListaKurkkaaja2_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        if (e.SelectedItem is Pelaajien_tiedotcs selectedItem)
        {

            Sukunimi2 = selectedItem.Sukunimi.ToString();
            Pelaaja2SukuNimi.Text = Sukunimi2;
            Etunimi2 = selectedItem.Etunimi.ToString();
            Pelaaja2EtuNimi.Text = Etunimi2;
            SyntymaAika2 = selectedItem.Syntymavuosi.ToString();
            Pelaaja2Syntym‰aika.Text = SyntymaAika2;
            await DisplayAlert("Tallennettu", "Pelaaja 2 tiedot Tallennettu", "ok");

            TiedotKunnossaP2 = 1;
            SeuraavaSivu();
        }
    }
    // jos pelaaja valitsee O:n pelimerkikseen teko‰ly‰ vastaan, niin tallennaa tiedot
    private async void OnValitsija_Clicked(object sender, EventArgs e)
    {
        Teko‰lynMerkki = "X";
        if (Pelaaja1EtuNimi.ToString() != null && Pelaaja1SukuNimi != null && Pelaaja1Syntym‰aika != null)
        {

            Etunimi1 = Pelaaja1EtuNimi.Text;
            Sukunimi1 = Pelaaja1SukuNimi.Text;
            SyntymaAika1 = Pelaaja1Syntym‰aika.Text;
            tiedot.Etunimi = Etunimi1;
            tiedot.Sukunimi = Sukunimi1;
            tiedot.Syntymavuosi = SyntymaAika1;
            string jsonString = JsonSerializer.Serialize(tiedot);


            if (File.Exists(MainPage.TallennusOsoite))
            {
                string OlevatJsonTiedot = await File.ReadAllTextAsync(MainPage.TallennusOsoite);

                try
                {
                    TietoLista = JsonSerializer.Deserialize<List<Pelaajien_tiedotcs>>(OlevatJsonTiedot) ?? new List<Pelaajien_tiedotcs>();
                }
                // jos tiedosto on olemassa, mutta tyhj‰
                catch (System.Text.Json.JsonException)
                {

                    TietoLista = new List<Pelaajien_tiedotcs>();
                }
            }
            else
            {

                TietoLista = new List<Pelaajien_tiedotcs>();
            }

            OlemassaOlevanTarkistaja(Etunimi1,Sukunimi1,SyntymaAika1);
            string UUdetJaVanhatTiedot = JsonSerializer.Serialize(TietoLista, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(MainPage.TallennusOsoite, UUdetJaVanhatTiedot);
            TiedotKunnossaP1 = 1;
            await DisplayAlert("Tallennettu", "Pelaaja 1 tiedot Tallennettu", "ok");
            SeuraavaSivu();
        }
    }

    //tarkistaa onko Valittu ihminen jo olemassa ja jos ei ole niin lis‰‰ h‰net listaan, muuten ei lis‰‰ listaan
    private void OlemassaOlevanTarkistaja(string Enimi, string Snimi, string Saika) 
    { 
        string jsonTiedot = File.ReadAllText(MainPage.TallennusOsoite);
        try
        {
            JArray Arvot = JArray.Parse(jsonTiedot);
            bool OnOlemassa = false;

            for (int i = 0; i < Arvot.Count; i++)
            {
                var item = Arvot[i];
                if (item["Etunimi"].ToString() == Enimi.ToString() && item["Sukunimi"].ToString() == Snimi.ToString() && item["Syntymavuosi"].ToString() == Saika.ToString())
                {
                    OnOlemassa = true;

                }
            }
            if (OnOlemassa == false)
            {
                TietoLista.Add(tiedot);
            }
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            TietoLista.Add(tiedot);
        }
        
    }

}




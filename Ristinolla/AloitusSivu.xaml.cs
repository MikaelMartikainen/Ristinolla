namespace Ristinolla;

public partial class AloitusSivu : ContentPage
{
    public static int teko‰lypeli = 0;
	public AloitusSivu()
	{
		InitializeComponent();

	}

    private async void yksiPeli_Clicked(object sender, EventArgs e)
    {
        // menee tietojen kysyimen sivulle, mutta t‰ss‰ laittaa teko‰ly pelin p‰‰lle
        teko‰lypeli = 1;
        await Navigation.PushAsync(new TietoKysyminen());
        //await Navigation.PushAsync(new AIRistinollacs());
        
    }

    private async void kaksiPeli_Clicked(object sender, EventArgs e)
    {
        // menee tietojen kysyimen sivulle
        await Navigation.PushAsync(new TietoKysyminen());
    }


}
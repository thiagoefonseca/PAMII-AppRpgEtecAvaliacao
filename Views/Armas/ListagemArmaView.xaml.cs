using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas;

public partial class ListagemArmaView : ContentPage
{
	ListagemArmaViewModel viewModel;
	public ListagemArmaView()
	{
		InitializeComponent();

        viewModel = new ListagemArmaViewModel();
        BindingContext = viewModel;
        Title = "Armas = App Rpg Etec";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.ObterArmas();
    }
}
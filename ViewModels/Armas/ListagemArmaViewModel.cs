using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;

//teste

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmaViewModel : BaseViewModel
    {
        private ArmaService aService;

        public ObservableCollection<Arma> Armas { get; set; }

        public ListagemArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            Armas = new ObservableCollection<Arma>();

            NovaArmaCommand = new Command(async () => { await ExibirCadastroArma(); });
            RemoverArmaCommand =
               new Command<Arma>(async (Arma a) => { await RemoverArma(a); });

            _ = ObterArmas();
        }

        public ICommand NovaArmaCommand { get; }
        public ICommand RemoverArmaCommand { get; set; }

        public async Task ObterArmas()
        {
            try // Junto com o Cacth evitará que erros fechem o aplicativo
            {
                Armas = await aService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas));  // Informara a view que houve carregamento
            }
            catch (Exception ex)
            {
                // Captara o erro para exibir em tela 
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Detalhes" + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroArma()
        {
            try
            {
                await Shell.Current.GoToAsync("cadArmaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Arma armaSelecionada;//CTRL + R,E
        public Arma ArmaSelecionada
        {
            get { return armaSelecionada; }
            set
            {
                if (value != null)
                {
                    armaSelecionada = value;



                    Shell.Current
                        .GoToAsync($"cadArmaView?pId={armaSelecionada.Id}");
                }
            }
        }

        public async Task RemoverArma(Arma a)
        {
            try
            {
                if (await Application.Current.MainPage
                        .DisplayAlert("Confirmação", $"Confirma a remoção de {a.Nome}?", "Sim", "Não"))
                {
                    await aService.DeleteArmaAsync(a.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                        "Arma removida com sucesso!", "Ok");

                    _ = ObterArmas();
                }
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}

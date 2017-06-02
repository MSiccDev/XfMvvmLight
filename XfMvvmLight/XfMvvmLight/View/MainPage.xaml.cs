using Xamarin.Forms;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = ViewModelLocator.Instance.MainVm;
        }
    }
}

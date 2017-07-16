using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using XfMvvmLight.BaseControls;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalPage : XfNavContentPage
    {


        public ModalPage()
        {
            InitializeComponent();

        }

        //demo hack only!
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


    }
}
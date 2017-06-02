using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;
using GalaSoft.MvvmLight.Ioc;

namespace XfMvvmLight.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public string HelloWorldString { get; private set; } = "Hello from Xamarin Forms with MVVM Light";


        private string _osVersionViaDs;
        public string OsVersionViaDs
        {
            get { return _osVersionViaDs; }
            set { Set(ref _osVersionViaDs, value); }
        }


        private RelayCommand _getOSVersionViaDsCommand;

        public RelayCommand GetOsVersionViaDsCommand => _getOSVersionViaDsCommand ?? (_getOSVersionViaDsCommand = new RelayCommand(() =>
        {
            OsVersionViaDs = DependencyService.Get<IOsVersionService>().GetOsVersion;

        }));




        private string _osVersionViaSimpleIoc;
        public string OsVersionViaSimpleIoc
        {
            get { return _osVersionViaSimpleIoc; }
            set { Set(ref _osVersionViaSimpleIoc, value); }
        }


        private RelayCommand _getOSVersionViaSimpleIocCommand;

        public RelayCommand GetOsVersionViaSimpleIocCommand => _getOSVersionViaSimpleIocCommand ?? (_getOSVersionViaSimpleIocCommand = new RelayCommand(() =>
        {
            OsVersionViaSimpleIoc = SimpleIoc.Default.GetInstance<IOsVersionService>().GetOsVersion;

        }));

    }
}

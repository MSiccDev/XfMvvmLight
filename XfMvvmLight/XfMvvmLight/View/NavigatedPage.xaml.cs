using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XfMvvmLight.Abstractions;
using XfMvvmLight.BaseControls;
using XfMvvmLight.ViewModel;

namespace XfMvvmLight.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigatedPage : XfNavContentPage
    {

        public NavigatedPage()
        {
            InitializeComponent();

        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XfMvvmLight.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CommandChainingDemoControl : ContentView
	{
        public CommandChainingDemoControl()
        {
            InitializeComponent();

            this.DemoCommand = new Command(() =>
            {
                FillFromBehind();
            });
        }


        private void FillFromBehind()
        {
            this.LabelFilledFromBehind.Text = "Text was empty, but we used command chaining to show this text inside a control.";
        }


        public static BindableProperty DemoCommandProperty = BindableProperty.Create(nameof(DemoCommand), typeof(ICommand), typeof(CommandChainingDemoControl), null, BindingMode.OneWayToSource);

        public ICommand DemoCommand
        {
            get => (ICommand)GetValue(DemoCommandProperty);
            set => SetValue(DemoCommandProperty, value);
        }

    }
}
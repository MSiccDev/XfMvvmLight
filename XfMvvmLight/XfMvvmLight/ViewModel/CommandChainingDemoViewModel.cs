using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace XfMvvmLight.ViewModel
{
    public class CommandChainingDemoViewModel : XfNavViewModelBase
    {
        private ICommand _invokeDemoCommand;
        private RelayCommand _demo1Command;

        public CommandChainingDemoViewModel()
        {
        }

        public ICommand InvokeDemoCommand { get => _invokeDemoCommand; set => Set(ref _invokeDemoCommand, value); }

        public RelayCommand Demo1Command => _demo1Command ?? (_demo1Command = new RelayCommand(() =>
        {
            this.InvokeDemoCommand?.Execute(null);
        }));
    }
}

using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ViewModel
{
    public class ActionSheetDemoViewModel : XfNavViewModelBase
    {
        private readonly IActionSheetService _actionSheetService;
        private RelayCommand<string> _showActionSheetCommand;

        public ActionSheetDemoViewModel(IActionSheetService actionSheetService)
        {
            _actionSheetService = actionSheetService;
        }

        public override void ExecuteViewDisappearingCommand()
        {
            this.ActionSheetResult = null;
            RaisePropertyChanged(() => this.ActionSheetResult);

            base.ExecuteViewDisappearingCommand();
        }

        public string ActionSheetResult { get; set; }

        public RelayCommand<string> ShowActionSheetCommand => _showActionSheetCommand ?? (_showActionSheetCommand = new RelayCommand<string>(async o =>
        {
            string result = null;
            switch (Convert.ToInt32(o))
            {
                case 1:
                    result = await _actionSheetService.ShowActionSheetAsync("Simple Action Sheet", new string[] { "Option 1", "Option 2" });
                    break;
                case 2:
                    result = await _actionSheetService.ShowActionSheetAsync("Action Sheet With Cancel", "Cancel", new string[] { "Option 1", "Option 2" });
                    break;
                case 3:
                    result = await _actionSheetService.ShowActionSheetAsync("Action Sheet With Cancel And Destroy", "Cancel", "Navigate Back", new string[] { "Option 1", "Option 2" });
                    break;
            }

            if (result == "Navigate Back")
            {
                this.ActionSheetResult = "Navigating back in 3 seconds...";
                await Task.Delay(3000);

                await NavService.GoHomeAsync();
            }
            else
            {
                this.ActionSheetResult = result;
            }

            RaisePropertyChanged(() => this.ActionSheetResult);
        }));
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ServiceImplementations
{
    public class XfNavigationService : IXfNavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private NavigationPage _navigationPage;


        //not using constructor injection here because in XF timing is always a problem
        public void Initialize(NavigationPage navigationPage)
        {
            _navigationPage = navigationPage;
        }









        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public async Task GoHomeAsync()
        {
            await _navigationPage.PopToRootAsync(true);
        }



        public (bool isRegistered, bool isModal) StackContainsNavKey(string pageKey)
        {
            bool isRegistered = _pagesByKey.ContainsKey(pageKey);
            bool isUsedModal = false;

            if (isRegistered)
            {
                var pageType = _pagesByKey.SingleOrDefault(p => p.Key == pageKey).Value;

                var foundInNavStack = _navigationPage.Navigation.NavigationStack.Any(p => p.GetType() == pageType);
                var foundInModalStack = _navigationPage.Navigation.ModalStack.Any(p => p.GetType() == pageType);

                if (foundInNavStack && !foundInModalStack || !foundInNavStack && !foundInModalStack)
                {
                    isUsedModal = false;
                }
                else if (foundInModalStack && !foundInNavStack)
                {
                    isUsedModal = true;
                }
                else
                {
                    throw new NotSupportedException("Pages should be used exclusively Modal or for Navigation");
                }
            }

            return (isRegistered, isUsedModal);
        }




        #region modal
        public int ModalStackCount => _navigationPage.Navigation.ModalStack.Count;

        public string CurrentModalPageKey
        {
            get
            {
                {
                    lock (_pagesByKey)
                    {
                        if (ModalStackCount == 1)
                        {
                            return null;
                        }

                        //only INavigation holds the ModalStack
                        var pageType = _navigationPage.Navigation.ModalStack.Last().GetType();

                        return _pagesByKey.ContainsValue(pageType) ? _pagesByKey.FirstOrDefault(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        public async Task GoBackModalAsync()
        {
            await _navigationPage.Navigation.PopModalAsync(true);
        }

        public async Task ShowModalPageAsync(string pageKey, bool animated = true)
        {
            await ShowModalPageAsync(pageKey, null);
        }

        public async Task ShowModalPageAsync(string pageKey, object parameter, bool animated = true)
        {
            if (_pagesByKey.ContainsKey(pageKey))
            {
                var type = _pagesByKey[pageKey];
                ConstructorInfo constructor = null;
                object[] parameters = null;

                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                    {
                    };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                return c.GetParameters().Count() == 1
                                       && c.GetParameters()[0].ParameterType == parameter.GetType();
                            });

                    parameters = new[] { parameter };
                }


                var page = constructor.Invoke(parameters) as Page;

                await _navigationPage.Navigation.PushModalAsync(page, animated);
            }
            else
            {
                throw new ArgumentException($"No page found with key: {pageKey}. Did you forget to call the Configure method?", nameof(pageKey));
            }
        }

        #endregion


        #region navigation

        public int NavigationStackCount => _navigationPage.Navigation.NavigationStack.Count;

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigationPage?.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = _navigationPage.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public Page GetCurrentPage()
        {
            return _navigationPage?.CurrentPage;
        }

        public async Task GoBackAsync()
        {
            await _navigationPage.Navigation.PopAsync(true);
        }

        public async Task NavigateToAsync(string pageKey, bool animated = true)
        {
            await NavigateToAsync(pageKey, null);
        }

        public async Task NavigateToAsync(string pageKey, object parameter, bool animated = true)
        {
            if (_pagesByKey.ContainsKey(pageKey))
            {
                var type = _pagesByKey[pageKey];
                ConstructorInfo constructor = null;
                object[] parameters = null;

                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                    {
                    };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                return c.GetParameters().Count() == 1
                                       && c.GetParameters()[0].ParameterType == parameter.GetType();
                            });

                    parameters = new[] { parameter };
                }

                if (constructor == null)
                {
                    throw new InvalidOperationException("No constructor found for page " + pageKey);
                }

                var page = constructor.Invoke(parameters) as Page;
                if (_navigationPage != null)
                {
                    await _navigationPage.Navigation.PushAsync(page, animated);
                }
                else
                {
                    //todo:
                    throw new NullReferenceException("there is no navigation page present, please check your page architecture and make sure you have called the Initialize Method before.");
                }
            }
            else
            {
                throw new ArgumentException(
                    $"No page with key: {pageKey}. Did you forget to call the Configure method?",
                    nameof(pageKey));
            }

        }

        #endregion






    }
}

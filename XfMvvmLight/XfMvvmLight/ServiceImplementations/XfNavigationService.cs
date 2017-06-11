using System;
using System.Collections.Generic;
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
        private NavigationPage _navigation;


        //not using constructor injection here because in XF timing is always a problem
        public void Initialize(NavigationPage navigation)
        {
            _navigation = navigation;
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
            await _navigation.PopToRootAsync(true);
        }


        #region modal
        public int ModalStackCount => Application.Current.MainPage.Navigation.ModalStack.Count;

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

                        //no need to keep a second stack floating arround 
                        //every XF App has a MainPage, which has the ModalStack in its Navigation property
                        //pulling it into here is enough to achieve the functionality that is needed
                        var pageType = Application.Current.MainPage.Navigation.ModalStack.Last().GetType();

                        return _pagesByKey.ContainsValue(pageType)
                            ? _pagesByKey.FirstOrDefault(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        public async Task GoBackModalAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
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

                //showing modals on application level only
                await Application.Current.MainPage.Navigation.PushModalAsync(page, animated);
            }
            else
            {
                throw new ArgumentException($"No page found with key: {pageKey}. Did you forget to call the Configure method?", nameof(pageKey));
            }
        }

        #endregion


        #region navigation
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation?.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = _navigation.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public Page GetCurrentPage()
        {
            return _navigation?.CurrentPage;
        }

        public async Task GoBackAsync()
        {
            await _navigation.PopAsync(true);
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
                await _navigation.PushAsync(page, animated);
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

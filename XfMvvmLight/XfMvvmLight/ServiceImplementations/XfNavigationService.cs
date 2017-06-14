using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XfMvvmLight.Abstractions;

namespace XfMvvmLight.ServiceImplementations
{
    public class XfNavigationService : IXfNavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private NavigationPage _navigationPage;

        //using this instead of lock statement
        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);


        //not using constructor injection here because in XF timing is always a problem
        public void Initialize(NavigationPage navigationPage)
        {
            _navigationPage = navigationPage;
        }



        public void Configure(string pageKey, Type pageType)
        {
            //synchronous lock
            _lock.Wait();
            try
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
            finally
            {
                _lock.Release();
            }
        }

        public async Task GoHomeAsync()
        {
            await _navigationPage.PopToRootAsync(true);
        }



        public (bool isRegistered, bool isModal) StackContainsNavKey(string pageKey)
        {

            bool isUsedModal = false;
            bool isRegistered = false;

            _lock.Wait();
            try
            {
                isRegistered = _pagesByKey.ContainsKey(pageKey);


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
                else
                {
                    throw new ArgumentException($"No page with key: {pageKey}. Did you forget to call the Configure method?", nameof(pageKey));
                }
            }
            finally
            {
                _lock.Release();
            }

            return (isRegistered, isUsedModal);
        }

        public Page GetCurrentPage()
        {
            return _navigationPage?.CurrentPage;
        }



        #region modal
        public int ModalStackCount => _navigationPage.Navigation.ModalStack.Count;

        public string CurrentModalPageKey
        {
            get
            {
                _lock.Wait();

                try
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
                finally
                {
                    _lock.Release();
                }
            }
        }

        public async Task GoBackModalAsync()
        {
            await _navigationPage.Navigation.PopModalAsync(true);
        }

        public async Task ShowModalPageAsync(string pageKey, bool animated = true)
        {
            await ShowModalPageAsync(pageKey, null, animated);
        }

        public async Task ShowModalPageAsync(string pageKey, object parameter, bool animated = true)
        {
            await _lock.WaitAsync();

            try
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

                    if (_navigationPage != null)
                    {
                        await _navigationPage.Navigation.PushModalAsync(page, animated);
                    }
                    else
                    {
                        throw new NullReferenceException("there is no navigation page present, please check your page architecture and make sure you have called the Initialize Method before.");
                    }
                }
                else
                {
                    throw new ArgumentException($"No page found with key: {pageKey}. Did you forget to call the Configure method?", nameof(pageKey));
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        #endregion


        #region navigation

        public int NavigationStackCount => _navigationPage.Navigation.NavigationStack.Count;

        public string CurrentPageKey
        {
            get
            {
                _lock.Wait();
                try
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
                finally
                {
                    _lock.Release();
                }
            }
        }



        public async Task GoBackAsync()
        {
            await _navigationPage.Navigation.PopAsync(true);
        }

        public async Task NavigateToAsync(string pageKey, bool animated = true)
        {
            await NavigateToAsync(pageKey, null, animated);
        }

        public async Task NavigateToAsync(string pageKey, object parameter, bool animated = true)
        {
            await _lock.WaitAsync();

            try
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
            finally
            {
                _lock.Release();
            }
        }

        #endregion






    }
}

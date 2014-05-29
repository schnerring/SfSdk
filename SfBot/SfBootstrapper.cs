using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using SfBot.Data;
using SfBot.Shell;
using SfBot.ViewModels;
using SFBot.ViewModels;
using SfBot.ViewModels.Details;
using SFBot.ViewModels.Details;

namespace SfBot
{
    public class SfBootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public SfBootstrapper()
        {
            base.StartRuntime();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            
            _container.Singleton<IShell, ShellViewModel>();

            _container.Singleton<ConfigurationStore>();

            _container.Singleton<CreateAccountViewModel>();
            _container.Singleton<AccountsViewModel>();
            _container.Singleton<SessionsViewModel>();
            _container.Singleton<FooterViewModel>();
            _container.Singleton<LoggedOutViewModel>();

            _container.PerRequest<DetailsViewModel>();
            _container.PerRequest<CharacterViewModel>();
            _container.PerRequest<HallOfFameCrawlerViewModel>();
            _container.PerRequest<ScrapbookViewModel>();
            _container.PerRequest<ScrapbookItemViewModel>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var result = _container.GetInstance(serviceType, key);

            if (result != null)
                return result;

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", serviceType));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Caliburn.Micro;
using SFBot.ViewModels;
using SfBot.Data;
using SfBot.Shell;
using SFBot.ViewModels.Details;
using SfSdk.Contracts;

namespace SfBot
{
    public class SfBootstrapper : Bootstrapper<IShell>
    {
        private CompositionContainer _container;

        protected override void Configure()
        {
            _container =
                new CompositionContainer(
                    new AggregateCatalog(
                        AssemblySource.Instance.Select(x => new AssemblyCatalog(x))));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<Func<Account, DetailsViewModel>>(a =>
            {
                var vm = IoC.Get<DetailsViewModel>();
                vm.Init(a);
                return vm;
            });
            batch.AddExportedValue<Func<IEnumerable<IScrapbookItem>, ScrapbookItemViewModel>>(i =>
            {
                var vm = IoC.Get<ScrapbookItemViewModel>();
                vm.Init(i);
                return vm;
            });

            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var result = _container.GetExportedValues<object>(contract).FirstOrDefault();

            if (result != null)
                return result;

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }
    }
}
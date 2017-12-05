using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;

namespace FriendOrganizer.UI.Startup
{
    /// <summary>
    ///     Responsible for creating AutoFac container.
    /// </summary>
    public class Bootstrapper
    {
        #region Public Methods and Operators

        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            // Container knows when IFriendRepository is required somewhere; it creates instance of FriendDataService class then
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            //builder.RegisterType<LookupDataService>().As<IFriendLookupDataService>();
            // AsImplementedInterfaces -> with this we can have two interfaces for one file.
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            return builder.Build();
        }

        #endregion
    }
}
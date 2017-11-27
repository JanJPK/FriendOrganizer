using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;

namespace FriendOrganizer.UI.Startup
{
    /// <summary>
    ///     Responsible for creating AutoFac container.
    /// </summary>
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            // Container knows when IFriendDataService is required somewhere; it creates instance of FriendDataService class then
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();
            //builder.RegisterType<LookupDataService>().As<IFriendLookupDataService>();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            return builder.Build();
        }
    }
}
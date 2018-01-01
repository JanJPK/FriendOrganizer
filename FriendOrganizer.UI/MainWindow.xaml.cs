using System.Globalization;
using System.Threading;
using System.Windows;
using FriendOrganizer.UI.ViewModel;
using MahApps.Metro.Controls;

namespace FriendOrganizer.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Fields

        private readonly MainViewModel viewModel;

        #endregion

        #region Constructors and Destructors

        // Taking parameter -> changes required in App.xaml.
        public MainWindow(MainViewModel viewModel)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            Loaded += MainWindow_Loaded;
        }

        #endregion

        #region Methods

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadAsync();
        }

        #endregion
    }
}
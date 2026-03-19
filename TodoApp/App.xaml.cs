using System.Windows;
using TodoApp.AppCore.UseCases;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Presentation.ViewModel;

namespace TodoApp
{
    /// <summary>
    /// Composition Root: 모든 의존성을 여기서 조립 (수동 DI).
    /// Infrastructure → UseCase → ViewModel → Window.
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Infrastructure
            var repository = new JsonTodoRepository();

            // UseCases
            var createUseCase = new CreateTodoUseCase(repository);
            var loadUseCase = new LoadTodosUseCase(repository);
            var updateUseCase = new UpdateTodoUseCase(repository);
            var deleteUseCase = new DeleteTodoUseCase(repository);
            var toggleUseCase = new ToggleTodoStatusUseCase(repository);

            // ViewModel
            var mainViewModel = new MainViewModel(
                createUseCase, loadUseCase, updateUseCase, deleteUseCase, toggleUseCase);

            // View
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }
}

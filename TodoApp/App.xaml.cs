using System.Windows;
using TodoApp.AppCore.UseCases;
using TodoApp.Infrastructure.Api;
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
        // TodoServer 주소 (서버 실행 후 포트 확인)
        private const string ServerUrl = "http://localhost:5000";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Infrastructure: API 서버 연결 (DB 저장)
            var repository = new ApiTodoRepository(ServerUrl);

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

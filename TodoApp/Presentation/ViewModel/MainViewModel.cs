using System.Windows.Input;
using TodoApp.AppCore.UseCases;
using TodoApp.Presentation.Commands;

namespace TodoApp.Presentation.ViewModel
{
    /// <summary>
    /// 앱 전체를 조율하는 최상위 ViewModel.
    /// UseCase들을 주입받아 커맨드 실행 → 결과에 따른 UI 상태 갱신.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly CreateTodoUseCase _createUseCase;
        private readonly LoadTodosUseCase _loadUseCase;
        private readonly UpdateTodoUseCase _updateUseCase;
        private readonly DeleteTodoUseCase _deleteUseCase;
        private readonly ToggleTodoStatusUseCase _toggleUseCase;
        private string _errorMessage;

        public TodoListViewModel TodoList { get; }
        public TodoEditorViewModel Editor { get; }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ToggleCommand { get; }

        public MainViewModel(
            CreateTodoUseCase createUseCase,
            LoadTodosUseCase loadUseCase,
            UpdateTodoUseCase updateUseCase,
            DeleteTodoUseCase deleteUseCase,
            ToggleTodoStatusUseCase toggleUseCase)
        {
            _createUseCase = createUseCase;
            _loadUseCase = loadUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
            _toggleUseCase = toggleUseCase;

            TodoList = new TodoListViewModel();
            Editor = new TodoEditorViewModel();
            Editor.Clear();

            AddCommand = new RelayCommand(_ => ExecuteAdd(), _ => !Editor.IsEditMode);
            UpdateCommand = new RelayCommand(_ => ExecuteUpdate(), _ => Editor.IsEditMode);
            DeleteCommand = new RelayCommand(_ => ExecuteDelete(), _ => TodoList.SelectedItem != null);
            EditCommand = new RelayCommand(_ => ExecuteEdit(), _ => TodoList.SelectedItem != null);
            CancelCommand = new RelayCommand(_ => ExecuteCancel());
            ToggleCommand = new RelayCommand(p => ExecuteToggle(p as TodoItemViewModel));

            LoadTodos();
        }

        private void LoadTodos()
        {
            var result = _loadUseCase.Execute();
            if (result.IsSuccess)
            {
                TodoList.AllItems.Clear();
                foreach (var task in result.Data)
                    TodoList.AllItems.Add(new TodoItemViewModel(task));
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        private void ExecuteAdd()
        {
            var result = _createUseCase.Execute(
                Editor.Title, Editor.Description, Editor.SelectedPriority, Editor.DueDate);

            if (result.IsSuccess)
            {
                TodoList.AllItems.Add(new TodoItemViewModel(result.Data));
                Editor.Clear();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        private void ExecuteUpdate()
        {
            if (!Editor.EditingTaskId.HasValue)
                return;

            var result = _updateUseCase.Execute(
                Editor.EditingTaskId.Value, Editor.Title, Editor.Description,
                Editor.SelectedPriority, Editor.DueDate);

            if (result.IsSuccess)
            {
                Editor.Clear();
                LoadTodos();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        private void ExecuteDelete()
        {
            if (TodoList.SelectedItem == null)
                return;

            var result = _deleteUseCase.Execute(TodoList.SelectedItem.Id);
            if (result.IsSuccess)
            {
                TodoList.AllItems.Remove(TodoList.SelectedItem);
                Editor.Clear();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        private void ExecuteEdit()
        {
            if (TodoList.SelectedItem == null)
                return;

            Editor.LoadFrom(TodoList.SelectedItem);
        }

        private void ExecuteCancel()
        {
            Editor.Clear();
            ErrorMessage = null;
        }

        private void ExecuteToggle(TodoItemViewModel item)
        {
            if (item == null)
                return;

            var result = _toggleUseCase.Execute(item.Id);
            if (result.IsSuccess)
            {
                item.IsCompleted = !item.IsCompleted;
                TodoList.FilteredItems.Refresh();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using TodoApp.Presentation.Commands;

namespace TodoApp.Presentation.ViewModel
{
    /// <summary>
    /// 할일 목록 영역 ViewModel.
    /// ICollectionView.Filter로 완료/미완료 필터링 처리.
    /// </summary>
    public class TodoListViewModel : ViewModelBase
    {
        private TodoItemViewModel _selectedItem;
        private string _currentFilter = "All";

        public ObservableCollection<TodoItemViewModel> AllItems { get; }
            = new ObservableCollection<TodoItemViewModel>();

        /// <summary>
        /// 필터링된 뷰. ListBox의 ItemsSource에 바인딩.
        /// </summary>
        public ICollectionView FilteredItems { get; }

        public TodoItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public string CurrentFilter
        {
            get { return _currentFilter; }
            set
            {
                if (SetProperty(ref _currentFilter, value))
                    FilteredItems.Refresh();
            }
        }

        public ICommand SetFilterCommand { get; }

        public TodoListViewModel()
        {
            FilteredItems = CollectionViewSource.GetDefaultView(AllItems);
            FilteredItems.Filter = FilterTodos;
            SetFilterCommand = new RelayCommand(p => CurrentFilter = p?.ToString() ?? "All");
        }

        private bool FilterTodos(object obj)
        {
            var item = obj as TodoItemViewModel;
            if (item == null)
                return false;

            switch (CurrentFilter)
            {
                case "Pending":
                    return !item.IsCompleted;
                case "Completed":
                    return item.IsCompleted;
                default:
                    return true;
            }
        }
    }
}

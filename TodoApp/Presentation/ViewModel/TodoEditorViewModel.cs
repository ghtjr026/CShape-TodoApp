using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Domain.Enums;

namespace TodoApp.Presentation.ViewModel
{
    /// <summary>
    /// 할일 추가/수정 폼의 상태를 관리하는 ViewModel.
    /// IsEditMode로 추가/수정 모드 구분.
    /// </summary>
    public class TodoEditorViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private TodoPriority _selectedPriority;
        private DateTime? _dueDate;
        private bool _isEditMode;
        private Guid? _editingTaskId;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public TodoPriority SelectedPriority
        {
            get { return _selectedPriority; }
            set { SetProperty(ref _selectedPriority, value); }
        }

        public DateTime? DueDate
        {
            get { return _dueDate; }
            set { SetProperty(ref _dueDate, value); }
        }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        public Guid? EditingTaskId
        {
            get { return _editingTaskId; }
            set { SetProperty(ref _editingTaskId, value); }
        }

        /// <summary>
        /// ComboBox 아이템 소스용 우선순위 목록.
        /// </summary>
        public List<TodoPriority> PriorityOptions { get; }
            = Enum.GetValues(typeof(TodoPriority)).Cast<TodoPriority>().ToList();

        /// <summary>
        /// 폼 초기화 (추가 후 또는 취소 시).
        /// </summary>
        public void Clear()
        {
            Title = string.Empty;
            Description = string.Empty;
            SelectedPriority = TodoPriority.Medium;
            DueDate = null;
            IsEditMode = false;
            EditingTaskId = null;
        }

        /// <summary>
        /// 선택된 항목의 데이터를 폼에 채움 (수정 모드 진입).
        /// </summary>
        public void LoadFrom(TodoItemViewModel item)
        {
            Title = item.Title;
            Description = item.Description;
            SelectedPriority = item.Priority;
            DueDate = item.DueDate;
            IsEditMode = true;
            EditingTaskId = item.Id;
        }
    }
}

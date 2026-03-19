using System;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Presentation.ViewModel
{
    /// <summary>
    /// 할일 목록의 각 항목을 표현하는 ViewModel.
    /// TodoTask 엔티티를 직접 노출하지 않고 UI 전용 속성으로 래핑.
    /// </summary>
    public class TodoItemViewModel : ViewModelBase
    {
        private bool _isCompleted;

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TodoPriority Priority { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (SetProperty(ref _isCompleted, value))
                    OnPropertyChanged(nameof(IsOverdue));
            }
        }

        /// <summary>
        /// 미완료 상태이면서 마감일 초과 시 true.
        /// </summary>
        public bool IsOverdue
        {
            get { return !IsCompleted && DueDate.HasValue && DueDate.Value.Date < DateTime.Today; }
        }

        public string DueDateText
        {
            get { return DueDate.HasValue ? DueDate.Value.ToString("yyyy-MM-dd") : "마감일 없음"; }
        }

        public TodoItemViewModel(TodoTask task)
        {
            Id = task.Id;
            Title = task.Title;
            Description = task.Description;
            Priority = task.Priority;
            DueDate = task.DueDate;
            CreatedAt = task.CreatedAt;
            _isCompleted = task.Status == TodoStatus.Completed;
        }
    }
}

using System;
using System.Runtime.Serialization;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities
{
    /// <summary>
    /// 할일 핵심 엔티티. 자신의 상태 변경 규칙을 메서드로 관리하여 도메인 무결성 보장.
    /// [DataContract] 속성은 JSON 직렬화/역직렬화를 위해 사용.
    /// </summary>
    [DataContract]
    public class TodoTask
    {
        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public TodoPriority Priority { get; private set; }

        [DataMember]
        public TodoStatus Status { get; private set; }

        [DataMember]
        public DateTime? DueDate { get; private set; }

        [DataMember]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// 미완료 상태이면서 마감일이 오늘 이전인 경우 true.
        /// </summary>
        public bool IsOverdue
        {
            get { return Status == TodoStatus.Pending && DueDate.HasValue && DueDate.Value.Date < DateTime.Today; }
        }

        public TodoTask(string title, string description, TodoPriority priority, DateTime? dueDate)
        {
            Id = Guid.NewGuid();
            Title = title ?? string.Empty;
            Description = description ?? string.Empty;
            Priority = priority;
            Status = TodoStatus.Pending;
            DueDate = dueDate;
            CreatedAt = DateTime.Now;
        }

        public void MarkCompleted()
        {
            Status = TodoStatus.Completed;
        }

        public void MarkPending()
        {
            Status = TodoStatus.Pending;
        }

        public void ToggleStatus()
        {
            if (Status == TodoStatus.Completed)
                MarkPending();
            else
                MarkCompleted();
        }

        public bool UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;
            Title = title;
            return true;
        }

        public void UpdateDescription(string description)
        {
            Description = description ?? string.Empty;
        }

        public void UpdatePriority(TodoPriority priority)
        {
            Priority = priority;
        }

        public void UpdateDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }
    }
}

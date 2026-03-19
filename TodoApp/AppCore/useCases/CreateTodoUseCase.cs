using System;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 할일 추가 UseCase.
    /// 제목 검증 → 엔티티 생성 → 기존 목록에 추가 → 저장.
    /// </summary>
    public class CreateTodoUseCase
    {
        private readonly ITodoRepository _repository;

        public CreateTodoUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result<TodoTask> Execute(string title, string description, TodoPriority priority, DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Fail<TodoTask>("\uc81c\ubaa9\uc744 \uc785\ub825\ud574\uc8fc\uc138\uc694.");

            var task = new TodoTask(title.Trim(), description, priority, dueDate);
            var tasks = _repository.LoadAll();
            tasks.Add(task);
            _repository.SaveAll(tasks);

            return Result.Ok(task);
        }
    }
}

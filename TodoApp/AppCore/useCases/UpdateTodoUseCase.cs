using System;
using System.Linq;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Enums;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 할일 수정 UseCase.
    /// 전체 로드 → ID로 찾기 → 도메인 메서드로 변경 → 저장.
    /// </summary>
    public class UpdateTodoUseCase
    {
        private readonly ITodoRepository _repository;

        public UpdateTodoUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result Execute(Guid id, string title, string description, TodoPriority priority, DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Fail("제목을 입력해주세요.");

            var tasks = _repository.LoadAll();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Fail("해당 할일을 찾을 수 없습니다.");

            task.UpdateTitle(title.Trim());
            task.UpdateDescription(description);
            task.UpdatePriority(priority);
            task.UpdateDueDate(dueDate);

            _repository.SaveAll(tasks);
            return Result.Ok();
        }
    }
}

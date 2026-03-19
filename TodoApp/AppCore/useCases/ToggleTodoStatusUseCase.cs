using System;
using System.Linq;
using TodoApp.AppCore.Interfaces;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 완료/미완료 토글 UseCase.
    /// 전체 로드 → 해당 Task의 ToggleStatus() 호출 → 저장.
    /// </summary>
    public class ToggleTodoStatusUseCase
    {
        private readonly ITodoRepository _repository;

        public ToggleTodoStatusUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result Execute(Guid id)
        {
            var tasks = _repository.LoadAll();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Fail("해당 할일을 찾을 수 없습니다.");

            task.ToggleStatus();
            _repository.SaveAll(tasks);
            return Result.Ok();
        }
    }
}

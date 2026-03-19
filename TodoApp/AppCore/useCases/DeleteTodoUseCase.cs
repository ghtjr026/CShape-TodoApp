using System;
using System.Linq;
using TodoApp.AppCore.Interfaces;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 할일 삭제 UseCase.
    /// 전체 로드 → ID로 찾아 제거 → 저장.
    /// </summary>
    public class DeleteTodoUseCase
    {
        private readonly ITodoRepository _repository;

        public DeleteTodoUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result Execute(Guid id)
        {
            var tasks = _repository.LoadAll();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Fail("해당 할일을 찾을 수 없습니다.");

            tasks.Remove(task);
            _repository.SaveAll(tasks);
            return Result.Ok();
        }
    }
}

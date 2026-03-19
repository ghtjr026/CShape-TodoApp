using System;
using System.Collections.Generic;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 전체 할일 목록 저장 UseCase.
    /// </summary>
    public class SaveTodosUseCase
    {
        private readonly ITodoRepository _repository;

        public SaveTodosUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result Execute(List<TodoTask> tasks)
        {
            try
            {
                _repository.SaveAll(tasks);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("\uc800\uc7a5\uc5d0 \uc2e4\ud328\ud588\uc2b5\ub2c8\ub2e4: " + ex.Message);
            }
        }
    }
}

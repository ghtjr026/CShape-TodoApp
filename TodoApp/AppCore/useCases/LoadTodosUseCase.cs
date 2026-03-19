using System;
using System.Collections.Generic;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Shared.Result;

namespace TodoApp.AppCore.UseCases
{
    /// <summary>
    /// 전체 할일 목록 로드 UseCase.
    /// </summary>
    public class LoadTodosUseCase
    {
        private readonly ITodoRepository _repository;

        public LoadTodosUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public Result<List<TodoTask>> Execute()
        {
            try
            {
                var tasks = _repository.LoadAll();
                return Result.Ok(tasks);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<TodoTask>>("\ud560\uc77c \ubaa9\ub85d\uc744 \ubd88\ub7ec\uc624\ub294\ub370 \uc2e4\ud328\ud588\uc2b5\ub2c8\ub2e4: " + ex.Message);
            }
        }
    }
}

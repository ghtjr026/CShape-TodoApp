using System.Collections.Generic;
using TodoApp.Domain.Entities;

namespace TodoApp.AppCore.Interfaces
{
    /// <summary>
    /// 할일 저장소 인터페이스. AppCore는 저장 방식을 모름 (의존성 역전).
    /// Infrastructure에서 JSON, DB 등으로 구현체를 제공.
    /// </summary>
    public interface ITodoRepository
    {
        List<TodoTask> LoadAll();
        void SaveAll(List<TodoTask> tasks);
    }
}

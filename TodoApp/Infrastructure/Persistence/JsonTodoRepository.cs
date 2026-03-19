using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence
{
    /// <summary>
    /// JSON 파일 기반 할일 저장소.
    /// 실행 폴더의 todos.json에 저장/로드.
    /// .NET Framework 4.8 기본 내장 DataContractJsonSerializer 사용.
    /// </summary>
    public class JsonTodoRepository : ITodoRepository
    {
        private readonly string _filePath;

        public JsonTodoRepository()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(directory, "todos.json");
        }

        public List<TodoTask> LoadAll()
        {
            if (!File.Exists(_filePath))
                return new List<TodoTask>();

            var bytes = File.ReadAllBytes(_filePath);
            if (bytes.Length == 0)
                return new List<TodoTask>();

            var serializer = new DataContractJsonSerializer(typeof(List<TodoTask>));
            using (var stream = new MemoryStream(bytes))
            {
                return (List<TodoTask>)serializer.ReadObject(stream);
            }
        }

        public void SaveAll(List<TodoTask> tasks)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<TodoTask>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, tasks);
                File.WriteAllBytes(_filePath, stream.ToArray());
            }
        }
    }
}

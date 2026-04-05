using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using TodoApp.AppCore.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Api
{
    /// <summary>
    /// REST API 기반 할일 저장소.
    /// TodoServer와 HTTP 통신으로 DB에 저장/로드.
    /// </summary>
    public class ApiTodoRepository : ITodoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly DataContractJsonSerializerSettings _jsonSettings;

        public ApiTodoRepository(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient();

            // ISO 8601 날짜 형식 사용 (System.Text.Json 호환)
            _jsonSettings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffffffK")
            };
        }

        public List<TodoTask> LoadAll()
        {
            var response = _httpClient.GetAsync(_baseUrl + "/api/todos").Result;
            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsByteArrayAsync().Result;
            var serializer = new DataContractJsonSerializer(typeof(List<TodoTask>), _jsonSettings);
            using (var stream = new MemoryStream(json))
            {
                return (List<TodoTask>)serializer.ReadObject(stream);
            }
        }

        public void SaveAll(List<TodoTask> tasks)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<TodoTask>), _jsonSettings);
            byte[] jsonBytes;
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, tasks);
                jsonBytes = stream.ToArray();
            }

            var content = new ByteArrayContent(jsonBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = _httpClient.PutAsync(_baseUrl + "/api/todos/bulk", content).Result;
            response.EnsureSuccessStatusCode();
        }
    }
}

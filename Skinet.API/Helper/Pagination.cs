using System.Text.Json.Serialization;

namespace Skinet.API.Helper
{
    public class Pagination<T> where T : class
    {
        [JsonPropertyOrder(1)]
        public int StatusCode { get; set; }

        [JsonPropertyOrder(2)]
        public string Message { get; set; }

        [JsonPropertyOrder(3)]
        public bool Success { get; set; }

        [JsonPropertyOrder(4)]
        public int PageSize { get; set; }

        [JsonPropertyOrder(5)]
        public int PageIndex { get; set; }

        [JsonPropertyOrder(6)]
        public int Count { get; set; }

        [JsonPropertyOrder(7)]
        public T Data { get; set; }

        public Pagination()
        {

        }
        public Pagination(bool success, int pageIndex, int pageSize, int count, T data)
        {
            Success = success;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public Pagination(bool success, int statusCode, string message, int pageIndex, int pageSize, int count, T data)
        {
            Success = success;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

    }
}

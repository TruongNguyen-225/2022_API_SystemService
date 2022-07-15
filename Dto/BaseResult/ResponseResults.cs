using System;

namespace SystemServiceAPI.Dto.BaseResult
{
    public class ResponseResults
    {
        public int Code { get; set; }
        public string Msg { get; set; } = String.Empty;
        public object? Result { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace MyRoomServer.Models
{
    public class ApiRes
    {
        public ApiRes(string msg)
        {
            Msg = msg;
        }

        public ApiRes(string msg, object data)
        {
            Msg = msg;
            Data = data;
        }

        public string Msg { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; set; } = null!;
    }
}

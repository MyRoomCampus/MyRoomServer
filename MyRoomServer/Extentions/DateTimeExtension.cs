namespace MyRoomServer.Extentions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime _unixStart = new(1970, 1, 1, 8, 0, 0);
        public static int ToLocalTimeStamp(this DateTime dateTime)
        {
            return (int)(dateTime - _unixStart).TotalSeconds;
        }

        public static DateTime ToDateTime(this int timestamp)
        {
            return _unixStart.AddSeconds(timestamp);
        }
    }
}

namespace MyRoomServer.Extentions
{
    public static class HttpResponseExtension
    {
        public static void AddLastModified(this HttpResponse response)
        {
            response.Headers.Add("Last-Modified", DateTime.Now.ToString());
        }

        public static void AddLastModified(this HttpResponse response, DateTime dateTime)
        {
            response.Headers.Add("Last-Modified", dateTime.ToString());
        }
    }
}

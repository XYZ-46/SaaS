namespace API
{
    public record BaseResponse
    {
        public object message { get; set; }
        public object errorMessage { get; set; }
        public object errorProperty { get; set; }
        public object data { get; set; } = new object();
        public object page { get; set; } = new object();
    }
}

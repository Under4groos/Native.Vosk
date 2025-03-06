namespace Native.Vosk.Models
{
    public class VoskModel
    {
        public string? lang { get; set; }
        public string? lang_text { get; set; }
        public string? md5 { get; set; }
        public string? name { get; set; }
        public string? obsolete { get; set; }
        public object? size { get; set; }
        public string? size_text { get; set; }
        // big, big-lgraph
        public string? type { get; set; }
        public string? url { get; set; }
        public string? version { get; set; }


        public string GetInfo()
        {
            return $"name:\"{name}\"\n version:{version}\n lang:\"{lang} / {lang_text}\"\n size:{size_text}";
        }
    }
}

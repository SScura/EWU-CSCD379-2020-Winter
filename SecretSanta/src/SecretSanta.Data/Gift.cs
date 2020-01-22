using System;

namespace SecretSanta.Data
{
    public class Gift : FingerPrintEntityBase
    {
        public string Title { get => _Title; set => _Title = value ?? throw new ArgumentNullException(nameof(value)); }
        public string Description { get => _Description; set => _Description = value ?? throw new ArgumentNullException(nameof(value)); }
        public string Url { get => _Url; set => _Url = value ?? throw new ArgumentNullException(nameof(value)); }
        private string _Url = string.Empty;
        public User? User { get; set; }
        private string _Title = string.Empty;
        private string _Description = string.Empty;
    }
}
using System;

namespace SecretSanta.Data
{
    public class Gift : FingerPrintEntityBase
    {
        public string Title { get => _Title; set => _Title = value ?? throw new ArgumentNullException(nameof(Title)); }
        public string Description { get => _Description; set => _Description = value ?? throw new ArgumentNullException(nameof(Description)); }
        public string Url { get => _Url; set => _Url = value ?? throw new ArgumentNullException(nameof(Url)); }
#nullable disable
        public User User { get ; set; }
#nullable enable
        private string _Url = string.Empty;
        private string _Title = string.Empty;
        private string _Description = string.Empty;
    }
}
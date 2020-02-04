﻿using System;

namespace SecretSanta.Data
{
    public class Gift : FingerPrintEntityBase
    {
        public Gift(string title, string url, string description, int userId)
        {
            Title = title;
            Url = url;
            Description = description;
            UserId = userId;
        }
        public string Title { get => _Title; set => _Title = value ?? throw new ArgumentNullException(nameof(Title)); }
        private string _Title = string.Empty;
        public string Description { get => _Description; set => _Description = value ?? throw new ArgumentNullException(nameof(Description)); }
        private string _Description = string.Empty;
        public string Url { get => _Url; set => _Url = value ?? throw new ArgumentNullException(nameof(Url)); }
        private string _Url = string.Empty;
#nullable disable
        public User User { get; set; }
#nullable enable
        public int UserId { get; set; }

        public Gift()
            : this("", "", "", 0)
        { }
    }
}

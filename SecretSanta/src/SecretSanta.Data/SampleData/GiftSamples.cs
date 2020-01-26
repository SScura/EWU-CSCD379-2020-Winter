using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public static class GiftSamples
    {
        public static  Gift Car => new Gift(
            "Ferrari",
            "This gift cost way too much.",
            ".redferrari.",
            UserSamples.InigoMontoya
        );
        public static Gift Motorcycle => new Gift(
            "Harley Davidson",
            "That's more like it.",
            ".hd.",
            UserSamples.LukeSkywalker
        );
    }
}

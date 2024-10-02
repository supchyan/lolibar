﻿using System.Windows.Media;

namespace LolibarApp.Modding
{
    public class ModIcons
    {
                                                                    // --- Parse icon's svg path ---
        public static readonly Geometry SoundIcon = Geometry.Parse("M5 1C5 0.447715 4.55228 0 4 0C3.44772 0 3 0.447715 3 1L3 13C3 13.5523 3.44772 14 4 14C4.55229 14 5 13.5523 5 13L5 1ZM10 2C10.5523 2 11 2.44772 11 3V11C11 11.5523 10.5523 12 10 12C9.44771 12 9 11.5523 9 11V3C9 2.44772 9.44771 2 10 2ZM1 4C1.55228 4 2 4.44772 2 5L2 9C2 9.55229 1.55228 10 1 10C0.447715 10 0 9.55229 0 9V5C0 4.44772 0.447715 4 1 4ZM14 5C14 4.44772 13.5523 4 13 4C12.4477 4 12 4.44772 12 5V9C12 9.55229 12.4477 10 13 10C13.5523 10 14 9.55229 14 9V5ZM7 3C7.55228 3 8 3.44772 8 4V10C8 10.5523 7.55229 11 7 11C6.44772 11 6 10.5523 6 10L6 4C6 3.44772 6.44772 3 7 3Z");

        public static readonly Geometry PlayAudioIcon = Geometry.Parse("M2.0764 9.71553C1.22088 10.4124 0 9.74943 0 8.58791V1.41209C0 0.250579 1.22088 -0.412449 2.0764 0.284466L6.4809 3.87237C7.17303 4.43619 7.17304 5.56381 6.4809 6.12763L2.0764 9.71553Z");
        public static readonly Geometry PauseAudioIcon = Geometry.Parse("M1.16667 10C0.522334 10 0 9.44036 0 8.75V1.25C0 0.559644 0.522334 0 1.16667 0C1.811 0 2.33333 0.559644 2.33333 1.25V8.75C2.33333 9.44036 1.811 10 1.16667 10ZM5.83333 10C5.189 10 4.66666 9.44036 4.66666 8.75V1.25C4.66666 0.559644 5.189 0 5.83333 0C6.47766 0 6.99999 0.559644 6.99999 1.25V8.75C6.99999 9.44036 6.47766 10 5.83333 10Z");
        public static readonly Geometry PreviousAudioIcon = Geometry.Parse("M1 0C1.55228 0 2 0.559644 2 1.25V8.75C2 9.44036 1.55228 10 1 10C0.447715 10 0 9.44036 0 8.75V1.25C0 0.559644 0.447715 0 1 0ZM7.47 0.210064C8.0168 0.592995 8.16456 1.36906 7.80003 1.94346L5.86022 5L7.80003 8.05654C8.16456 8.63094 8.0168 9.40701 7.47 9.78994C6.9232 10.1729 6.18441 10.0177 5.81988 9.44326L3 5L5.81988 0.556743C6.18441 -0.0176529 6.9232 -0.172866 7.47 0.210064Z");
        public static readonly Geometry NextAudioIcon = Geometry.Parse("M7 10C6.44772 10 6 9.44036 6 8.75L6 1.25C6 0.559643 6.44772 0 7 0C7.55228 0 8 0.559643 8 1.25L8 8.75C8 9.44036 7.55228 10 7 10ZM0.529999 9.78994C-0.0168047 9.40701 -0.164562 8.63094 0.199974 8.05654L2.13978 5L0.199974 1.94346C-0.164562 1.36906 -0.0168047 0.592995 0.529999 0.210064C1.0768 -0.172867 1.81559 -0.0176525 2.18012 0.556743L5 5L2.18012 9.44326C1.81559 10.0177 1.0768 10.1729 0.529999 9.78994Z");
    }
}
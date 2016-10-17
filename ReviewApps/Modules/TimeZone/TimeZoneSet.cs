using System;
using ReviewApps.Models.POCO.IdentityCustomization;

namespace ReviewApps.Modules.TimeZone {
    public class TimeZoneSet {
        public TimeZoneInfo TimeZoneInfo { get; set; }
        public UserTimeZone UserTimezone { get; set; }

        public bool IsTimeZoneInfoExist() {
            return TimeZoneInfo != null;
        }

        public bool IsUserTimeZoneInfoExist() {
            return UserTimezone != null;
        }
    }
}
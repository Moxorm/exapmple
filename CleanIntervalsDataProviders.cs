using System;
using System.Collections;
using NUnit.Framework;
using TestCommonData;
using Trawl4ClassDescriptionConst;

namespace CleanIntervals.IntegrationTests.Test.Tests.Tests
{
    public class CleanIntervalsDataProviders
    {
        #region Naz
        public static IEnumerable C345496CleanIntervalsMainCaseNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C345496CleanIntervalsMainCaseNaz))
                    .SetCategory(nameof(TestCategories.PriorityHigh));
            }
        }
        
        public static IEnumerable C416052CleanIntervalsEmptyLongStorageDateNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        null,
                        null,
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416052CleanIntervalsEmptyLongStorageDateNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416053CleanIntervalsEmptyLongStorageDateForSummaryNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        null,
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416053CleanIntervalsEmptyLongStorageDateForSummaryNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416054CleanIntervalsEmptyLongStorageDateForTaskNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        null,
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416054CleanIntervalsEmptyLongStorageDateForTaskNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416055CleanIntervalsMiddleIntervalNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.AddDays(-1).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-1).AddSeconds(-1),
                        timeNow.ToUniversalTime().AddDays(-1).AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416055CleanIntervalsMiddleIntervalNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416056CleanIntervalsStartIntervalNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-3),
                        timeNow.ToUniversalTime().AddDays(-3),
                        timeNow.AddDays(-2).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-2).AddSeconds(-1),
                        timeNow.ToUniversalTime().AddDays(-2).AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416056CleanIntervalsStartIntervalNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416057CleanIntervalsChronicleIsCrossingDeleteDayNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.AddSeconds(-100).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416057CleanIntervalsChronicleIsCrossingDeleteDayNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416058CleanIntervalsChronicleSameAsSummaryNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        new DateTimeOffset(timeNow.Date.ToUniversalTime()),
                        new DateTimeOffset(timeNow.Date.ToUniversalTime()).AddDays(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416058CleanIntervalsChronicleSameAsSummaryNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416060CleanIntervalsCurrentDataLessThanAssignNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(1),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416060CleanIntervalsCurrentDataLessThanAssignNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416061CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(10),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416061CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C416062CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNaz
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddMinutes(10),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNazId)
                    .SetName(nameof(C416062CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNaz))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        #endregion
        #region Nvd
        public static IEnumerable C1760308CleanIntervalsMainCaseNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760308CleanIntervalsMainCaseNvd))
                    .SetCategory(nameof(TestCategories.PriorityHigh));
            }
        }
        
        public static IEnumerable C1760309CleanIntervalsEmptyLongStorageDateNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        null,
                        null,
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760309CleanIntervalsEmptyLongStorageDateNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760310CleanIntervalsEmptyLongStorageDateForSummaryNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        null,
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760310CleanIntervalsEmptyLongStorageDateForSummaryNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760311CleanIntervalsEmptyLongStorageDateForTaskNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        null,
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760311CleanIntervalsEmptyLongStorageDateForTaskNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760312CleanIntervalsMiddleIntervalNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.AddDays(-1).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-1).AddSeconds(-1),
                        timeNow.ToUniversalTime().AddDays(-1).AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760312CleanIntervalsMiddleIntervalNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760313CleanIntervalsStartIntervalNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-2),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-3),
                        timeNow.ToUniversalTime().AddDays(-3),
                        timeNow.AddDays(-2).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-2).AddSeconds(-1),
                        timeNow.ToUniversalTime().AddDays(-2).AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760313CleanIntervalsStartIntervalNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760314CleanIntervalsChronicleIsCrossingDeleteDayNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.AddSeconds(-100).ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760314CleanIntervalsChronicleIsCrossingDeleteDayNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760315CleanIntervalsChronicleSameAsSummaryNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        new DateTimeOffset(timeNow.Date.ToUniversalTime()),
                        new DateTimeOffset(timeNow.Date.ToUniversalTime()).AddDays(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760315CleanIntervalsChronicleSameAsSummaryNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760316CleanIntervalsCurrentDataLessThanAssignNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddDays(1),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760316CleanIntervalsCurrentDataLessThanAssignNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760317CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(10),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760317CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        public static IEnumerable C1760318CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNvd
        {
            get
            {
                var timeNow = DateTimeOffset.Now;
                yield return new TestCaseData(
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime().AddMinutes(1),
                        timeNow.ToUniversalTime().AddMinutes(10),
                        timeNow.ToUniversalTime().AddDays(-1),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime(),
                        timeNow.ToUniversalTime().AddSeconds(-1),
                        timeNow.ToUniversalTime().AddSeconds(1),
                        Trawl4_SystemGuids.SystemNvdId)
                    .SetName(nameof(C1760318CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNvd))
                    .SetCategory(nameof(TestCategories.PriorityMedium));
            }
        }
        
        #endregion
    }
}
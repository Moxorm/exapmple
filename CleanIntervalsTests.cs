using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Aggregation.IntegrationTests.Tools.Extensions;
using CleanIntervals.IntegrationTests.Test.Tests.Initialization;
using CleanIntervals.IntegrationTests.Test.Tests.TestsBases;
using Downloader.IntegrationTests.Tools.Extensions;
using Filters.Core.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RecordingDevices.Common.Domain.Chunks;
using RecordingDevices.IntegrationTests.Const;
using Storage.ClientService.Http.IntegrationTests.Helpers;
using Storage.IntegrationTests.DataHelper;
using TaskDocumentModule.IntegrationTests.Tools.Helpers;
using TaskDocumentModule.IntegrationTests.Tools.Helpers.Extensions;
using TaskDocumentModule.IntegrationTests.Tools.Models;
using Tasks.IntegrationTests.Base.Helpers;
using Tasks.IntegrationTests.Base.Helpers.Extensions;
using Tasks.IntegrationTests.Base.Models.TypedChannelLink;
using TestCommonData;
using TestToolsCommon.Helpers;
using TestToolsCommon.Models;
using TaskDocumentModule.IntegrationTests.Tools.Interfaces;
using TaskDocumentModule.IntegrationTests.Tools.Models.DiaryRecord;

namespace CleanIntervals.IntegrationTests.Test.Tests.Tests
{
	[TestFixture]
    public class CleanIntervalsTests
    {
	    private static List<TestInformationInterval> GetLinks(IReadOnlyCollection<TestTypedChannelLinkWithRange> data)
	    {
		    var result = new List<TestInformationInterval>();
		    var ranges = data.Select(x => x.Ranges).ToList();
		    foreach (var range in ranges)
		    {
			    foreach (var dict in range)
			    {
				    var intervals = dict.Value.ToList();
				    result = result.Concat(intervals).ToList();
			    }
		    }
		    
		    return result;
	    }
	    
	    private static (ITestCreateSummaryArgs, TestPartialUpdateSummaryArgs) PrepareSummary(
		    Guid taskId, 
		    string storageKey, 
		    DateTimeOffset formedFor, 
		    DateTimeOffset? dateOfIssueToInitiator)
	    {
		    var summaryArgs = TestSummaryDataDefault.GetDefaultCreateSummaryArgs(taskId);
		    summaryArgs.MinPeriodDateTimeOffset = formedFor;
		    summaryArgs.MaxPeriodDateTimeOffset = formedFor;
		    summaryArgs.StorageKey = storageKey;
		    var summaryUpdateArgs = new TestPartialUpdateSummaryArgs();
		    summaryUpdateArgs.StorageKey = storageKey;
		    summaryUpdateArgs.Status = "HandedToInitiator";
		    summaryUpdateArgs.DateOfIssueToInitiator = dateOfIssueToInitiator;
		    
		    return (summaryArgs, summaryUpdateArgs);
	    }
        		
	    private static async Task Trigger(
		    DeviceTestScope scope, 
		    Filter<TestTypedChannelLinkWithRange> filterTaskChannel, 
		    bool wait = true)
	    {
		    var client = new HttpClient();

		    var dicti = new JObject(new JProperty("name", "DataLifetime_CleanInformationIntervals"),
			    new JProperty("cron", "0 0 1 1 *"),
			    new JProperty("consumerKey", "DataLifetime_CleanInformationIntervals"));

		    string json = JsonConvert.SerializeObject(dicti, Formatting.Indented);

		    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

		    var byteContent = new ByteArrayContent(buffer);

		    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		    var shUrl = Loader.ShUrl;

		    var links = await Loader.ContainerService
			    .GetLinksWithRangesByFilterAsync(scope.AdditionalParameters, filterTaskChannel)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    TestsHelper.SendRequestToScheduler(client, shUrl + "api/v1/scheduler/RecurringJobs/Update", byteContent);
		    
		    TestsHelper.SendRequestToScheduler(client, shUrl + "api/v1/scheduler/RecurringJobs/Trigger/DataLifetime_CleanInformationIntervals", null);
		    
		    if (wait)
		    {
			    Assert.True(await Waiter.WaitAsync(async () =>
					    {
						    var linksLoc = await Loader.ContainerService
							    .GetLinksWithRangesByFilterAsync(scope.AdditionalParameters, filterTaskChannel)
							    .AssertSuccess()
							    .ConfigureAwait(false);

						    return !TestDataComparer.CompareCollections(GetLinks(links.Result),
							    GetLinks(linksLoc.Result));
					    },
					    TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)),
				    "Links have no changes after trigger Scheduler");
		    }
	    }
        		
	    private static async Task TriggerWithOutChanges(
		    DeviceTestScope scope, 
		    Filter<TestTypedChannelLinkWithRange> filterTaskChannel, 
		    bool wait = true)
	    {
		    var client = new HttpClient();

		    var dicti = new JObject(new JProperty("name", "DataLifetime_CleanInformationIntervals"),
			    new JProperty("cron", "0 0 1 1 *"),
			    new JProperty("consumerKey", "DataLifetime_CleanInformationIntervals"));

		    string json = JsonConvert.SerializeObject(dicti, Formatting.Indented);

		    var buffer = System.Text.Encoding.UTF8.GetBytes(json);

		    var byteContent = new ByteArrayContent(buffer);

		    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		    var shUrl = Loader.ShUrl;

		    var links = await Loader.ContainerService
			    .GetLinksWithRangesByFilterAsync(scope.AdditionalParameters, filterTaskChannel)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    
		    TestsHelper.SendRequestToScheduler(client, shUrl + "api/v1/scheduler/RecurringJobs/Update", byteContent);
		    
		    TestsHelper.SendRequestToScheduler(client, shUrl + "api/v1/scheduler/RecurringJobs/Trigger/DataLifetime_CleanInformationIntervals", null);
		    
		    await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);

	    }
	    
	    [Test]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C345496CleanIntervalsMainCaseNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416052CleanIntervalsEmptyLongStorageDateNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416053CleanIntervalsEmptyLongStorageDateForSummaryNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416054CleanIntervalsEmptyLongStorageDateForTaskNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416055CleanIntervalsMiddleIntervalNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416056CleanIntervalsStartIntervalNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416057CleanIntervalsChronicleIsCrossingDeleteDayNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416058CleanIntervalsChronicleSameAsSummaryNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760308CleanIntervalsMainCaseNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760309CleanIntervalsEmptyLongStorageDateNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760310CleanIntervalsEmptyLongStorageDateForSummaryNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760311CleanIntervalsEmptyLongStorageDateForTaskNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760312CleanIntervalsMiddleIntervalNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760313CleanIntervalsStartIntervalNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760314CleanIntervalsChronicleIsCrossingDeleteDayNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760315CleanIntervalsChronicleSameAsSummaryNvd))]
	    public async Task PositiveTestWithChronic(
		    DateTimeOffset taskStart, 
		    DateTimeOffset taskEnd, 
		    DateTimeOffset? taskLongStorage,
		    DateTimeOffset? summaryLongStorage, 
		    DateTimeOffset formedForSummary,
		    DateTimeOffset dateOfAssign,
		    DateTimeOffset chronicleStart,
		    DateTimeOffset chronicleEnd,
		    string taskType)
	    {
		    var task = TestDataDefault.GetDefaultArgs(); 
		    task.TaskData.TaskExecuting.LongStorageDate = taskLongStorage;
		    task.TaskData.CommonData.Start = taskStart;
		    task.TaskData.CommonData.End = taskEnd;
		    task.SetTaskTypeId(Guid.Parse(taskType));
		    
		    await using var scope = await new DeviceTestScope()
			    .Initialization(RecordDevicesNames.H905_16)
			    .ConfigureAwait(false);
		    var testChannel = scope.Channels.First();
		    
		    var taskId = await Loader.ContainerService
			    .CreateTaskAsyncReplaceReferences(scope.AdditionalParameters, task)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    
		    var toCreateDiaryRecordArgs = new TestCreateDiaryRecordArgs
		    {
			    RecordText = "le lel le",
			    TaskId = taskId.Result,
		    };
		    
		    var diaryRecordId = (await Loader.ContainerService
				    .CreateDiaryRecordAsync(toCreateDiaryRecordArgs, scope.AdditionalParameters))
			    .Result;
		    
		    var entityId = await TestsHelper.SaveEventEntityAsync(
				    taskId.Result,
				    chronicleStart,
				    chronicleEnd,
				    scope)
			    .ConfigureAwait(false);
		    
		    var storageKey = await Loader.ContainerService
			    .UploadDataAsync(
				    scope.AdditionalParameters,
				    DataHelper.GetUniqueStream())
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    (var summaryArgs, var summaryUpdateArgs) = PrepareSummary(
			    taskId.Result, 
			    storageKey.Result,
			    formedForSummary.Date.ToUniversalTime(), 
			    dateOfAssign);
		    
		    var summaryId = (await Loader.ContainerService
				    .CreateSummaryAsync(scope.AdditionalParameters, summaryArgs)
				    .AssertSuccess()
				    .ConfigureAwait(false))
			    .Result;
		    
		    await Loader.ContainerService
			    .LinkTasksToLogicChannelsAsync(scope.AdditionalParameters,
				    new[] {taskId.Result,},
				    new[] {testChannel.Id})
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .ToRecordLogicChannelsAsync(scope.AdditionalParameters,
				    new[]
				    {
					    taskId.Result,
				    })
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Waiter.WaitAsync(async () =>
			    {
				    scope.Chunks = (await Loader.ContainerService.LoadChunks(
						    scope.AdditionalParameters,
						    scope.RecordingChannel.Id,
						    scope.StartDate,
						    scope.EndDate,
						    ChunkType.AudioVideo,
						    0,
						    100)
					    .AssertSuccess()
					    .ConfigureAwait(false)).Result.ToList();
				    return scope.Chunks.Any();
			    },
			    TimeSpan.FromSeconds(Loader.DownloadPeriodSeconds),
			    TimeSpan.FromMinutes(Loader.TimeoutForChunks));

		    var filterTaskChannel = new Filter<TestTypedChannelLinkWithRange>();
		    var predicateTaskChannel = filterTaskChannel.AddPredicate(t => t.ExternalIdentifier);
		    predicateTaskChannel.Equal(taskId.Result);
		    
		    summaryUpdateArgs.SummaryId = summaryId;
		    summaryUpdateArgs.LongStorageDate = summaryLongStorage;
		    await Loader.ContainerService
			    .PartialUpdateSummaryAsync(scope.AdditionalParameters, summaryUpdateArgs)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    await Loader.ContainerService
			    .ResetBlockingToEditSummaryAsync(scope.AdditionalParameters, summaryId)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    
		    await Trigger(scope, filterTaskChannel).ConfigureAwait(false);
		    
		    //prepare result
		    
		    var interval1 = new TestInformationInterval();
		    interval1.Start = task.TaskData.CommonData.Start ?? new DateTimeOffset();
		    interval1.End = formedForSummary.Date.ToUniversalTime();
		    var interval2 = new TestInformationInterval();
		    interval2.Start = formedForSummary.Date.ToUniversalTime().AddDays(1);
		    interval2.End = task.TaskData.CommonData.End ?? new DateTimeOffset();
		    
		    //get result
		    
		    var links = await Loader.ContainerService
			    .GetLinksWithRangesByFilterAsync(scope.AdditionalParameters, filterTaskChannel)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    
		    var summaryResult = await Loader.ContainerService
			    .GetSummaryAsync(scope.AdditionalParameters, summaryId)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    if (chronicleStart.LocalDateTime.Date == formedForSummary.Date && chronicleEnd.Date == formedForSummary.Date)
		    {
			    await Loader.ContainerService
				    .GetEntityAsync(entityId, scope.AdditionalParameters)
				    .AssertError(CommonErrorCodes.ErrorCode404)
				    .ConfigureAwait(false);
		    }
		    
		    await Loader.ContainerService
			    .DownloadDataAsync(scope.AdditionalParameters, storageKey.Result)
			    .AssertError(CommonErrorCodes.ErrorCode404)
			    .ConfigureAwait(false);
		    
		    if (formedForSummary.Date == DateTime.Now.Date.ToUniversalTime())
		    {
			    await Loader.ContainerService
				    .GetDiaryRecordByIdentifierAsync(diaryRecordId, scope.AdditionalParameters)
				    .AssertError(CommonErrorCodes.ErrorCode404)
				    .ConfigureAwait(false);
		    }

		    // Assert result
			   
		    Assert.IsNull(summaryResult.Result,
			    "Summary was not deleted");

		    var expected = new List<TestInformationInterval>();
		    if (taskStart.Date < formedForSummary.Date)
		    {
			    expected.Add(interval1);
		    }
		    if (taskEnd.Date > formedForSummary.Date)
		    {
			    expected.Add(interval2);
		    }

		    Assert.That(GetLinks(links.Result), 
			    Is.EquivalentTo(expected),
			    "Result intervals are not cuted");

	    }

	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416060CleanIntervalsCurrentDataLessThanAssignNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416061CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C416062CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNaz))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760316CleanIntervalsCurrentDataLessThanAssignNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760317CleanIntervalsCurrentDataLessThanLongStorageDAteForSummaryNvd))]
	    [TestCaseSource(typeof(CleanIntervalsDataProviders),
		    nameof(CleanIntervalsDataProviders.C1760318CleanIntervalsCurrentDataLessThanLongStorageDAteForTaskNvd))]
	    public async Task PositiveTestNoClean(
		    DateTimeOffset taskStart,
		    DateTimeOffset taskEnd,
		    DateTimeOffset? taskLongStorage,
		    DateTimeOffset? summaryLongStorage,
		    DateTimeOffset formedForSummary,
		    DateTimeOffset dateOfAssign,
		    DateTimeOffset chronicleStart,
		    DateTimeOffset chronicleEnd,
		    string taskType)
	    {
		    var task = TestDataDefault.GetDefaultArgs();
		    task.TaskData.TaskExecuting.LongStorageDate = taskLongStorage;
		    task.TaskData.CommonData.Start = taskStart;
		    task.TaskData.CommonData.End = taskEnd;
		    task.SetTaskTypeId(Guid.Parse(taskType));

		    await using var scope = await new DeviceTestScope()
			    .Initialization(RecordDevicesNames.H905_16)
			    .ConfigureAwait(false);
		    var testChannel = scope.Channels.First();

		    var taskId = await Loader.ContainerService
			    .CreateTaskAsyncReplaceReferences(scope.AdditionalParameters, task)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    var toCreateDiaryRecordArgs = new TestCreateDiaryRecordArgs
		    {
			    RecordText = "le lel le",
			    TaskId = taskId.Result,
		    };

		    var diaryRecordId = await Loader.ContainerService
			    .CreateDiaryRecordAsync(toCreateDiaryRecordArgs, scope.AdditionalParameters)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    var entityId = await TestsHelper.SaveEventEntityAsync(
				    taskId.Result,
				    chronicleStart,
				    chronicleEnd,
				    scope)
			    .ConfigureAwait(false);

		    var storageKey = await Loader.ContainerService
			    .UploadDataAsync(
				    scope.AdditionalParameters,
				    DataHelper.GetUniqueStream())
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    (var summaryArgs, var summaryUpdateArgs) = PrepareSummary(
			    taskId.Result, 
			    storageKey.Result,
			    formedForSummary.Date.ToUniversalTime(), 
			    dateOfAssign);

		    var summaryId = await Loader.ContainerService
				    .CreateSummaryAsync(scope.AdditionalParameters, summaryArgs)
				    .AssertSuccess()
				    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .LinkTasksToLogicChannelsAsync(scope.AdditionalParameters,
				    new[] {taskId.Result,},
				    new[] {testChannel.Id})
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .ToRecordLogicChannelsAsync(scope.AdditionalParameters,
				    new[]
				    {
					    taskId.Result,
				    })
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Waiter.WaitAsync(async () =>
			    {
				    scope.Chunks = (await Loader.ContainerService.LoadChunks(
						    scope.AdditionalParameters,
						    scope.RecordingChannel.Id,
						    scope.StartDate,
						    scope.EndDate,
						    ChunkType.AudioVideo,
						    0,
						    100)
					    .AssertSuccess()
					    .ConfigureAwait(false)).Result.ToList();
				    return scope.Chunks.Any();
			    },
			    TimeSpan.FromSeconds(Loader.DownloadPeriodSeconds),
			    TimeSpan.FromMinutes(Loader.TimeoutForChunks));

		    var filterTaskChannel = new Filter<TestTypedChannelLinkWithRange>();
		    var predicateTaskChannel = filterTaskChannel.AddPredicate(t => t.ExternalIdentifier);
		    predicateTaskChannel.Equal(taskId.Result);

		    summaryUpdateArgs.SummaryId = summaryId.Result;
		    summaryUpdateArgs.LongStorageDate = summaryLongStorage;
		    await Loader.ContainerService
			    .PartialUpdateSummaryAsync(scope.AdditionalParameters, summaryUpdateArgs)
			    .AssertSuccess()
			    .ConfigureAwait(false);
		    await Loader.ContainerService
			    .ResetBlockingToEditSummaryAsync(scope.AdditionalParameters, summaryId.Result)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await TriggerWithOutChanges(scope, filterTaskChannel).ConfigureAwait(false);

		    //prepare result

		    var interval = new TestInformationInterval();
		    interval.Start = task.TaskData.CommonData.Start ?? new DateTimeOffset();
		    interval.End = task.TaskData.CommonData.End ?? new DateTimeOffset();

		    //get result

		    var links = await Loader.ContainerService
			    .GetLinksWithRangesByFilterAsync(scope.AdditionalParameters, filterTaskChannel)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    var summaryResult = await Loader.ContainerService
			    .GetSummaryAsync(scope.AdditionalParameters, summaryId.Result)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .GetEntityAsync(entityId, scope.AdditionalParameters)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .DownloadDataAsync(scope.AdditionalParameters, storageKey.Result)
			    .AssertSuccess()
			    .ConfigureAwait(false);

		    await Loader.ContainerService
			    .GetDiaryRecordByIdentifierAsync(diaryRecordId.Result, scope.AdditionalParameters)
			    .AssertSuccess()
			    .ConfigureAwait(false);


		    // Assert result

		    Assert.IsNotNull(summaryResult.Result,
			    "Summary was deleted");

		    Assert.That(GetLinks(links.Result),
			    Is.EquivalentTo(new[]{interval}),
			    "Result intervals are not cuted");

	    }
    }
}
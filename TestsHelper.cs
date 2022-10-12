using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AccountingObjects.Core.Patches;
using Aggregation.IntegrationTests.Tools.Extensions;
using Aggregation.IntegrationTests.Tools.Models;
using Analysis.IntegrationTests.Models.Models;
using Analysis.IntegrationTests.Models.Models.Builders;
using Analysis.IntegrationTests.Tools.Helpers.Extensions;
using ClassDescription.Const.Common;
using CleanIntervals.IntegrationTests.Test.Tests.Initialization;
using CleanIntervals.IntegrationTests.Test.Tests.TestsBases;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TaskDocumentModule.IntegrationTests.Tools.Models.ModelsForRecords;
using TestToolsCommon.DependencyInjection;
using TestToolsCommon.Helpers;
using TestToolsCommon.Models;
using Trawl.Aggregation.SystemTypes;
using Trawl.Aggregation.SystemTypes.Media.Chronic;
using Trawl.Aggregation.SystemTypes.Media.Event;
using Trawl.Aggregation.SystemTypes.Media.Segment;

namespace CleanIntervals.IntegrationTests.Test.Tests
{
    public static class TestsHelper
    {
        public static TestTypedEntity CreateEventEntityAsync(
            Guid taskId,
            Guid channelId,
            Guid userId,
            DateTimeOffset start,
            DateTimeOffset end)
        {
            // = DateTimeOffset.UtcNow.ToUniversalTime().AddSeconds(10)
            var eventId = Guid.NewGuid();
            var eventInformation = new TestEventInformation
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                StartedAtDateTimeUnix = start.ToUnixTimeMilliseconds(),
                EndedAtDateTimeUnix = end.ToUnixTimeMilliseconds(),
                Duration = end.ToUnixTimeMilliseconds() - start.ToUnixTimeMilliseconds(),
                CreatedByUserAccountingObjectId = userId
            };

            var data = new TestTypedEntity()
                .SetTypeId(AggregatedPacketMainInformationKeys.TypeId)
                .SetAdditionalInformation(new[]
                {
                    new TestInformationHolder
                    {
                        Id = Guid.NewGuid(),
                        TypeId = ChronicalEventInformationKeys.TypeId,
                        Informations = new[]
                        {
                            JObject.FromObject(new TestChronicalEventInformation
                            {
                                Id = Guid.NewGuid(),
                                TaskId = taskId,
                                Description = TestStringsHelper.RandomLatinString(),
                                ChannelId = channelId,
                                EventInformationId = eventInformation.Id
                            })
                        }
                    },
                    new TestInformationHolder
                    {
                        Id = eventId,
                        TypeId = EventInformationKeys.TypeId,
                        Informations = new[]
                        {
                            JObject.FromObject(eventInformation)
                        }
                    },
                    new TestInformationHolder
                    {
                        Id = Guid.NewGuid(),
                        TypeId = MediaSegmentInformationKeys.TypeId,
                        Informations = new[]
                        {
                            JObject.FromObject(new TestMediaSegmentInformation
                            {
                                Id = Guid.NewGuid(),
                                ChannelId = channelId,
                                TaskId = taskId,
                                EventInformationId = eventInformation.Id,
                            }),
                        }
                    }

                });
            return data;
        }
        
        public static async Task<Guid> SaveEventEntityAsync(
            Guid taskId,
            DateTimeOffset start,
            DateTimeOffset end,
            DeviceTestScope scope)
        {
            
            var patchBuilder = new ApplyPatchBuilder()
                .SetMandatePatch(new TestMandatePatchData())
                .SetPatchType(PatchType.Create)
                .SetClassDescriptionKey(UserClassDescriptionBaseConst.CdKey)
                .AddFieldPatch(new TestFieldPatchData(FieldPatchType.Set, UserClassDescriptionBaseConst.Name,
                    TestStringsHelper.RandomLatinString(15)));

            var user = (await Loader.ContainerService.ApplyPatchAsync(
                        scope.AdditionalParameters,
                        patchBuilder.Build())
                    .AssertSuccess()
                    .ConfigureAwait(false))
                .Result;
            
            var entity = CreateEventEntityAsync(taskId, scope.RecordingChannel.Id, user.ObjectId, start, end);
            
		    await Loader.ContainerService
                .SaveEntityAsync(entity, scope.AdditionalParameters)
                .AssertSuccess()
                .ConfigureAwait(false);
            
            return entity.Id;
        }

        public static async void SendRequestToScheduler(HttpClient client, string url, ByteArrayContent byteContent)
        {
            Loader.ContainerService.GetTestLogger().Info($"Try send HTTP request to Scheduler url {url}");
            var responseUpdate = await client
                .PostAsync(url, byteContent)
                .ConfigureAwait(false);
            Assert.IsTrue(responseUpdate.StatusCode == HttpStatusCode.OK, 
                $"Send HTTP request to Scheduler url {url} exec with error." + Environment.NewLine + 
                $"Status code: {responseUpdate.StatusCode}" + Environment.NewLine +
                $"Error message: {responseUpdate.ReasonPhrase}");
        }
        

    }
}
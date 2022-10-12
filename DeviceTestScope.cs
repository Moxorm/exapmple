using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.IntegrationTests.Tools.Extensions;
using CleanIntervals.IntegrationTests.Test.Tests.Initialization;
using Downloader.IntegrationTests.Tools.Extensions;
using Downloader.IntegrationTests.Tools.Models;
using NUnit.Framework;
using RecordingDevices.Common.Core;
using RecordingDevices.IntegrationTests.Const;
using RecordingDevices.IntegrationTests.Tools.Helpers;
using RecordingDevices.IntegrationTests.Tools.Helpers.Extensions;
using RecordingDevices.IntegrationTests.Tools.Models;
using TestCommonData;
using TestToolsCommon.Helpers;
using TestToolsCommon.Models;

namespace CleanIntervals.IntegrationTests.Test.Tests.TestsBases
{
	public class DeviceTestScope
	{
		public List<TestChunkInfo> Chunks;
		public IList<TestChannelInfo> Channels { get; private set; }
		public Guid RecordDeviceId { get; private set; }
		public Guid RecordStationId { get; private set; }
		public DateTimeOffset StartDate { get; private set; }
		public DateTimeOffset EndDate { get; private set; }
		public TestChannelInfo RecordingChannel { get; private set; }
		public AdditionalParameters AdditionalParameters { get; private set; }

		public async Task<DeviceTestScope> Initialization(
			RecordDevicesNames device,
			string login = MainTestLoginData.TestLogin,
			string password = MainTestLoginData.TestPassword)
		{
			try
			{
				var result = await Loader.ContainerService.LoginAsync(login, password)
					.AssertSuccess();
				AdditionalParameters = new AdditionalParameters(result.Result.SessionIdentifier);
				RecordStationId = (await Loader.ContainerService.CreateSubGroupAsync(
						AdditionalParameters,
						new TestRecordStationParams
						{
							Name = TestStringsHelper.RandomLatinString(),
							Description = TestStringsHelper.RandomLatinString(),
							Category = TestStringsHelper.RandomLatinString()
						})
					.AssertSuccess()
					.ConfigureAwait(false)).Result;

				var deviceInfo = ManagerCreationHelper.Convert(Loader.DeviceConfiguration, device);

				RecordDeviceId = (await Loader.ContainerService.AddRecordingDeviceAsync(
							AdditionalParameters,
							RecordStationId,
							deviceInfo)
						.AssertSuccess()
						.ConfigureAwait(false))
					.Result;

				StartDate = DateTime.UtcNow.AddMinutes(-Loader.SleepBeforeStartRecordingMin);
				StartDate = StartDate.AddTicks(-(StartDate.Ticks % TimeSpan.TicksPerSecond));
				Assert.True(await Waiter.WaitAsync(() =>
						{
							Channels = (Loader.ContainerService.GetChannelsAsync(
										AdditionalParameters,
										new TestChannelsFilter
										{
											DeviceIds = new[] {RecordDeviceId},
											Limit = 100
										})
									.AssertSuccess()).GetAwaiter().GetResult()
								.Result.Items;

							return Channels.Count == device.GetCountOfChannels();
						},
						TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)),
					$"Channels count for {device}. Expected {device.GetCountOfChannels()}, but found {Channels.Count}");

				RecordingChannel = Channels.First();
				EndDate = DateTime.UtcNow;
				EndDate = EndDate.AddTicks(-(EndDate.Ticks % TimeSpan.TicksPerSecond));
			}
			catch
			{
				await Loader.ContainerService.DeleteAllDevicesAndStations(AdditionalParameters);

				if (AdditionalParameters != null)
				{
					await Loader.ContainerService.LogoutAsync(
							AdditionalParameters.SessionId)
						.AssertSuccess()
						.ConfigureAwait(false);
				}

				throw;
			}

			return this;
		}

		public async ValueTask DisposeAsync()
		{
			try
			{
				var devices = await Loader.ContainerService.GetDevicesAsync(AdditionalParameters,
					new TestDevicesFilter()
					{
						Limit = 5000
					}, false);
				//calling GetNextIntervals for filling dictionary and then getting all intervals and set state to inactive
				//if there are any active intervals for recording device, you can't remove it from station
				if (devices != null)
				{
					foreach (var device in devices.Result.Items)
					{

						var intervals = (await Loader.ContainerService
							.GetAllIntervalsForDeviceAsync(device.Id)
							.AssertSuccess()
							.ConfigureAwait(false)).Result;

						foreach (var interval in intervals)
						{
							await Loader.ContainerService
								.UpdateIntervalStateAsync(interval.DeviceId, interval.Id, false)
								.ConfigureAwait(false);
						}
					}
				}

				await Loader.ContainerService.DeleteAllDevicesAndStations(AdditionalParameters);
			}
			finally
			{
				if (AdditionalParameters != null)
				{
					await Loader.ContainerService.LogoutAsync(
							AdditionalParameters.SessionId)
						.ConfigureAwait(false);
				}
			}
		}
	}
}
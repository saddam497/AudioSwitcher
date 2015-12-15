﻿using System;
using System.Linq;
using AudioSwitcher.Tests.Common;
using Moq;
using Xunit;

namespace AudioSwitcher.AudioApi.Tests
{
    public partial class DeviceTests
    {
        private IAudioController CreateTestController()
        {
            return new TestDeviceController(2, 2);
        }


        [Fact]
        public void Device_DeviceType_PlaybackIsPlayback()
        {
            var controller = CreateTestController();
            Assert.True(DeviceType.Playback.HasFlag(controller.GetPlaybackDevices().First().DeviceType));
        }

        [Fact]
        public void Device_DeviceType_PlaybackIsAll()
        {
            var controller = CreateTestController();
            Assert.True(DeviceType.All.HasFlag(controller.GetPlaybackDevices().First().DeviceType));
        }

        [Fact]
        public void Device_DeviceType_CaptureIsCapture()
        {
            var controller = CreateTestController();
            Assert.True(DeviceType.Capture.HasFlag(controller.GetCaptureDevices().First().DeviceType));
        }

        [Fact]
        public void Device_DeviceType_CaptureIsAll()
        {
            var controller = CreateTestController();
            Assert.True(DeviceType.All.HasFlag(controller.GetCaptureDevices().First().DeviceType));
        }

        [Fact]
        public void DefaultDeviceChangedArgs_Sets_Device_And_Type_And_Default()
        {
            const bool communicationsDevice = false;
            var device = new Mock<IDevice>();
            var args = new DefaultDeviceChangedArgs(device.Object, communicationsDevice);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.DefaultDevice, args.ChangedType);
            Assert.True(args.IsDefaultEvent);
            Assert.False(args.IsDefaultCommunicationsEvent);
        }

        [Fact]
        public void DefaultDeviceChangedArgs_Sets_Device_And_Type_And_DefaultCommunications()
        {
            const bool communicationsDevice = true;
            var device = new Mock<IDevice>();
            var args = new DefaultDeviceChangedArgs(device.Object, communicationsDevice);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.DefaultCommunicationsDevice, args.ChangedType);
            Assert.True(args.IsDefaultCommunicationsEvent);
            Assert.False(args.IsDefaultEvent);
        }

        [Fact]
        public void DeviceAddedArgs_Sets_Device_And_Type()
        {
            var device = new Mock<IDevice>();
            var args = new DeviceAddedArgs(device.Object);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.DeviceAdded, args.ChangedType);
        }

        [Fact]
        public void DeviceRemovedArgs_Sets_Device_And_Type()
        {
            var device = new Mock<IDevice>();
            var args = new DeviceRemovedArgs(device.Object);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.DeviceRemoved, args.ChangedType);
        }

        [Fact]
        public void DeviceStateChangedArgs_Sets_Device_And_Type()
        {
            const DeviceState state = DeviceState.Active;
            var device = new Mock<IDevice>();
            var args = new DeviceStateChangedArgs(device.Object, state);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.StateChanged, args.ChangedType);
            Assert.Equal(state, args.State);
        }

        [Fact]
        public void DeviceVolumeChangedArgs_Sets_Device_And_Type()
        {
            const int volume = 23;
            var device = new Mock<IDevice>();
            var args = new DeviceVolumeChangedArgs(device.Object, volume);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(volume, args.Volume);
        }

        [Fact]
        public void DevicePropertyChangedArgs_Sets_Device_And_Type()
        {
            const string propertyName = "something";
            var device = new Mock<IDevice>();
            var args = new DevicePropertyChangedArgs(device.Object, propertyName);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.PropertyChanged, args.ChangedType);
            Assert.Equal(propertyName, args.PropertyName);
        }

        [Fact]
        public void DevicePropertyChangedArgs_FromExpression_Sets_Device_And_Type()
        {
            var device = new Mock<IDevice>();
            var args = DevicePropertyChangedArgs.FromExpression(device.Object, x => x.Controller);

            Assert.NotNull(args);
            Assert.NotNull(args.Device);
            Assert.Equal(DeviceChangedType.PropertyChanged, args.ChangedType);
            Assert.Equal("Controller", args.PropertyName);
        }

        [Fact]
        public void DevicePropertyChangedArgs_FromExpression_Invalid_Sets_Device_And_Type()
        {
            var device = new Mock<IDevice>();
            Assert.ThrowsAny<Exception>(() => DevicePropertyChangedArgs.FromExpression(device.Object, x => x.SetAsDefault()));
        }
    }
}

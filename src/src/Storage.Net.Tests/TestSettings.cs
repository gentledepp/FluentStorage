﻿using Config.Net;

namespace Storage.Net.Tests.Integration
{
   public class TestSettings : SettingsContainer
   {
      public readonly Option<string> AzureStorageName = new Option<string>("Azure.Storage.Name", null);

      public readonly Option<string> AzureStorageKey = new Option<string>("Azure.Storage.Key", null);

      public readonly Option<string> ServiceBusConnectionString = new Option<string>("Azure.ServiceBus.ConnectionString", null);

      public readonly Option<string> ServiceBusQueueName = new Option<string>("Azure.ServiceBus.QueueName", "testqueuelocal");

      public readonly Option<string> ServiceBusTopicName = new Option<string>("Azure.ServiceBus.TopicName", "testtopiclocal");

      public readonly Option<string> OneDriveClientId = new Option<string>("OneDrive.ClientId", null);

      public readonly Option<string> OneDriveClientSecret = new Option<string>("OneDrive.ClientSecret", null);

      public readonly Option<string> AwsAccessKeyId = new Option<string>("Aws.AccessKeyId", null);

      public readonly Option<string> AwsSecretAccessKey = new Option<string>("Aws.SecretAccessKey", null);

      public readonly Option<string> AwsTestBucketName = new Option<string>("Aws.TestBucketName", null);

      protected override void OnConfigure(IConfigConfiguration configuration)
      {
         configuration.UseIniFile("c:\\tmp\\integration-tests.ini");
         configuration.UseEnvironmentVariables();
      }

      private static TestSettings _instance;

      public static TestSettings Instance
      {
         get { return _instance ?? (_instance = new TestSettings()); }
      }
   }
}
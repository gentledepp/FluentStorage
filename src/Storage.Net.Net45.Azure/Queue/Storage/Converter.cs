﻿using Microsoft.WindowsAzure.Storage.Queue;
using Storage.Net.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Net.Azure.Queue.Storage
{
   static class Converter
   {
      private const string PropBeginWord = "PROPBEGIN";
      private const string PropEndWord = "PROPEND";

      public static CloudQueueMessage ToCloudQueueMessage(QueueMessage message)
      {
         if(message.Properties == null || message.Properties.Count == 0)
         {
            return new CloudQueueMessage(message.Content);
         }

         var sb = new StringBuilder();
         sb.Append(PropBeginWord);
         var clazz = new JsonProps
         {
            Properties = message.Properties.Select(p => new JsonProp { Name = p.Key, Value = p.Value }).ToArray()
         };
         sb.Append(clazz.ToJsonString());
         sb.Append(PropEndWord);
         sb.Append(message.Content);

         return new CloudQueueMessage(sb.ToString());
      }

      public static QueueMessage ToQueueMessage(CloudQueueMessage message)
      {
         if(message == null) return null;

         string content = message.AsString;
         if(content.StartsWith(PropBeginWord))
         {
            int idx = content.IndexOf(PropEndWord);
            if(idx != -1)
            {
               string json = content.Substring(PropBeginWord.Length, idx - PropBeginWord.Length);
               content = content.Substring(idx + PropEndWord.Length);
               JsonProps props = json.AsJsonObject<JsonProps>();
               var result = new QueueMessage(CreateId(message), content);
               foreach(JsonProp prop in props.Properties)
               {
                  result.Properties[prop.Name] = prop.Value;
               }
               return result;
            }
         }

         return new QueueMessage(CreateId(message), message.AsString);
      }

      private static string CreateId(CloudQueueMessage message)
      {
         if(string.IsNullOrEmpty(message.PopReceipt)) return message.Id;

         return message.Id + ":" + message.PopReceipt;
      }

      internal static void SplitId(string compositeId, out string id, out string popReceipt)
      {
         string[] parts = compositeId.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
         id = parts[0];
         popReceipt = parts.Length > 1 ? parts[1] : null;
      }

   }
}
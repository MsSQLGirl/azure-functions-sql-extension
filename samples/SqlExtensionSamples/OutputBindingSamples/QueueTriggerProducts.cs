﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static SqlExtensionSamples.ProductUtilities;

namespace SqlExtensionSamples
{
    public static class QueueTriggerProducts
    {
        [FunctionName("QueueTriggerProducts")]
        public static void Run(
            [QueueTrigger("testqueue")] string queueMessage, ILogger log,
            [Sql("[dbo].[Products]", ConnectionStringSetting = "SqlConnectionString")] ICollector<Product> products)
        {
            int totalUpserts = 100;
            log.LogInformation($"[QueueTrigger]: {DateTime.Now} starting execution {queueMessage}. Rows to generate={totalUpserts}.");

            var sw = new Stopwatch();
            sw.Start();

            List<Product> newProducts = GetNewProducts(totalUpserts);
            foreach (Product product in newProducts)
            {
                products.Add(product);
            }

            string line = $"[QueueTrigger]: {DateTime.Now} finished execution {queueMessage}. Total time to create {totalUpserts} rows={sw.ElapsedMilliseconds}.";
            log.LogInformation(line);

        }
    }
}

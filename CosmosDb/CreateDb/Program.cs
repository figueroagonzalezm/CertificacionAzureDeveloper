using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CreateDb
{
    public class Program
    {
        private static readonly string _endpointUri = "https://cosmosdbaccount-mafg.documents.azure.com:443/";
        private static readonly string _primaryKey = "F4DiFcqpFbbnCXnbmwG1e7rvffnjOtuAGVfCjo2GWpThcIsJVTPv9YsZrrO4SDgyqhoM6AMs5I5FqOBEQlkpbQ==";
        public static async Task Main(string[] args)
        {         
            using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
            {        
                DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync("Products");
                Database targetDatabase = databaseResponse.Database;
                await Console.Out.WriteLineAsync($"Database Id:\t{targetDatabase.Id}");

                IndexingPolicy indexingPolicy = new IndexingPolicy
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true,
                    IncludedPaths =
                    {
                        new IncludedPath
                        {
                            Path = "/*"
                        }
                    }
                };
                var containerProperties = new ContainerProperties("Clothing", "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };
                var containerResponse = await targetDatabase.CreateContainerIfNotExistsAsync(containerProperties, 10000);
                var customContainer = containerResponse.Container;
                await Console.Out.WriteLineAsync($"Custom Container Id:\t{customContainer.Id}");
            }
        }
    }
}

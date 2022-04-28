using System;
using System.Globalization;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SeaDrillParsingFunction.Models;
using System.Linq;
using System.Collections.Generic;
using TinyCsvParser.Mapping;
using Kusto.Data;
using Kusto.Data.Net.Client;
using Kusto.Data.Common;
using Kusto.Ingest;
using System.Data;
using System.Threading;
using Kusto.Cloud.Platform.Utils;
using TinyCsvParser;

namespace SeaDrillParsingFunction
{
    public static class SeaDrillParsingFunction
    {

        //#region Property  
        //private readonly AppDbContext appDbContext;
        //#endregion

        //#region Constructor  
        //public SeaDrillParsingFunction(AppDbContext appDbContext)
        //{
        //    this.appDbContext = appDbContext;
        //}
        //#endregion


        [FunctionName("SeaDrillParsingFunction")]
        public static void Run([BlobTrigger("armtemplate/{name}", Connection = "AzureBlobStorageConnString")]Stream myBlob, string name, ILogger log)
        {
            //log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            //List<FileRecords> result;
            //var enUsCulture = new CultureInfo("en-US");
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvFileRecordsMapping csvMapper = new CsvFileRecordsMapping();
            CsvParser<FileRecords> csvParser = new CsvParser<FileRecords>(csvParserOptions, csvMapper);


            var csv = csvParser.ReadFromStream(myBlob, System.Text.Encoding.UTF8).ToList();
            
            IDataReader GetDataAsIDataReader()
            {

                var ret = new Kusto.Cloud.Platform.Data.EnumerableDataReader<FileRecords>(csv.Select(x => x.Result), "Country", "Confirmed", "Deaths", "Recovered", "Active", "NewCases", "NewDeaths", "NewRecovered","DeathsBy100Cases", "RecoveredBy100Cases", "DeathsBy100Recovered", "ConfirmedLastWeek", "OneWeekChange", "OneWeekPercentIncrease", "WHORegion");
                return ret;
            }


            var tenantId = "xxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx";
            var kustoUri = "https://xxxxxxxxtestforrajat.centralindia.kusto.windows.net";
            var databaseName = "xxxxxxdb";
            var tableName = "xxxxxxxtest";

            var kustoConnectionStringBuilder = new KustoConnectionStringBuilder(kustoUri).WithAadUserPromptAuthentication(tenantId);

            using (var kustoClient = KustoClientFactory.CreateCslAdminProvider(kustoConnectionStringBuilder))
            {
                IKustoIngestClient directClient = KustoIngestFactory.CreateDirectIngestClient(kustoConnectionStringBuilder);
                var kustoIngestionProperties = new KustoIngestionProperties(databaseName, tableName);
                directClient.IngestFromDataReaderAsync(GetDataAsIDataReader(), kustoIngestionProperties);

            }


            //appDbContext.FileRecords.AddRange(result);
            //appDbContext.SaveChanges();

        }


        class Row
        {
            public string a;
            public int b;

            public Row(string a, int b)
            {
                this.a = a;
                this.b = b;
            }
        }

        private class CsvFileRecordsMapping : CsvMapping<FileRecords>
        {
            public CsvFileRecordsMapping()
                : base()
            {
                MapProperty(0, x => x.Country);
                MapProperty(1, x => x.Confirmed);
                MapProperty(2, x => x.Deaths);
                MapProperty(3, x => x.Recovered);
                MapProperty(4, x => x.Active);
                MapProperty(5, x => x.NewCases);
                MapProperty(6, x => x.NewDeaths);
                MapProperty(7, x => x.NewRecovered);
                MapProperty(8, x => x.DeathsBy100Cases);
                MapProperty(9, x => x.RecoveredBy100Cases);
                MapProperty(10, x => x.DeathsBy100Recovered);
                MapProperty(11, x => x.ConfirmedLastWeek);
                MapProperty(12, x => x.OneWeekChange);
                MapProperty(13, x => x.OneWeekPercentIncrease);
                MapProperty(14, x => x.WHORegion);


            }
        }

    }



   
}

using BusinessEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class Stocks
    {
        public void GetLatestStockValues() {
            List<StocksEntity> _stocks = GetStocks();

            foreach(var eachStock in _stocks) {
                LoadData(eachStock.Symbol);
            }
        }

        private List<StocksEntity> GetStocks()
        {
            throw new NotImplementedException();
        }

        private void LoadData(string quote)
        {
            StockValues _stockValues = GetResponse(quote);

            List<StocksEntity> lstStocksData = FormatStockValues(_stockValues);

            UpdateStockValue(lstStocksData[0]);

            var dividends = lstStocksData.Where(r => r.dividendamount > 0);
            if (dividends != null && dividends.Count() > 0) {
                UpdateDividends(dividends.ToList());
            }
        }

        private List<StocksEntity> FormatStockValues(StockValues stockValues)
        {
            List<StocksEntity> result = new List<StocksEntity>();

            if (stockValues != null)
            {
                foreach (var item in stockValues.TimeSeriesDaily)
                {
                    result.Add(new StocksEntity()
                    {
                        Symbol = stockValues.MetaData.Symbol,
                        Date = DateTime.Parse(item.Key),
                        open = item.Value.open,
                        low = item.Value.low,
                        close = item.Value.close,
                        high = item.Value.high,
                        dividendamount = item.Value.dividendamount,
                        volume = item.Value.volume,
                        adjustedclose = item.Value.adjustedclose
                    });
                }
            }
            return result.OrderByDescending(r=> r.Date).ToList();
        }

        private StockValues GetResponse(string quote)
        {
            var client = new HttpClient();
            string _address = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + (string.IsNullOrWhiteSpace(quote) ? "MSFT" : quote) + "&apikey=UZF61T3SJI96RSNU";
            HttpResponseMessage response = client.GetAsync(_address).Result;
            StockValues returnobj = null;
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(result))
                {
                    result = result.Replace("7. dividend amount", "dividendamount")
                                    .Replace("6. volume", "volume")
                                    .Replace("4. close", "close")
                                    .Replace("3. low", "low")
                                    .Replace("2. high", "high")
                                    .Replace("1. open", "open")
                                    .Replace("2. Symbol", "Symbol")
                                    .Replace("Time Series (Daily)", "TimeSeriesDaily")
                                    .Replace("Meta Data", "MetaData")
                                    .Replace("adjusted close", "adjustedclose")
                                    .Replace("1. ", "").Replace("2. ", "").Replace("3. ", "").Replace("4. ", "").Replace("5. ", "").Replace("6. ", "").Replace("7. ", "").Replace("8. ", "").Replace("9. ", "").Replace("10. ", "");

                    returnobj = JsonConvert.DeserializeObject<StockValues>(result);
                }
            }
            return returnobj;
        }

        private void UpdateDividends(List<StocksEntity> list)
        {
            throw new NotImplementedException();
        }

        private void UpdateStockValue(StocksEntity stocksEntity)
        {
            throw new NotImplementedException();
        }

    }
}

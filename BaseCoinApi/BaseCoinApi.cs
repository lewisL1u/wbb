using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCoinApis
{
    public class BaseCoinApi
    {

        private String secret_key;

        private String accessId;

        private String url_prex;

        public BaseCoinApi(String url_prex, String accessId, String secret_key)
        {
            this.accessId = accessId;
            this.secret_key = secret_key;
            this.url_prex = url_prex;
        }

        public BaseCoinApi(String url_prex)
        {
            this.url_prex = url_prex;
        }

        /**
         * 行情URL
         */
        public string TICKER_URL = "market/ticker";

        /**
         * 市场深度URL
         */
        public string DEPTH_URL = "market/depth";

        /**
         * 历史交易信息URL
         */
        public string TRADES_URL = "market/deals";

        /**
         * 历史交易信息URL
         */
        public string KLINE_URL = "market/kline";

        /**
         *  批量获取用户订单URL
         */
        public string BALANCE_URL = "balance/";

        public string PENDING_ORDER_URL = "order/pending";

        public string FINISHED_ORDER_URL = "order/finished";
        /**
         * 下限价单
         */
        public string PUT_LIMIT_URL = "order/limit";

        /**
         * 下限价单
         */
        public string PUT_MARKET_URL = "order/market";

        /**
         *  撤销订单URL
         */
        public string CANCEL_ORDER_URL = "order/pending";

        public enum HTTP_METHOD
        {
            GET, POST, DELETE
        }

        public enum MARKET
        {
            ETHCNY,
            ETHBTC,
            BTCCNY,
            BTCBCH
        }

        public enum ORDER_TYPE
        {
            sell, buy
        }

        public string doRequest(String url, Dictionary<string, string> paramMap, HTTP_METHOD method)
        {
            if (paramMap == null)
            {
                paramMap = new Dictionary<string, string>();
            }
            paramMap.Add("access_id", this.accessId);
            paramMap.Add("tonce", TimeHelper.ConvertDateTimeToInt(DateTime.Now).ToString());
            String authorization = MD5Util.buildMysignV1(paramMap, this.secret_key);
            HttpUtilManager httpUtil = HttpUtilManager.getInstance();
            switch (method)
            {
                case HTTP_METHOD.GET:
                    return httpUtil.requestHttpGet(url_prex, url, paramMap, authorization);

                case HTTP_METHOD.POST:
                    return httpUtil.requestHttpPost(url_prex, url, paramMap, authorization);

                case HTTP_METHOD.DELETE:
                    return httpUtil.requestHttpDelete(url_prex, url, paramMap, authorization);

                default:
                    return httpUtil.requestHttpGet(url_prex, url, paramMap, authorization);
            }
        }

        public String ticker(MARKET market)
        {

            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            return doRequest(TICKER_URL, param, HTTP_METHOD.GET);
        }

        public String depth(MARKET market, String merge, Int32 limit)
        {

            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("merge", merge);
            param.Add("limit", Convert.ToString(limit));
            return doRequest(DEPTH_URL, param, HTTP_METHOD.GET);
        }

        public String trades(MARKET market)
        {

            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            return doRequest(TRADES_URL, param, HTTP_METHOD.GET);
        }

        public String kline(MARKET market, String type)
        {

            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("type", type);
            return doRequest(KLINE_URL, param, HTTP_METHOD.GET);
        }

        public String account()
        {
            return doRequest(BALANCE_URL, null, HTTP_METHOD.GET);
        }

        public String pendingOrder(MARKET market, int page, int account)
        {
            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("page", Convert.ToString(page));
            param.Add("account", Convert.ToString(account));
            return doRequest(PENDING_ORDER_URL, param, HTTP_METHOD.GET);
        }

        public String finishedOrder(MARKET market, String page, String account)
        {
            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("page", page);
            param.Add("account", account);
            return doRequest(FINISHED_ORDER_URL, param, HTTP_METHOD.GET);
        }

        public String putLimitOrder(MARKET market, ORDER_TYPE type, float amount, float price)
        {
            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("type", type.ToString());
            param.Add("amount", Convert.ToString(amount));
            param.Add("price", Convert.ToString(price));
            return doRequest(PUT_LIMIT_URL, param, HTTP_METHOD.POST);
        }

        public String putMarketOrder(MARKET market, ORDER_TYPE type, float amount)
        {
            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("type", type.ToString());
            param.Add("amount", Convert.ToString(amount));
            return doRequest(PUT_MARKET_URL, param, HTTP_METHOD.POST);
        }

        public String cancelOrder(MARKET market, String orderID)
        {
            Dictionary<String, String> param = new Dictionary<String, String>();
            param.Add("market", market.ToString());
            param.Add("order_id", orderID);
            return doRequest(CANCEL_ORDER_URL, param, HTTP_METHOD.DELETE);
        }
    }
}
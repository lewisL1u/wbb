using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCoinApis
{
    public class CoinExApi : BaseCoinApi
    {

        private String secret_key1;

        private String accessId1;

        private String url_prex1;

        public CoinExApi(String url_prex, String accessId, String secret_key)
            : base(url_prex, accessId, secret_key)
        {
            this.url_prex1 = url_prex;
            this.secret_key1 = secret_key;
            this.accessId1 = accessId;
        }

        public CoinExApi(String url_prex)
            : base(url_prex)
        {
            this.url_prex1 = url_prex;

        }
    }
}

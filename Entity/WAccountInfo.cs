using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class WAccountInfo
    {
        private long _ID;

        private long exchangeID;

        private string exchangeCode;

        private string exchangeName;

        private long accountID;

        private long accountType;

        private long accountStatus;

        private Int32 expiredDate;

        private string _AccountName;

        private string _ApiKey;

        private string _ApiSecret;

        private DateTime _UpdateTime;

        public Int32 ExpiredDate
        {
            get { return expiredDate; }
            set { expiredDate = value; }
        }

        public long AccountStatus
        {
            get { return accountStatus; }
            set { accountStatus = value; }
        }

        public long AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        public long AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        public string ExchangeName
        {
            get { return exchangeName; }
            set { exchangeName = value; }
        }

        public string ExchangeCode
        {
            get { return exchangeCode; }
            set { exchangeCode = value; }
        }

        public long ExchangeID
        {
            get { return exchangeID; }
            set { exchangeID = value; }
        }



        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return _AccountName;
            }
            set
            {
                _AccountName = value;
            }
        }

        public string ApiKey
        {
            get
            {
                return _ApiKey;
            }
            set
            {
                _ApiKey = value;
            }
        }

        public string ApiSecret
        {
            get
            {
                return _ApiSecret;
            }
            set
            {
                _ApiSecret = value;
            }
        }

        public string DisplayApiSecret
        {
            get
            {
                if (_ApiSecret.Length > 4)
                {
                    return "********" + _ApiSecret.Substring(0, 4);
                }
                return _ApiSecret;
            }
        }

        public DateTime UpdateTime
        {
            get
            {
                return _UpdateTime;
            }
            set
            {
                _UpdateTime = value;
            }
        }
    }
}

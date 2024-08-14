using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.Test.Infra.BaseTest
{
    public static class EnumList
    {

        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
        public enum ErrorCode
        {
            //Transaction error list
            TimeOutTransaction = 10003,
            AmountTransactionMissing = 20001,
            AmountCreditTooHigh = 20002,
            AmountNegative = 20004,
            AmountIncorectFormat = 20005,
            BusinessIdEmpty = 20006,
            BusinessIdIncorrect = 20007,
            ExternalIdIncorect = 20009,
            PosIdIncorrect = 20010,
            GenericParam =20011,
            ExternalIdNotFound=20013,
            TransactionIdNotFound = 20014,
            RefundAmountHigherOriginalTransaction = 40001,
            Refund_BaseRefundTransactionNotAllowed = 40012,

            //Authontication
            UserOrPasswordIncorrect = 105
        }
        public enum TransactionType
        {
            DebitTransaction = 101,
            RefundTransaction = 103,
            RemoteTransfare =2
        }
        public enum BuyerMethod
        {
            BluetoothAndRefundTransaction =0,
            QrCode =1,
            PayByCode =2
        }

        public enum BuyerDataEnum
        {
            CurrentBalance,
            BuyerRegistrationStatus,
            UserId,
            PhoneNumber,
            BankAccountId
        }
        public enum StatusCode
        {
            Pending = 0,
            Success = 1,
            Canceled = 2,
            Rejected = 3,
            Requested = 7,
            Limited = 1
        }
        public enum TabName
        {
            Name1 = 1,
            [Description("Transactions")]
            Transactions,
            [Description("Ext. Transactions")]
            ExTransaction,
            [Description("Monitors & Alerts")]
            MonitoAndAlert,
            [Description("Monitors - Transactions")]
            MonitorsTransactions,
            [Description("Monitors - Users")]
            MonitorUsers,
            [Description("Monitors - Balances")]
            MonitorBalance
        }
        public static class Categories
        {
            //Test categoty
            public const string Api = nameof(Api);
            public const string UiWeb = nameof(UiWeb);
            public const string MobileAndroid = nameof(MobileAndroid);
            public const string LoadTest = nameof(LoadTest);

        }
        public static class TestLevel 
        {
            //Test level
            public const string Level_1 = nameof(Level_1);
          

        }
        public const string Category = nameof(Category);
    
    }
}

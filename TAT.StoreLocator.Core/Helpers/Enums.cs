using System.Text.Json.Serialization;

namespace TAT.StoreLocator.Core.Helpers
{
    public class Enums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum EDayOfWeek
        {
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum EUserStatus
        {
            Block,
            Active,
            InActive,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ERoleType
        {
            Admin,
            StoreOwner,
            Customer
        }
        public enum EGenderType
        {
            NotInformation,
            Men,
            Women,
            Other,

        }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum EOrderBy
        {
            HighToLow,
            LowToHigh,
            Latest,
            Oldest,
            Default
        }

        #region UploadFile properties
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum EUploadFileStatus
        {
            Delete,
            Active,
        }
        #endregion

        #region TagType 
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ETagType
        {
            Category,
            Product,
            Post

        }
        #endregion

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum EReviewStatus
        {
            Pending = 0,
            Approved = 1,
        }

    }
}

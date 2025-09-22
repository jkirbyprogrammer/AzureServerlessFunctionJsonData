namespace AzureFunctionJsonData
{
    internal class FileNameHelper
    {
        private readonly string _type = "ussec";
        private readonly string _year = "2025";

        public FileNameHelper(string type, string year)
        {
            if (!String.IsNullOrEmpty(type))
                _type = type;
            if (!String.IsNullOrEmpty(year))
                _year = year;
        }

        public string GetCountyFileName()
        {
            if (_type == "ussec")
                return _year + "CountyUsSecLayer.json";
            else
                return _year + "CountyPresLayer.json";
        }

        public string GetStateFileName()
        {
            if (_type == "ussec")
                return _year + "StateUsSecLayer.json";
            else
                return _year + "StatePresLayer.json";
        }

    }
}

using System.ComponentModel;
using System.Reflection;

namespace DataEntity.Pagination
{
    public class PagingRequest
    {
        public int PageIndex { get; set; } = 1;

        private PageSizeEnm _pageSize = PageSizeEnm.SEPULUH;
        public int PageSize
        {
            get => (int)_pageSize;
            set
            {
                if (Enum.IsDefined(typeof(PageSizeEnm), value)) _pageSize = (PageSizeEnm)value;
                else _pageSize = PageSizeEnm.SEPULUH;
            }
        }

        public List<SearchCriteria> Search { get; set; } = [];
        public List<SortCriteria> Sort { get; set; } = [];

        public (bool, Dictionary<string, object>) ValidateModel<TModel>() where TModel : class
        {
            bool isValid = false;
            Dictionary<string, object> errorList = [];
            PropertyInfo prop;

            // return true kalau param search null
            if (Search == null) return (true, errorList);

            // Validasi search property merupakan bagian dari model
            foreach (var itemSearch in Search)
            {
                prop = typeof(TModel).GetProperty(itemSearch.PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                var searchErrorList = SearchValidate(itemSearch, prop);
                if (searchErrorList.Count > 0) errorList.Add("Search", searchErrorList);
            }

            // Validasi sort property merupakan bagian dari model
            List<string> sortErrorList = [];

            if (Sort.Count > 2) sortErrorList.Add("Maximum number of Sort items is 2");
            if (Sort.GroupBy(x => x.PropertyNameOrder).Any(x => x.Count() > 1)) sortErrorList.Add("Sort items must be unique");

            // return list error, check point validasi
            if (sortErrorList.Count > 0) return (isValid, errorList);

            Sort.ForEach(itemSort =>
            {
                prop = typeof(TModel).GetProperty(itemSort.PropertyNameOrder, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (string.IsNullOrWhiteSpace(itemSort.PropertyNameOrder)) sortErrorList.Add("The PropertyNameOrder field is required");
                else if (prop is null) sortErrorList.Add($"Unknown property {itemSort.PropertyNameOrder}");
            });

            if (sortErrorList.Count > 0) errorList.Add("Sort", sortErrorList);

            return (errorList.Count == 0, errorList);
        }

        static bool CanConvert(string value, Type type)
        {
            bool result = false;

            if (string.IsNullOrEmpty(value) || type == null) return result;

            TypeConverter conv = TypeDescriptor.GetConverter(type);

            if (type.FullName.Contains("Date"))
            {
                result = DateTime.TryParseExact(value, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
            }
            else if (conv.CanConvertFrom(typeof(string)))
            {
                try
                {
                    conv.ConvertFrom(value);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        static List<string> SearchValidate(SearchCriteria _search, PropertyInfo prop)
        {
            List<string> searchErrorList = [];

            if (string.IsNullOrWhiteSpace(_search.PropertyName)) searchErrorList.Add("The PropertyName field is required");
            if (string.IsNullOrWhiteSpace(_search.Operator.ToString())) searchErrorList.Add("The Operator field is required");

            // return errorList, check point validasi
            if (searchErrorList.Count > 0) return searchErrorList;

            if (prop is null) searchErrorList.Add($"Unknown property {_search.PropertyName}");
            if (!Enum.TryParse(typeof(OperatorEnm), _search.Operator, true, out _)) searchErrorList.Add("The Operator field is invalid");

            // return errorList, check point validasi
            if (searchErrorList.Count > 0) return searchErrorList;

            // jika operator between, startValue dan endValue harus ada
            if (_search.Operator.Equals("between", StringComparison.OrdinalIgnoreCase)
                || _search.Operator.Equals("NotBetween", StringComparison.OrdinalIgnoreCase))
                searchErrorList.AddRange(BetweenValueValidate(_search, prop));
            else searchErrorList.AddRange(ValueValidate(_search, prop));

            return searchErrorList;
        }

        static List<string> BetweenValueValidate(SearchCriteria _search, PropertyInfo prop)
        {
            List<string> searchErrorList = [];
            if (string.IsNullOrWhiteSpace(_search.StartValue)) searchErrorList.Add("The StartValue field is required");
            if (string.IsNullOrWhiteSpace(_search.EndValue)) searchErrorList.Add("The EndValue field is required");

            // return errorList, check point validasi
            if (searchErrorList.Count > 0) return searchErrorList;

            if (!CanConvert(_search.StartValue, prop?.PropertyType))
                searchErrorList.Add($"[{_search.StartValue}] is invalid value of type [{prop?.PropertyType.Name}]");

            if (!CanConvert(_search.EndValue, prop?.PropertyType))
                searchErrorList.Add($"[{_search.EndValue}] is invalid value of type [{prop?.PropertyType.Name}]");

            return searchErrorList;
        }

        static List<string> ValueValidate(SearchCriteria _search, PropertyInfo prop)
        {
            List<string> searchErrorList = [];

            if (string.IsNullOrWhiteSpace(_search.Value)) searchErrorList.Add("The value field is required");

            // return errorList, check point validasi
            if (searchErrorList.Count > 0) return searchErrorList;

            if (!CanConvert(_search.Value, prop?.PropertyType))
                searchErrorList.Add($"[{_search.Value}] is invalid value of type [{prop?.PropertyType.Name}]");

            return searchErrorList;
        }

    }
}
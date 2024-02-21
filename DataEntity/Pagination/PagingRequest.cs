using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DataEntity.Pagination
{
    public class PagingRequest
    {
        public int PageIndex { get; set; } = 1;

        private PageSizeEnum _pageSize { get; set; } = PageSizeEnum.SEPULUH;
        public int PageSize
        {
            get => (int)_pageSize;
            set
            {
                if (Enum.IsDefined(typeof(PageSizeEnum), value)) _pageSize = (PageSizeEnum)value;
                else _pageSize = PageSizeEnum.SEPULUH;
            }
        }

        public List<SearchCriteria> Search { get; set; } = [];
        public List<SortCriteria> Sort { get; set; } = [];

        public (bool, Dictionary<string, object>) ValidateModel<TModel>() where TModel : class
        {
            bool isValid = false;
            Dictionary<string, object> errorList = [];
            PropertyInfo prop;

            // Validasi search property merupakan bagian dari model
            if (Search != null)
            {
                foreach (var itemSearch in Search)
                {
                    prop = typeof(TModel).GetProperty(itemSearch.PropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var searchErrorList = SearchValidate(itemSearch, prop);
                    if (searchErrorList.Count > 0) errorList.Add("Search", searchErrorList);
                }
            }

            // Validasi sort property merupakan bagian dari model
            if (Sort != null)
            {
                List<string> sortErrorList = [];

                if (Sort.Count > 2) sortErrorList.Add("Maximum number of Sort items is 2");
                if (Sort.GroupBy(x => x.PropertyNameOrder).Any(x => x.Count() > 1)) sortErrorList.Add("Sort items must be unique");

                if (sortErrorList.Count == 0)
                {
                    foreach (var itemSort in Sort)
                    {
                        prop = typeof(TModel).GetProperty(itemSort.PropertyNameOrder, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (string.IsNullOrWhiteSpace(itemSort.PropertyNameOrder)) sortErrorList.Add("The PropertyNameOrder field is required");
                        else if (prop is null) sortErrorList.Add($"Unknown property {itemSort.PropertyNameOrder}");
                    }
                }

                if (sortErrorList.Count > 0) errorList.Add("Sort", sortErrorList);
            }

            if (errorList.Count == 0) isValid = true;
            return (isValid, errorList);
        }

        public static List<TModel> GetData<TModel>()
        {
            var _pageData = new List<TModel>();

            return _pageData;
        }

        static bool CanConvert(string value, Type type)
        {
            bool result = false;

            if (string.IsNullOrEmpty(value) || type == null) return result;

            TypeConverter conv = TypeDescriptor.GetConverter(type);

            if (type.Name == "DateTime")
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
            else
            {
                if (prop is null) searchErrorList.Add($"Unknown property {_search.PropertyName}");
                else
                {
                    if (string.IsNullOrWhiteSpace(_search.Operator)) searchErrorList.Add("The Operator field is required");
                    else
                    {
                        if (_search.Operator == "between")
                        {
                            if (string.IsNullOrWhiteSpace(_search.StartValue)) searchErrorList.Add("The StartValue field is required");
                            if (string.IsNullOrWhiteSpace(_search.EndValue)) searchErrorList.Add("The EndValue field is required");

                            if (!string.IsNullOrWhiteSpace(_search.StartValue) && !string.IsNullOrWhiteSpace(_search.EndValue))
                            {
                                if (!CanConvert(_search.StartValue, prop.PropertyType))
                                    searchErrorList.Add($"[{_search.StartValue}] is invalid value of type [{prop.PropertyType.Name}]");

                                if (!CanConvert(_search.EndValue, prop.PropertyType))
                                    searchErrorList.Add($"[{_search.EndValue}] is invalid value of type [{prop.PropertyType.Name}]");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(_search.Value)) searchErrorList.Add("The value field is required");
                            else
                            {
                                if (!CanConvert(_search.Value, prop.PropertyType))
                                    searchErrorList.Add($"[{_search.Value}] is invalid value of type [{prop.PropertyType.Name}]");
                            }
                        }
                    }
                }
            }
            return searchErrorList;
        }
    }
}
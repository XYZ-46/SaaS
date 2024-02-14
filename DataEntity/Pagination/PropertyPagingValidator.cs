using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DataEntity.Pagination
{
    public static class PropertyPagingValidator
    {
        public static List<ModelValidationResult> PagingPropertyValidate<TModel>(this PagingRequest pagingRequest, TModel _tModel) where TModel : class
        {
            var modelErrorList = new List<ModelValidationResult>();

            foreach (var sortList in pagingRequest.Sort)
            {
                var asdf = SortPropertyValidate(sortList, _tModel);
            }

            return modelErrorList;
        }
        private static List<string> SearchPropertyValidate<TModel>(this SearchCriteria searchCriteria, TModel _tModel) where TModel : class
        {
            var errorList = new List<string>();
            if (!ValidateProperty(searchCriteria.PropertyName, _tModel))
                errorList.Add($"Invalid property name {searchCriteria.PropertyName}");

            return errorList;
        }

        private static bool ValidateProperty<TModel>(string propertyName, TModel _tModel) where TModel : class =>
                _tModel.GetType().GetProperties().ToList().Exists(x => x.Name == propertyName);

        private static void GetType<TModel>(string propertyName, TModel _tModel) where TModel : class
        {

        }

        private static List<string> SortPropertyValidate<TModel>(this SortCriteria sortCriteria, TModel _tModel) where TModel : class
        {
            var errorList = new List<string>();
            if (!ValidateProperty(sortCriteria.PropertyNameOrder, _tModel))
                errorList.Add($"Invalid property name {sortCriteria.PropertyNameOrder}");

            return errorList;
        }
    }
}

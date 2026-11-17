using UserManagementApi.Enums;

namespace UserManagementApi.Validation;

public class DateFilteredRequestValidator
{
    public bool Validate(DateFilterType? dateFilterType, DateTime from, DateTime? to)
    {
        if (!dateFilterType.HasValue)
        {
            return false;
        }

        else if (dateFilterType != DateFilterType.Created && dateFilterType != DateFilterType.Updated)
        {
            return false;
        }

        else if (to.HasValue && from > to.Value)
        {
            return false;
        }

        return true;
    }
}

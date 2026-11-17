using UserManagementApi.Enums;

namespace UserManagementApi.Web.Models;

public class DateFilteredRequest
{
    public DateFilterType? DateFilterType { get; set; }
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
}

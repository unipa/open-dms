using Web.Model.Customize;

namespace Web.BL.Interface
{
    public interface ICustomizeLeftPanelBL
    {
        Task<List<OptionPage>> GetOptionPages(string UserId);
        Task<List<OptionPage>> GetOptionPagesForUser(string UserId);

    }
}
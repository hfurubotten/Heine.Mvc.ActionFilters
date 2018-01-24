namespace Heine.Mvc.ActionFilters.Interfaces
{
    public interface IAuditable
    {
        void Audit(string userId);
    }
}
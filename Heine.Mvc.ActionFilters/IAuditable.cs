namespace Heine.Mvc.ActionFilters
{
    public interface IAuditable
    {
        void Audit(string userId);
    }
}
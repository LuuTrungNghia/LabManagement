public interface ILabService
{
    Task<Lab> GetLabAsync();
    Task<Lab> UpdateLabAsync(Lab lab);
}
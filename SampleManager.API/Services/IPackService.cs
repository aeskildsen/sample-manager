namespace SampleManager.API.Services;
using SampleManager.API.Models;

public interface IPackService
{
    SamplePack? GetPack(string packSlug);
    IEnumerable<SamplePack> GetAllPacks();
}

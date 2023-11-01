namespace IHelperServices;

public interface IHelperService
{
    Task<string> GetNameAsync();
    int[] GetNumbers();
}
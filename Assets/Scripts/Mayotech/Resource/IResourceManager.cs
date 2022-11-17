namespace Mayotech.Resources
{
    public interface IResourceManager
    {
        LocalResource GetResource(string resourceType);
        void GainResource(string resourceType, int amount);
        bool ConsumeResource(string resourceType, int amount, bool checkResource = true);
        bool PayPrice(Price price);
    }
}
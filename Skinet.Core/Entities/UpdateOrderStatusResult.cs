namespace Skinet.Core.Entities
{
    public enum UpdateOrderStatusResult
    {
        Success,
        OrderNotFound,
        InvalidStatusTransition,
        InvalidStatus,
        SaveFailed
    }
}

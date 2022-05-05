namespace AlbelliAPI.FunctionalTest.Routes;

public class OrderRoutes
{
    internal static readonly string Root = "api";

    internal static string SubmitOrder() => $"{Root}";

    internal static string GetOrder(string orderId) => $"{Root}/{orderId}";
}
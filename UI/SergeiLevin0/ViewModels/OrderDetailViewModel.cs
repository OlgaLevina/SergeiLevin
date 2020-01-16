namespace SergeiLevin0.ViewModels
{
    /// <summary>
    /// Полная модель-представления для представления корзины, 
    /// где есть область корзины и область оформления заказа
    /// </summary>
    public class OrderDetailsViewModel
    {
        public CartViewModel CartViewModel { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
    }
}

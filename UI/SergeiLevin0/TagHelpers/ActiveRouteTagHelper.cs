using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.TagHelpers
{
    //создаем си-шарп таг-хелпер с его содержимым
    //[HtmlTargetElement(Attributes = "имена атрибутов,через,запятую")]//в скобочках либо имя тега, либо атрибуты. Таким образом таг-хелпер можно ориентировать на другие html-элементы, т.е. он может связаться с html-элементом или его атрибутом
    [HtmlTargetElement(Attributes = AttributeName)]//используем наш собственный атрибут как константу
    public class ActiveRouteTagHelper : TagHelper // TagHelper обязательный суффикс имени класса. iActiveRoute - имя таг-хелпера. Можно использовать теперь как тег: <active-route></active-route>
    {
        //если таг-хелперу нужны дополнительне сервисы (логер, ипродактдата - запрашиваем через конструктор) 
        public const string AttributeName = "is-active-route";//наш собственный атрибут
        public const string IgnoreActionName = "ignore-action";//наш атрибут игнорирования
        [HtmlAttributeName("asp-action")]//связываем свойство с атрибутом, данные по которому должны сохраниться в свойстве
        public string Action { get; set; }
        [HtmlAttributeName("asp-controller")]//аналогично, как выше
        public string Controller { get; set; }
        private IDictionary<string, string> _RouteValues;//для хранения параметров маршрута
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]//для привязки исползуется либо атрибуты тега с asp-all-route-data либо asp-route-..... Все они попадут в наш словарь параметров
        public IDictionary<string, string> RouteValues
        {
            get => _RouteValues ?? (_RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));//если будет запрошен RouteValues, но он при этом еще не был создан, то он создается и при этом ключи не зависят от регистра
            set => _RouteValues = value;
        }
        [HtmlAttributeNotBound, ViewContext]//указываем, что это свойство не связано с разметкой и что явуляется вьюконтенстом, чтобы при создании таг-хелпера в это свойство автоматически записывался контенст таг-хелпера
        public ViewContext ViewContext { get; set; }//свой собственный визуальный  контектс таг-хелпера 

        public override void Process(TagHelperContext context, TagHelperOutput output)//переопределяем базовый элемент. В нем делается вся работа таг-хелпера для синхронных таг-хелперов. Для асинхронных - соответственно ProcessAsync/ В нашем случае асинхронный не нужен. 
        {
            //в качестве параметров передаются данные, формируемые средой исполнения в процессе рендеринга html-контента
            //TagHelperContext - в нем происходит процесс рендеринга, через него можно получить доступ к маршруту, запросу и др.с доп.инфо
            //TagHelperOutput - то куда рендериться наш штимл - с ним можно взаимодействовать дописывая или удаляя элементы
            var ignore_action = context.AllAttributes.ContainsName(IgnoreActionName); //.TryGetAttribute(IgnoreActionName, out _);
            //if (IsActive()) MakeActive(output);//проверяем является ли текущий элемент активным, если да - делаем активным. Без учета игронирования
            if (IsActive(ignore_action)) MakeActive(output);
            //!!!проблема с Блогом - видимо, из-за того, что для Блога, Хоум и Контакт один и тот же контролер, а для блога еще и игрон действия - блог с Хоум и Контакт всегда тоже выделяется - потом исправить!!!
            output.Attributes.RemoveAll(AttributeName);//стираем из разметки по завершении
            output.Attributes.RemoveAll(IgnoreActionName);
        }
        private bool IsActive(bool IgnoreAction)//добавляем атрибует игнорирования
        {
            var route_values = ViewContext.RouteData.Values;//извлекаем параметры маршрута
            var current_controller = route_values["Controller"].ToString();//извлекаем контроллер
            var current_action = route_values["Action"].ToString();//извлекаем дейсвтие
            const StringComparison ignore_case = StringComparison.OrdinalIgnoreCase;//задаем параметр сравнения строк, чтобы уменьшить кол-во кода
            if (!string.IsNullOrWhiteSpace(Controller) && !string.Equals(Controller, current_controller, ignore_case))  return false;//если контроллер не задан или не совпадает - игнор
            if (!IgnoreAction && !string.IsNullOrWhiteSpace(Action) && !string.Equals(Action, current_action, ignore_case)) return false;
            foreach (var (key, value) in RouteValues)
                if (!route_values.ContainsKey(key) || route_values[key].ToString() != value) return false;
            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var class_attribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (class_attribute is null) output.Attributes.Add(new TagHelperAttribute("class", "active"));
            else
            {
                if (class_attribute.Value.ToString().Contains("active")) return;
                output.Attributes.SetAttribute(
                    "class",
                    class_attribute.Value is null
                        ? "active"
                        : class_attribute.Value + " active"
                );
            }
        }

    }
}

Cart = {//создаем структуру содержащую "поля" и функции
    _properties: {
        getCartViewLink: "",//для сохранения метода вывода данных корзины
        addToCartLink: ""// для метода добавления в корзину
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);//сохраняем из объекта пропотис в _пропотис данные по методам
        Cart.initEvents();//подвешиваем кликовые события
    },

    initEvents: function () {
        $(".add-to-cart").click(Cart.addToCart);
    },

    addToCart: function (event) {
        event.preventDefault();//отключаем стандартную реакцию
        var button = $(this);
        var id = button.data("id");
        //$.get(Cart._properties.addToCartLink + "/" + id)
        $.get(`${Cart._properties.addToCartLink}/${id}`)//другой метод записи
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("addToCart fail"); });
    },

    showToolTip: function (button) {
        button.tooltip({ title: "Добавлено в корзину" }).tooltip("show");
        setTimeout(function () {
            button.tooltip("destroy");
        }, 500);
    },

    refreshCartView: function () {
        var container = $("#cart-container");//ищем на странице айди=cart-container и заполняем его содержимое
        $.get(Cart._properties.getCartViewLink)
            .done(function (cart_html) {
                container.html(cart_html);
            })
            .fail(function () { console.log("refreshCartView fail"); });
    }
} 
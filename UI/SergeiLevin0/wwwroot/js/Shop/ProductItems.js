ProductItems = {
    _properties: {
        getUrl: ""
    },

    init: properties => {
        $.extend(ProductItems._properties, properties);
        $(".pagination li a").click(ProductItems.clickOnPage);
    },

    clickOnPage: function(event) {
        event.preventDefault();
        const button = $(this);

        if (button.prop("href").length > 0) {
            var page = button.data("page");
            const container = $("#catalog-items-container");

            container.LoadingOverlay("show");

            const data = button.data();

            let query = "";
            for (let key in data) {
                if (data.hasOwnProperty(key))
                    query += `${key}=${data[key]}`; //то что query += key + "=" + data[key];
            }

            $.get(`${ProductItems._properties.getUrl}?${query}`)//формируем запрос к серверу
                .done(html => {//если все в порядке
                    container.html(html);
                    container.LoadingOverlay("hide");//отображаем полученную от сервера разметку

                    $(".pagination li").removeClass("active");//у всех обнуляем активность
                    $(".pagination li a").prop("href", "#");
                    $(`.pagination li a[data-page=${page}]`)//восстанавливаем активность к правильной кнопки
                        .removeAttr("href")
                        .parent().addClass("active");
                })
                .fail(() => {//если что-то пошло не так
                    container.LoadingOverlay("hide");
                    console.log("clickOnPage getItems error");
                });
        }
    }
} 
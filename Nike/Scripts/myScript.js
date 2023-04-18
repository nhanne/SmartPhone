$(document).ready(function () {
    // hover 
    $(".category-item").hover(
        function () {
            $(this).addClass("category-item--active"); // thêm class "hover" khi hover vào
        },
        function () {
            $(this).removeClass("category-item--active"); // loại bỏ class "hover" khi hover ra
        }
    );
    // Thêm class active
    var currentUrl = window.location.href;
    $(".menu li a.category-link").each(function () {
        if ($(this).attr("href").indexOf("?sort=@item.CatalogName.ToLower()") !== -1) {
            $(this).parent().addClass("category-item--active");
        }
    });

});
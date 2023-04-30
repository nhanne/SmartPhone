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
  // Thêm class active vào category
  const currentUrl = window.location.href;
  const getNameCate = currentUrl.split("?sort=")?.[1];
  console.log(getNameCate);
  if (getNameCate) {
    const allCateDom = document.querySelectorAll(".category-item");
    let activeCate;
    for (var i = 0; i < allCateDom.length; i++) {
      if (allCateDom[i].textContent.trim() === getNameCate) {
        activeCate = allCateDom[i];
        break;
      }
    }
    if (activeCate) {
      activeCate.classList.add("category-item-click--active");
    }

    // đổi btn filter
    const allfilterBtn = document.querySelectorAll(".home-filter__btn");
    let activeFilter;
    for (var i = 0; i < allfilterBtn.length; i++) {
      if (
        allfilterBtn[i].getAttribute("href").split("?sort=")?.[1] ===
        getNameCate
      ) {
        activeFilter = allfilterBtn[i];
        break;
      }
    }
    if (activeFilter) {
      activeFilter.classList.add("btn--primary");
    }
  }
});

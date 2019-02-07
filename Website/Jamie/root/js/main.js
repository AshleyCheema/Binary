/* 
$(window).on('load', function () {;
    console.log('refresh');
    setTimeout(scroll, 0);

}); */

$('.slide-show').owlCarousel({
    autoplay: true,
    items: 1,
    loop: true,
    navigation: true,
    slideSpeed: 300,
    paginationSpeed: 400,
    singleItem: true,
    autoHeight: true
});

$('.meet-carousel').owlCarousel({
    loop: true,
    autoplay: true,
    margin: 10,
    singleItem: false,
    slideSpeed: 500,
    paginationSpeed: 200,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 3
        },
        1000: {
            items: 4
        }
    }
})

var swiper = new Swiper('.blog-slider', {
    spaceBetween: 30,
    effect: 'fade',
    loop: true,
    mousewheel: {
        invert: false,
    },
    // autoHeight: true,
    pagination: {
        el: '.blog-slider__pagination',
        clickable: true,
    }
});

/* $('#start').click(function () {
    $('html, body').animate({
        scrollTop: $("#navbar").offset().top
    }, 300);
    setTimeout(deleteSpace, 300)


});

function deleteSpace() {
    console.log('deleted');
    $('#space').hide();
    $('#start').hide();
    $('html, body').animate({
        scrollTop: $("#navbar").offset().top
    }, 0);
}

function scroll() {
    $('html, body').animate({
        scrollTop: $("#space").offset().top
    }, 0);
} */
$(window).on('load', function () {
    if ($(document).scrollTop() < 200) {
        $('#start').fadeIn();
    }

})

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
        100: {
            items: 1
        },
        425: {
            items: 1
        },
        768: {
            items: 2
        },
        1024: {
            items: 3
        },
        1440: {
            items: 6
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

$('#start').click(function () {
    $('html, body').animate({
        scrollTop: $("#navbar").offset().top
    }, 300);
    /*setTimeout(deleteSpace, 300)*/


});

function deleteSpace() {
    console.log('deleted');
    $('#space').hide();

    $('html, body').animate({
        scrollTop: $("#navbar").offset().top
    }, 0);
}

/* function scroll() {
    $('html, body').animate({
        scrollTop: $("#space").offset().top
    }, 0);
} */

var $document = $(document),
    $element = $('#start'),
    className = 'hasScrolled';

$(document).scroll(function () {
    if ($(document).scrollTop() >= 200) {
        // user scrolled 50 pixels or more;
        // do stuff
        $('#start').fadeOut();
        $('#title-text').fadeOut();
    } else {
        $('#start').fadeIn();
        $('#title-text').fadeIn();
    }


});
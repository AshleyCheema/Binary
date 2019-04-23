$(document).ready(function () {

    resizeOnStart();

    ajaxRequstOnLoad();

});

$('#open-btn').click(function () {

    $('#content-panel').removeClass('mg-sm');
    $('#title-panel').removeClass('newWidth');

    $('#smallnav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#sidenav').show("slide", {
        direction: "left"
    }, 500);

});

$('#close-btn').click(function () {

    $('#content-panel').addClass('mg-sm');
    $('#title-panel').addClass('newWidth');

    $('#sidenav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#smallnav').show("slide", {
        direction: "left"
    }, 500);

});


function resizeOnStart() {
    //If window is smaller than 768 show the small navigation
    if ($(window).width() < 768) {
        $('#sidenav').hide();
        $('#smallnav').show();
        $('#content-panel').addClass('mg-sm');
        $('#title-panel').addClass('newWidth');
    } else { //else show the large navigation
        $('#smallnav').hide();
    }
}

function ajaxRequstOnLoad() {

    var url_string = window.location.href;
    var url = new URL(url_string);
    var page = url.searchParams.get("page");
    //console.log(page);

    //Remove existing class from all elements
    $(".nav-btn-active").removeClass('nav-btn-active');

    if (page != null) {
        //Ajax load the necessary page
        $("#content-panel").load('pages/' + page + '.php');
        $('.' + page).addClass('nav-btn-active');
    } else {
        //default load
        $("#content-panel").load('pages/profile.php');
        $('.profile').addClass('nav-btn-active');
    }

}

$('.cms-button').click(function () {
    event.preventDefault();
});
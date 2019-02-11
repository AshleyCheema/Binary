$(document).ready(function () {
    $('#smallnav').hide();
});

$('#open-btn').click(function () {

    $('#title-panel').removeClass('mg-sm');
    $('#title-panel').addClass('mg-lg');
    $('#content-panel').removeClass('mg-sm');
    $('#content-panel').addClass('mg-lg');

    $('#smallnav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#sidenav').show("slide", {
        direction: "left"
    }, 500);

});

$('#close-btn').click(function () {

    $('#title-panel').addClass('mg-sm');
    $('#content-panel').addClass('mg-sm');

    $('#sidenav').hide("slide", {
        direction: "left"
    }, 500).delay(1000);

    $('#smallnav').show("slide", {
        direction: "left"
    }, 500);

});
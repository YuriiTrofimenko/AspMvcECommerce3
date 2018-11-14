var $modal;
var $modalContainer;
var modalInstance;

var preloaderHide = function () {
    $('.preloader-wrapper').css('display', 'none');
}

var preloaderShow = function () {
    $('.preloader-wrapper').css('display', 'block');
}

var onSignIn = function (login) {

    $("a[href='#!home:out']").text('Signout (' + login + ')');
    $("a[href='#!home:out']").css('display', 'block');

    $("a[href='#!signin']").css('display', 'none');
    $("a[href='#!signup']").css('display', 'none');
}

var onSignOut = function () {

    $("a[href='#!home:out']").text('');
    $("a[href='#!home:out']").css('display', 'none');

    $("a[href='#!signin']").css('display', 'block');
    $("a[href='#!signup']").css('display', 'block');

    $("section#!admin").html('');
}

var getCurrentSectionName = function () {
    var hash = location.hash || "#!home";
    var re = /#!([-0-9A-Za-z]+)(\:(.+))?/;
    var match = re.exec(hash);
    return (match !== null) ? match[1] : "home";
}

//https://material.io/design/
//https://materializecss.com
$(document).ready(function () {
    
    $('.sidenav').sidenav();

    $('.modal').modal();
    $modal = $('.modal');
    $modal.find('.modal-footer > a').click(function () {
        $(this).attr('href', "#!" + getCurrentSectionName())
    });
    $modalContainer = $modal.find('.modal-content');
    modalInstance =
        M.Modal.getInstance($modal);
});

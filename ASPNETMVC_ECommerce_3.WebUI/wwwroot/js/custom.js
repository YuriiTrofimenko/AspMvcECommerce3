var $modal;
var $modalContainer;
var modalInstance;
var $cart = $('.cart');

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

    $(".cart").css('display', 'block');
}

var onSignOut = function () {

    $("a[href='#!home:out']").text('');
    $("a[href='#!home:out']").css('display', 'none');

    $("a[href='#!signin']").css('display', 'block');
    $("a[href='#!signup']").css('display', 'block');

    $(".cart").css('display', 'none');

    $("section#admin").html('');
    $("section#adminunit").html('');
    $modal.find('.modal-content').html('');
}

var getCurrentSectionName = function () {
    var hash = location.hash || "#!home";
    var re = /#!([-0-9A-Za-z]+)(\:(.+))?/;
    var match = re.exec(hash);
    return (match !== null) ? match[1] : "home";
}

var setModalOk = function () {

    var sectionName = getCurrentSectionName();
    $modal.find('.modal-footer > a').attr('href', "#!" + sectionName);
};

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

    $.get("api/auth/checkauth")
        .done(function (resp) {

            if (resp !== undefined) {
                if (resp.status === 'success' && resp.data !== null) {

                    onSignIn(resp.data);
                }
            } else {
                $modalContainer.html('Error: ' + resp.message);
                modalInstance.open();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $modalContainer.html(jqXHR);
            modalInstance.open();
        });
});

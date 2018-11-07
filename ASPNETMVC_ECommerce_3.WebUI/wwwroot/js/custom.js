var preloaderHide = function () {
    $('.preloader-wrapper').css('display', 'none');
}

var preloaderShow = function () {
    $('.preloader-wrapper').css('display', 'block');
}


//https://material.io/design/
//https://materializecss.com
$(document).ready(function () {
    $('.modal').modal();
    $('.sidenav').sidenav();
    /*var modalInstance =
        M.Modal.getInstance($('.modal'));
    modalInstance.open();*/
});

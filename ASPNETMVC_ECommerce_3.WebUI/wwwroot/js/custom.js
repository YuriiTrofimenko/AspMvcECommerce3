$.ajax({
    url: 'api/Demo/1'
    , method: 'GET'
    , cache: false
}).done(function (resp) {
    console.log(resp);
    /*$('#root').html(
        '<p>Name: ' + resp.login
        + ' (role: ' + resp.Role.name + ' )</p>'
    );*/
    $('#root').html(
        `<p>Name: ${resp.login} (role: ${resp.Role.name} )</p>`
    );
}).fail(function (xhr, status, message) {
    console.log(message);
});

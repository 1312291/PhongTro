function onSuccess(googleUser) {
    var profile = googleUser.getBasicProfile();
        gapi.client.load('plus', 'v1', function () {
            var request = gapi.client.plus.people.get({
                'userId': 'me'
            });
        //Display the user details
        request.execute(function (resp) {
            $('#gSignIn').slideUp('slow');
            var email = resp.emails[0].value;
            $.ajax({
                url: "http://timphongtro.apphb.com/api/users", 
                type: "POST",
                data: {
                    UserName: email,
                    Password: "loc123",
                    ConfirmPassword: "loc123",
                    Email: email,
                    FirstName: resp.name.givenName,
                    LastName: resp.name.familyName,
                    DateOfBirth: "11-09-1995",
                    Phone: "01665023317",
                    Roles: [
                        "Lodger"
                    ]
                },
                error: function(xhr,status,error){
                    if(xhr.status == 400){
                        var x = login(email,"loc123");
                    }
                },
                success: function(result){
                    var x = login(email,"loc123");
                }

            });
        });
    });
}

function login(email,password) {
    $.ajax({
        url: "http://timphongtro.apphb.com/api/users/login", 
        type: "POST",
        data: {
            username: email,
            password: password,
            grant_type: "password"
        },
        error: function(xhr,status,error){
            alert("Login Fail");
        },
        success: function(result){
            setCookie("access_token",result.access_token,365);
            window.open("http://localhost:8080/hotel","_self")
        }
    });
}

function onFailure(error) {
    alert(error);
}
function renderButton() {
    gapi.signin2.render('gSignIn', {
        'scope': 'profile email',
        'width': 290,
        'height': 50,
        'longtitle': true,
        'theme': 'dark',
        'onsuccess': onSuccess,
        'onfailure': onFailure
    });
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires="+d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
  
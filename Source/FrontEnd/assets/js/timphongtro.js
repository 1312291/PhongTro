angular.module('timphongtro', [])
.controller('timphongtroController', function ($window,$scope, $http) {

	$scope.host = "http://timphongtro.apphb.com/";
	$scope.title = "Send request to server, choose method please!";    

    $scope.dateDepart = "";
    $scope.dateReturn = "";
    $scope.listPostData = [];
    $scope.DestinationData = [];
    $scope.Flights = [];
    $scope.Seats = [];
    
    $scope.initPhongTroPage = function(){
        $scope.getAllPost();  
    };
    
    
    $scope.getAllPost = function () {
    var config = {headers:  {
            "Content-Type" : "application/json",
            "Authorization" : "Bearer " + getCookie("access_token")
        }
    };
	$http.get($scope.host + "api/posts"
        , config)
	  	.then(function(response) {
        //success   
            if(response.data.length == 0){
                alert("Hien tai khong co nha tro cho thue");
                return;
            }

            $scope.listPostData = response.data;
            for(var i=0;i<$scope.listPostData.length; i++){
                $scope.listPostData[i].lastUpdate = $scope.listPostData[i].lastUpdate.replace("T"," ");
                if($scope.listPostData[i].numberReviewers == 0)
                    $scope.listPostData[i].rate =  3.0;
                else
                    $scope.listPostData[i].rate = (0.1)*$scope.listPostData[i].totalPoint/$scope.listPostData[i].numberReviewers;
                if($scope.listPostData[i].images[0] == null)
                    $scope.listPostData[i].images[0] = "images/photos/8.jpg";
            }
       
  		},function(response) {
        //Second function handles error
            $scope.title = "Something wrong";
            alert($scope.title);
        });
	};

    $scope.detailsRoom = function (id) {
        setCookie("id_curr_room",id,1);
        window.open("http://localhost:8080/hotel/room-details.php","_self");
    };

    $scope.getRoom = function () {
    var config = {headers:  {
            "Content-Type" : "application/json",
            "Authorization" : "Bearer " + getCookie("access_token")
        }
    };
    $http.get($scope.host + "api/posts/" + getCookie("id_curr_room")
        , config)
        .then(function(response) {
        //success   
            $scope.currPost = response.data;
            $scope.currPost.lastUpdate = $scope.currPost.lastUpdate.replace("T"," ");
            if($scope.currPost.numberReviewers == 0)
                $scope.currPost.rate =  3.0;
            else
                $scope.currPost.rate = (0.1)*$scope.currPost.totalPoint/$scope.currPost.numberReviewers;
            if($scope.currPost.images[0] == null)
                $scope.currPost.images[0] = "images/photos/8.jpg";

            $scope.getListComment(getCookie("id_curr_room"));

        },function(response) {
        //Second function handles error
            $scope.title = "Something wrong";
            alert($scope.title);
        });
    };

    $scope.getListComment = function (id) {
    var config = {headers:  {
            "Content-Type" : "application/json",
            "Authorization" : "Bearer " + getCookie("access_token")
        }
    };
    $http.get($scope.host + "api/posts/" + id +"/comments"
        , config)
        .then(function(response) {
        //success   
            $scope.currPost.listComment = response.data;
        },function(response) {
        //Second function handles error
            $scope.title = "Something wrong";
            alert($scope.title);
        });
    };

    $scope.addComment = function () {
    var id =getCookie("id_curr_room");
    var config = {headers:  {
            "Content-Type" : "application/json",
            //"Authorization" : "Bearer " + getCookie("access_token")
            "Authorization": "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiJiYzI2ODY3Yi1lZmU0LTQ5NTktYTgwZS01YTYzNjVmNmI1N2YiLCJ1bmlxdWVfbmFtZSI6ImRja2hvYW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL2FjY2Vzc2NvbnRyb2xzZXJ2aWNlLzIwMTAvMDcvY2xhaW1zL2lkZW50aXR5cHJvdmlkZXIiOiJBU1AuTkVUIElkZW50aXR5IiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJjYTY3NWRjNy00NjA3LTRmYjYtYmNhNS1lZTNjOGY5OTYyOGQiLCJyb2xlIjpbIkFkbWluIiwiTGFuZGxvcmQiLCJMb2RnZXIiXSwiaXNzIjoiUGhvbmdUcm9BdXRoZW50aWNhdGlvbiIsImF1ZCI6IjQxNGUxZTM3YTM4ODRmNjhhYmM0M2Y3MjgzODM3ZmQxIiwiZXhwIjoxNDg0MTA2NzM5LCJuYmYiOjE0ODQwMjAzMzl9.GpCllUQ6X4MqB1hM0pYnt9cHqMJjNcsiBrd34S4xDcM"
        }
    };
    var data = {
        data:{
            content: document.getElementsByName("contentComment")[0].value
        }
    };

    $http.post($scope.host + "api/posts/" + id +"/comments", config,data)
        .then(function(response) {
        //success   
            alert("OK");
            $window.location.reload();
        },function(response) {
        //Second function handles error
            $scope.title = "Something wrong";
            alert($scope.title);
        });
    };


//login-------------------------------------------
    $scope.login = "Đăng Nhập";
    $scope.link_login = "login.php";
    if(getCookie("access_token")){
        $scope.login = "Tài Khoản";
        $scope.link_login = "account.php";
    }
    $scope.loginNormal = function(){
        var email = document.getElementsByName("user")[0].value;
        var password = document.getElementsByName("pass")[0].value;
        login(email,password);
    };
    
});

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
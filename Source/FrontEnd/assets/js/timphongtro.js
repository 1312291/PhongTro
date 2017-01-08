angular.module('formExample', [])
.controller('ExampleController', function ($scope, $http) {

	$scope.host = "http://timphongtro.apphb.com/";
	$scope.title = "Send request to server, choose method please!";
    $token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiJiYzI2ODY3Yi1lZmU0LTQ5NTktYTgwZS01YTYzNjVmNmI1N2YiLCJ1bmlxdWVfbmFtZSI6ImRja2hvYW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL2FjY2Vzc2NvbnRyb2xzZXJ2aWNlLzIwMTAvMDcvY2xhaW1zL2lkZW50aXR5cHJvdmlkZXIiOiJBU1AuTkVUIElkZW50aXR5IiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJjYTY3NWRjNy00NjA3LTRmYjYtYmNhNS1lZTNjOGY5OTYyOGQiLCJyb2xlIjpbIkFkbWluIiwiTGFuZGxvcmQiLCJMb2RnZXIiXSwiaXNzIjoiUGhvbmdUcm9BdXRoZW50aWNhdGlvbiIsImF1ZCI6IjQxNGUxZTM3YTM4ODRmNjhhYmM0M2Y3MjgzODM3ZmQxIiwiZXhwIjoxNDgzODY4ODU4LCJuYmYiOjE0ODM3ODI0NTh9.e330IRw39Qk1udzyOdiZx3RTINhqJqfr4m5muwZzOqE"

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
            "Authorization" : $token
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
    
    
    

});



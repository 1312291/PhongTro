<?php include 'header.php';?>

<div class="container">

<h2>Phòng Trọ</h2>


<!-- form -->

<div class="row">
  <div class="main-agileinfo" data-ng-init="initPhongTroPage()">
      <div class="animate-repeat" ng-repeat="phongtro in listPostData">
        <div class="col-sm-6  fadeInUp">
          <div class="rooms">
            <img src="{{phongtro.images[0]}}" class="img-responsive" style="max-height: 346px">
              <div class="info">
                <h3>{{phongtro.address}}</h3>
                <p> {{phongtro.description}}</p>
                <p class= "pull-left"> 
                  <li >Số người: {{phongtro.numberLodgers}} </li>
                  <li >Giá: {{phongtro.price}} VND </li>
                  <li >Rate: {{phongtro.rate}}/5.0 </li>
                  <li >Đăng ngày: {{phongtro.lastUpdate}}</li>
                </p>
                <button ng-click="detailsRoom(phongtro.id);" class="btn btn-default">Check Details</button>
              </div>
          </div>
        </div>
      </div>
  </div>

  <div class="text-center">
  <ul class="pagination">
  <li class="disabled"><a href="#">«</a></li>
  <li><a href="#">»</a></li>
  </ul>
  </div>


</div>
</div>
<?php include 'footer.php';?>


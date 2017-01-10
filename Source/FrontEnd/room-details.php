<?php include 'header.php';?>

<div class="container" data-ng-init="getRoom()">

<h1 class="title">{{currPost.address}}</h1>



 <!-- RoomDetails -->
            <div id="RoomDetails" class="carousel slide" data-ride="carousel">
                <div class="carousel-inner">
                  
                  <img src="{{currPost.images[0]}}" class="img-responsive" alt="slide"></img>
                
            </div>
  <!-- RoomCarousel-->

<div class="room-features spacer">
  <div class="row">
    <div class="col-sm-12 col-md-5"> 
    <p>{{currPost.description}}</p>
    <p>Đăng ngày: {{currPost.lastUpdate}}</p>
    </div>
    <div class="col-sm-6 col-md-3 amenitites"> 
    <h3>Quan tâm    
    <ul>
      <li >Số người: {{currPost.numberLodgers}} </li>
      <li >Rate: {{currPost.rate}}/5.0 </li>
    </ul>
    </div>  
    <div class="col-sm-3 col-md-2">
      <div class="size-price">Commnet<span>{{currPost.listComment.length}}</span></div>
    </div>
    <div class="col-sm-3 col-md-2">
      <div class="size-price">Giá<span>{{currPost.price}}VND</span></div>
    </div>

    <div class="container read-comment">
      <div class="row">
        <div class="col-sm-12">
          <h3>User Comment</h3>
        </div><!-- /col-sm-12 -->
      </div><!-- /row -->
        <div class="row" ng-repeat="comment in currPost.listComment">
          <div class="col-sm-1">
            <div class="thumbnail">
              <img class="img-responsive user-photo" src="https://ssl.gstatic.com/accounts/ui/avatar_2x.png">
            </div><!-- /thumbnail -->
          </div><!-- /col-sm-1 -->

        <div class="col-sm-5">
          <div class="panel panel-default">
            <div class="panel-heading">
              <strong>{{comment.firstName}}</strong> <span class="text-muted">{{comment.dateComment}}</span>
            </div>
            <div class="panel-body">{{comment.content}}</div><!-- /panel-body -->
          </div><!-- /panel panel-default -->
        </div><!-- /col-sm-5 -->
        </div><!-- /row -->

    </div><!-- /container -->
  </div>
</div>
                     
<div class="container write-comment">
  <div class="row">
    <h3>Viết bình luận</h3>
  </div>
    
    <div class="row">
    
    <div class="col-md-6">
                <div class="widget-area no-padding blank">
                <div class="status-upload">
                  <form>
                    <textarea name="contentComment" placeholder="Bình luận" ></textarea>
                    <button ng-click="addComment()" type="submit" class="btn btn-success green">Comment</button>
                  </form>
                </div><!-- Status Upload  -->
              </div><!-- Widget Area -->
            </div>
    </div>
</div>

</div>
</div>
<?php include 'footer.php';?>
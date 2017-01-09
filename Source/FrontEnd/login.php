<?php include 'header.php';?>
<?php
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST');?>

<meta name="google-signin-client_id" content="64175708162-c1ijo9p50aeph724nn3n75vqmtibtmg3.apps.googleusercontent.com">
<script src="https://apis.google.com/js/client:platform.js" async defer></script>

<div class="modal-dialog">
    <div class="loginmodal-container">
        <h1>Login to Your Account</h1><br>
            <input type="text" name="user" placeholder="Username"></input>
            <input type="password" name="pass" placeholder="Password"></input>
            <input ng-click="loginNormal()" type="submit" name="login" class="login loginmodal-submit" value="Login"></input>
            <div id="gSignIn"></div>

        
    </div>
</div>
<?php include 'footer.php';?>
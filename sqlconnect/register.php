<?php
	$connection = mysqli_connect('localhost', 'root', '', 'unityacces');

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$username = $_POST["user"];
	$password = $_POST["pass"];

	//Check if name exists

	$namecheckquerry = "SELECT username FROM users WHERE username='" . $username . "';";
	$namecheck = mysqli_query($connection, $namecheckquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($namecheck) > 0){
		echo "Error code #3 USERNAME IS TAKEN"; //Error code #3 = Username already exists
		exit();
	}

	//Add user to table
 	$salt = "\$5\$rounds=5000\$" . "steamedhams" . $username . "\$";
 	$hash = crypt($password, $salt);
 	$insertuserquerry = "INSERT INTO users (username, hash, salt) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "');";
 	mysqli_query($connection, $insertuserquerry) or die("Error code #4 INSERTION TO PLAYER INTO THE DATABASE FAILED"); //Error code #4 = Insert user failed

 	echo ("0");
	$connection->close();
?>
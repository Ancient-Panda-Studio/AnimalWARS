<?php
	require "constant.php";
	$connection = $CON;

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$username = $_POST["user"];
	$password = $_POST["pass"];
    $email = $_POST["email"];

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
    $insertuserquerry = "INSERT INTO users (username, hash, salt, email) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "', '" . $email . "');";
    mysqli_query($connection, $insertuserquerry) or die("Error code #4 INSERTION TO PLAYER INTO THE DATABASE FAILED"); //Error code #4 = Insert user failed
    $sqlInsert = "INSERT INTO userdata (username) VALUE ('" . $username . "');" or die("kekw ERROR");
    mysqli_query($connection, $sqlInsert) or die ("kekw");
 	echo ("0");
	$connection->close();
?>
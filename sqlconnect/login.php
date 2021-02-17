<?php
	require "constant.php";
	$connection = $CON;

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$username = $_POST["user"];
	$password = $_POST["pass"];

	//Check if name exists

	$namecheckquerry = "SELECT  id, salt, hash FROM users WHERE username='" . $username . "';";
	$namecheck = mysqli_query($connection, $namecheckquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($namecheck) == 0){
		echo "Error code #5 NO USER WITH THAT NAME IS FOUND"; //Error code #5 = NO USER EXSITS ALLOW REGISTRATION WITH 1 CLICK
		exit();
	} elseif (mysqli_num_rows($namecheck) > 1) {
		echo "Error code #6"; //Error code #6 SOMETHING WENT VERY WRONG KEKW
		exit();
	} else {
		$tableinfo = mysqli_fetch_assoc($namecheck);
		$salt = $tableinfo["salt"];
		$hash = $tableinfo["hash"];
		$loginhash = crypt($password, $salt);
		if($hash != $loginhash){
			echo "Error code #7 INCORRECT PASSWORD";
			exit();
		}	
 		echo "0\t" . $tableinfo["id"];
	}
	$connection->close();
?>
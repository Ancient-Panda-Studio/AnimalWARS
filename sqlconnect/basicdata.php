<?php
	$connection = mysqli_connect('localhost', 'root', '', 'unityacces');

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$username = $_POST["user"];

	//Check if name exists

	$basicdataquerry = "SELECT id, coins, level, currentxp FROM users WHERE username='" . $username . "';";
	$basicdata = mysqli_query($connection, $basicdataquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($basicdata) == 0){
		echo "Error code #5 NO USER WITH THAT NAME IS FOUND"; //Error code #5 = NO USER EXSITS ALLOW REGISTRATION WITH 1 CLICK
		exit();
	} elseif (mysqli_num_rows($basicdata) > 1) {
		echo "Error code #6"; //Error code #6 SOMETHING WENT VERY WRONG KEKW
		exit();
	} else {
		$tableinfo = mysqli_fetch_assoc($basicdata);
 		echo "0\t" . $tableinfo["coins"] . "\t" . $tableinfo["level"] . "\t" . $tableinfo["currentxp"] . "\t" . $tableinfo["id"];
	}
	$connection->close();
?>
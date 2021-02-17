<?php
	require "constant.php";
	$connection = $CON;

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$skinID = $_POST["id"];
	
	$skindataquerry = "SELECT image FROM skins WHERE skinID ='" . $skinID . "';";
	$skindataresult = mysqli_query($connection, $skindataquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($skindataresult) == 0){
		echo "0"; //No items found
		exit();
	} else {
		$skininfo = mysqli_fetch_assoc($skindataresult);
		$path = $skininfo["image"];
		$image = file_get_contents($path);
		echo $image;
	}
	$connection->close();
?>
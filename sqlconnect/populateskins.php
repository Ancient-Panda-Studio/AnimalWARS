<?php
	require "constant.php";
	$connection = $CON;

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	$userid = $_POST["id"];

	//Check if name exists

	$itemsownedquerry = "SELECT skinID FROM userskins WHERE userid ='" . $userid . "';";
	$itemsowned = mysqli_query($connection, $itemsownedquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($itemsowned) == 0){
		echo "0"; //No items found
		exit();
	} else {

		$rows = array();
		while ($row = mysqli_fetch_assoc($itemsowned) ) {
			$rows[] = $row;
		}
		echo json_encode($rows);
		//var_dump(json_encode($rows));
	}
	$connection->close();
?>
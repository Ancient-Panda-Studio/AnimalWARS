<?php
	require "constants.php";
	$connection = $CON;

	if(mysqli_connect_errno()){
		echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
		exit();
	}

	//Check if name exists

	$skinquerry = "SELECT skinID FROM skins;";
	$skinresult = mysqli_query($connection, $skinquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
	if(mysqli_num_rows($skinresult) == 0){
		echo "0"; //No items found
		exit();
	} else {

		$rows = array();
		while ($row = mysqli_fetch_assoc($skinresult) ) {
			$rows[] = $row;
		}
		echo json_encode($rows);
	}
	$connection->close();
?>
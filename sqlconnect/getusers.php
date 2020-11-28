<?php
	require "constant.php";
	$connection = $CON;

if(mysqli_connect_errno()){
    echo "Error code #1 CONNECTION TO DATABASE FAILED"; //Error code #1 = connection failed
    exit();
}

$username = $_POST["user"];
$username = $username . "%";

//Check if name exists

$getfriendsquerry = "SELECT username FROM users WHERE username LIKE '" . $username . "' AND online = 1;";
$friendsresult = mysqli_query($connection, $getfriendsquerry) or die("Error code #2 CONNECTION WITH TABLE FAILED"); //Error code #2 = Connection with table failed
if(mysqli_num_rows($friendsresult) == 0){
    echo "0"; //No items found
    exit();
} else {

    $rows = array();
    while ($row = mysqli_fetch_assoc($friendsresult) ) {
        $rows[] = $row;
    }
    echo json_encode($rows);
}
$connection->close();
?>
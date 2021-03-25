<?php

// code to make use of databaseoptions.php class
require_once('databaseoptions.php');
$connection = new databaseoptions;

$result = $connection->QuestionCount();
echo $result[0];

?>


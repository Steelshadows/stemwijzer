<?php

// code to make use of databaseoptions.php class
require_once('databaseoptions.php');
$connection = new databaseoptions;

$result = $connection->Questions();

$alltimes = "";
echo "[";
foreach ($result as $row) {
    $alltimes .= '"' . $row['question'] .'"' . ',';
}
$alltimes = rtrim($alltimes, ',');
echo $alltimes;
echo "]";

?>


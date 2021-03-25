<?php

// code to make use of databaseoptions.php class
require_once('databaseoptions.php');
$connection = new databaseoptions;

$result = $connection->PartyAnswers();

$return = "";
$return .= "[";
foreach ($result as $row) {
	$return .= "{";
    $return .= "party_name: ". '"' . $row['name'] . '"' . ',' . "question_id: ". '"' . $row['question_id'] .'"' . ',' . "answer: " .  '"' . $row['answer'] . '"' . "," . "party_id: ". '"' . $row['party_id'] . '"' ;
	$return .= "},";
}
$return .= "]";
$return = substr(trim($return), 0, -2) . "]";
echo $return;


?>


<?php

// PDO connection to database
class DB1
{

    public function connect()
    {
        $user = "root";
        $pass = "";

        try {
            $options = [
                PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION
            ];
            $db = new PDO('mysql:host=localhost;dbname=voting', $user, $pass, $options);
        } catch (PDOException $e) {
            echo $e->getMessage();
        }
        return $db;
    }

}


class databaseoptions {

    public function Questions() {
        $conn = (new DB1)->connect();
        $sql = $conn->prepare('SELECT * from Question');
        $sql->execute();
        return $sql->fetchAll();
    }
	public function QuestionCount(){
	    $conn = (new DB1)->connect();
        $sql = $conn->prepare('SELECT COUNT(question_id) from Question');
        $sql->execute();
        return $sql->fetchAll()[0];	
	}
	public function PartyAnswers(){
	    $conn = (new DB1)->connect();
        $sql = $conn->prepare('SELECT p.name, pa.question_id, pa.answer, pa.party_id FROM party_answers pa INNER JOIN party p WHERE p.party_id=pa.party_id; ORDER BY question_id ASC;');
        $sql->execute();
        return $sql->fetchAll();	
	}

}
?>
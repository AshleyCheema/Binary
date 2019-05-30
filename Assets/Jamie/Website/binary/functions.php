<?php

function leaderBoard(){

include "gameconfig.php";

//default value
if (strpos($_SERVER['REQUEST_URI'], "Most") !== false){
    $task = "wins";
} else if (strpos($_SERVER['REQUEST_URI'], "Played") !== false) {
    $task = "gamesPlayed";
} else if (strpos($_SERVER['REQUEST_URI'], "Hacker") !== false) {
    $task = "winSpy";
} else if (strpos($_SERVER['REQUEST_URI'], "Merc") !== false) {
    $task = "winMerc";
} else if (strpos($_SERVER['REQUEST_URI'], "Steps") !== false) {
    $task = "stepsTaken";
} else if (strpos($_SERVER['REQUEST_URI'], "Shots") !== false) {
    $task = "shotsFired";
} else if (strpos($_SERVER['REQUEST_URI'], "Abilities") !== false) {
    $task = "abilitiesUsed";
} else if (strpos($_SERVER['REQUEST_URI'], "Points") !== false) {
    $task = "pointsCaptured";
} else {
    $task = "wins";
}

//sql to find all information in the playerstats and to arrange it in descending order.
$p1 = "SELECT * FROM playerstats ORDER BY $task DESC LIMIT 0,1" ;
$p2 = "SELECT * FROM playerstats ORDER BY $task DESC LIMIT 2,1" ;
$p3 = "SELECT * FROM playerstats ORDER BY $task DESC LIMIT 3,1" ;
$p4 = "SELECT * FROM playerstats ORDER BY $task DESC LIMIT 4,1" ;
$p5 = "SELECT * FROM playerstats ORDER BY $task DESC LIMIT 5,1" ;

//Result of querying the database for player #1
$result = mysqli_query($con, $p1) or die("Bad Query: $playersql");
$row = mysqli_fetch_assoc($result);
$p1name = $row['name'];
$p1score = $row[$task];

mysqli_free_result($result);

//Result of querying the database for player #2
$result = mysqli_query($con, $p2) or die("Bad Query: $playersql");
$row = mysqli_fetch_assoc($result);
$p2name = $row['name'];
$p2score = $row[$task];

mysqli_free_result($result);

//Result of querying the database for player #3
$result = mysqli_query($con, $p3) or die("Bad Query: $playersql");
$row = mysqli_fetch_assoc($result);
$p3name = $row['name'];
$p3score = $row[$task];

mysqli_free_result($result);

//Result of querying the database for player #4
$result = mysqli_query($con, $p4) or die("Bad Query: $playersql");
$row = mysqli_fetch_assoc($result);
$p4name = $row['name'];
$p4score = $row[$task];

mysqli_free_result($result);

//Result of querying the database for player #5
$result = mysqli_query($con, $p5) or die("Bad Query: $playersql");
$row = mysqli_fetch_assoc($result);
$p5name = $row['name'];
$p5score = $row[$task];

mysqli_free_result($result);

?>
    <div class="player player1">
        <div class="row">
            <div class="col-3">#1</div>
            <div class="col-6"><?php echo ucfirst($p1name); ?></div>
            <div class="col-3  ta-r"><?php echo $p1score; ?></div>
        </div>
    </div>
    <hr>
    <div class="player player2">
        <div class="row">
            <div class="col-3">#2</div>
            <div class="col-6"><?php echo ucfirst($p2name); ?></div>
            <div class="col-3  ta-r"><?php echo $p2score; ?></div>
        </div>
    </div>
    <hr>
    <div class="player player3">
        <div class="row">
            <div class="col-3">#3</div>
            <div class="col-6"><?php echo ucfirst($p3name); ?></div>
            <div class="col-3  ta-r"><?php echo $p3score; ?></div>
        </div>
    </div>
    <hr>
    <div class="player">
        <div class="row">
            <div class="col-3">#4</div>
            <div class="col-6"><?php echo ucfirst($p4name); ?></div>
            <div class="col-3  ta-r"><?php echo $p4score; ?></div>
        </div>
    </div>
    <hr>
    <div class="player">
        <div class="row">
            <div class="col-3">#5</div>
            <div class="col-6"><?php echo ucfirst($p5name); ?></div>
            <div class="col-3  ta-r"><?php echo $p5score; ?></div>
        </div>
    </div>
    <hr style="padding-top:25px;">


<?php

}

?>
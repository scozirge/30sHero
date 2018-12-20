<?PHP
$time_start = microtime(true);
//導入config
require_once('./config.php');
//導入加密類
require_once('./3DES.php');
//導入Writer
require_once('./Writer.php');
//取得建立帳戶時間，格式為年份/月/日 時:分:秒(台北時區)
date_default_timezone_set('Asia/Taipei');
$signUpTime= date("Y/m/d H:i:s");
//接收資料
$ac_K=$_POST['ac_K'];
$userID_K=$_POST['userID_K'];
//找尋Kongergate UserID
$con_l = mysql_connect($db_host_load,$db_user,$db_pass) or ("Fail:1:"  . mysql_error());
if (!$con_l)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_l) or die ("Fail:1:" . mysql_error());

$check = mysql_query("SELECT * FROM ".$db_name.".playeraccount WHERE `userID_K`=".$userID_K,$con_l);
$numrows = mysql_num_rows($check);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
	
if ($numrows == 0)//找不到已經存在的Kongregate帳號就創新帳號
{

    if (!$con_w)
        die('Fail:1:' . mysql_error());
	$gold=100;
	$emerald=0;
	$curFloor=1;
	$maxFloor=1;
	$killBoss="";
	//寫入資料庫
    $signUpResult = mysql_query("INSERT INTO  ".$db_name.".playeraccount (  `ac_K` ,`userID_K` ,`gold` ,`emerald` ,`maxFloor`,`killBoss`,`signUpTime`,`signInTime`) VALUES ( '".$ac_K."','".$userID_K."','".$gold."','".$emerald."','".$maxFloor."','".$killBoss."' ,'".$signUpTime."','".$signUpTime."') ; ",$con_w);
	if ($signUpResult)
	{
		//流水號
		$id=mysql_insert_id();
       //對帳號進行加密
        $rep = new Crypt3Des (); // new一個加密類
        $ACPass=$rep->encrypt ( "u.6vu4".$ac."gk4ru4");
        //計算執行時間
        $time_end = microtime(true);
        $executeTime = $time_end - $time_start;
		//送回client
		die("Success:".$id.",".$gold.",".$emerald.",".$curFloor.",".$maxFloor.": \nExecuteTime=".$executeTime);
	}
	else
	{
		WriteLastMysqlError($userID_K,"新創帳號");
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		die ("Fail:5: \nExecuteTime=".$executeTime."");
	}
}
else//找到Kongregate帳戶就進行登入
{
	 if (!$con_w)
		die('Fail:1:' . mysql_error());
	$id=0;
	while($row = mysql_fetch_assoc($check))
	{
		$id=$row['id'];
		$gold=$row['gold'];
		$emerald=$row['emerald'];
		$curFloor=$row['curFloor'];
		$maxFloor=$row['maxFloor'];
		$killBoss=$row['killBoss'];
	}
	$set = mysql_query("UPDATE ".$db_name.".playeraccount SET `signInTime` = '".$signUpTime."' WHERE `userID_K` = '".$userID_K."' ",$con_w);
	if ($set)
	{
		//計算執行時間
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		//送回client
		die("Success:".$id.",".$gold.",".$emerald.",".$curFloor.",".$maxFloor.",".$killBoss.": \nExecuteTime=".$executeTime);
	}
	else
	{
		WriteLastMysqlError($userID_K,"登入舊帳號時更新登入時間");
		//計算執行時間
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		die ("Fail:5: \nExecuteTime=".$executeTime."");
	}
}

?>
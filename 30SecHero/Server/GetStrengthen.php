<?PHP
$time_start = microtime(true);
//導入config
require_once('./config.php');
//導入加密類
require_once('./3DES.php');

//取得建立帳戶時間，格式為年份/月/日 時:分:秒(台北時區)
date_default_timezone_set('Asia/Taipei');
$signUpTime= date("Y/m/d H:i:s");
//接收資料
$ownUserID=$_POST['ownUserID'];
//找尋Kongergate UserID
$con_l = mysql_connect($db_host_load,$db_user,$db_pass) or ("Fail:1:"  . mysql_error());
if (!$con_l)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_l) or die ("Fail:1:" . mysql_error());
$check = mysql_query("SELECT * FROM ".$db_name.".strengthen WHERE `ownUserID`=".$ownUserID,$con_l);
$numrows = mysql_num_rows($check);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());

if ($numrows == 0)//找不到已經存在的Kongregate帳號
{
    //計算執行時間
    $time_end = microtime(true);
    $executeTime = $time_end - $time_start;
	die("Success:: \nExecuteTime=".$executeTime);
}
else//找到Kongregate帳戶
{
	 if (!$con_w)
		die('Fail:1:' . mysql_error());
    $currentRankStr='';
    $dataCount=0;
	while($row = mysql_fetch_assoc($check))
	{
        if($dataCount!=0)
            $currentRankStr.='/';
		//$id=$row['id'];
		$jid=$row['jid'];
		$lv=$row['lv'];
		$currentRankStr.=$jid.','.$lv;
		$dataCount++;
	}
    //計算執行時間
    $time_end = microtime(true);
    $executeTime = $time_end - $time_start;
	//送回client
	die("Success:".$currentRankStr.": \nExecuteTime=".$executeTime);
}

?>
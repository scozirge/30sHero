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
$now= date("Y/m/d H:i:s");
//接收資料
$ownUserID=$_POST['ownUserID'];
$jid=$_POST['jid'];
$lv=$_POST['lv'];
$emerald=$_POST['emerald'];
//新增讀取DB連線
$con_l = mysql_connect($db_host_load,$db_user,$db_pass) or ("Fail:1:"  . mysql_error());
if (!$con_l)
	die('Fail:1:' . mysql_error());
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
if (!$con_w)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_l) or die ("Fail:1:" . mysql_error());
$check = mysql_query("SELECT * FROM ".$db_name.".enchant WHERE `ownUserID` = '".$ownUserID."' AND `jid` = '".$jid."'",$con_l);
$numrows = mysql_num_rows($check);
$enchantResult=false;
if ($numrows == 0)//找不到已經存在的附魔資料就建立一筆新的
{
	//寫入資料庫
    $insertResult = mysql_query("INSERT INTO  ".$db_name.".enchant (  `jid` ,`lv` ,`ownUserID`,`acquireTime`) VALUES ( '".$jid."','".$lv."','".$ownUserID."','".$now."') ; ",$con_w);
	if($insertResult)
		$enchantResult=true;
}
else
{
	$set = mysql_query("UPDATE ".$db_name.".enchant SET `lv` = '".$lv."', `acquireTime` = '".$now."' WHERE `ownUserID` = '".$ownUserID."' AND `jid` = '".$jid."'",$con_w);
	//更新資料的回傳結果
	if($set)
		$enchantResult=true;
}
if($enchantResult)
{
	$set = mysql_query("UPDATE ".$db_name.".playeraccount SET `emerald` = '".$emerald."' WHERE `id` = '".$ownUserID."' ",$con_w);
	if($set)
	{
		//計算執行時間
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		die("Success:".$jid.": \nExecuteTime!=".$executeTime);
	}
	else
	{
		WriteLastMysqlError($ownUserID,"附魔後更新玩家資源");
		die("Fail:5");
	}
}
else
{
	WriteLastMysqlError($ownUserID,"附魔");
	die("Fail:5");
}


?>
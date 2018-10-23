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
$ids=$_POST['ids'];
//die("ID:".$id1.",".$equipSlot1."  ID2:".$id2.",".$equipSlot2);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
if (!$con_w)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_w) or die ("Fail:1:" . mysql_error());
$sellEquips=explode(",",$ids);
$arrayCount=count($sellEquips);
$result=true;
if($arrayCount!=0)
{
	for($i=0;$i<$arrayCount;$i++)
	{
		$set = mysql_query("DELETE FROM ".$db_name.".equipment WHERE `ownUserID` = '".$ownUserID."' AND `id` = '".$sellEquips[$i]."'",$con_w);
		if(!$set)
			$result=false;
	}
	if($result)
	{
		//計算執行時間
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		die("Success:: \nExecuteTime=".$executeTime);
	}
	else
	{
		WriteLastMysqlError($id,"販賣裝備");
		die("Fail:5");
	}
		
}
else
{
	//計算執行時間
	$time_end = microtime(true);
	$executeTime = $time_end - $time_start;
	die("Success:: \nExecuteTime=".$executeTime);
}

?>
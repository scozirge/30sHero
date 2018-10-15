<?PHP
$time_start = microtime(true);
//導入config
require_once('./config.php');
//導入加密類
require_once('./3DES.php');

//取得建立帳戶時間，格式為年份/月/日 時:分:秒(台北時區)
date_default_timezone_set('Asia/Taipei');
$now= date("Y/m/d H:i:s");
//接收資料
$ownUserID=$_POST['ownUserID'];
$id1=$_POST['id1'];
$equipSlot1=$_POST['equipSlot1'];
$id2=$_POST['id2'];
$equipSlot2=$_POST['equipSlot2'];
//die("ID:".$id1.",".$equipSlot1."  ID2:".$id2.",".$equipSlot2);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
if (!$con_w)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_w) or die ("Fail:1:" . mysql_error());
$result1=false;
$result2=false;

if($id1!=0)
{
	$set = mysql_query("UPDATE ".$db_name.".equipment SET `equipSlot` = '".$equipSlot1."' WHERE `ownUserID` = '".$ownUserID."' AND `id` = '".$id1."'",$con_w);
	if($set)
		$result1=true;
}
else
	$result1=true;

if($id2!=0)
{
	$set2 = mysql_query("UPDATE ".$db_name.".equipment SET `equipSlot` = '".$equipSlot2."' WHERE `ownUserID` = '".$ownUserID."' AND `id` = '".$id2."'",$con_w);
	if($set2)
		$result2=true;
}
else
	$result2=true;

if($result1 && $result2)
{
	//計算執行時間
	$time_end = microtime(true);
	$executeTime = $time_end - $time_start;
	die("Success:: \nExecuteTime!=".$executeTime);
}
else
	die("Fail:5");
?>
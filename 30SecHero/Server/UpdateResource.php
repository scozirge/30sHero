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
$id=$_POST['id'];
$gold=$_POST['gold'];
$emerald=$_POST['emerald'];
//die("ID:".$id1.",".$equipSlot1."  ID2:".$id2.",".$equipSlot2);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
if (!$con_w)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_w) or die ("Fail:1:" . mysql_error());


$set = mysql_query("UPDATE ".$db_name.".playeraccount SET `gold` = '".$gold."', `emerald` = '".$emerald."' WHERE `id` = '".$id."'",$con_w);
if($set)
{
	//計算執行時間
	$time_end = microtime(true);
	$executeTime = $time_end - $time_start;
	die("Success:: \nExecuteTime!=".$executeTime);
}
else
	die("Fail:5");

?>
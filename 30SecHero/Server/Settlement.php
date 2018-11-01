<?PHP
$time_start = microtime(true);
//導入config
require_once('./config.php');
//導入Writer
require_once('./Writer.php');
//導入加密類
require_once('./3DES.php');

//取得建立帳戶時間，格式為年份/月/日 時:分:秒(台北時區)
date_default_timezone_set('Asia/Taipei');
$now= date("Y/m/d H:i:s");
//接收資料
$id=$_POST['id'];
$gold=$_POST['gold'];
$emerald=$_POST['emerald'];
$curFloor=$_POST['curFloor'];
$maxFloor=$_POST['maxFloor'];
$equipStr=$_POST['equipStr'];
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
if (!$con_w)
	die('Fail:1:' . mysql_error());
$settlementResult=false;
$update = mysql_query("UPDATE ".$db_name.".playeraccount SET `gold` = '".$gold."', `emerald` = '".$emerald."', `curFloor` = '".$curFloor."', `maxFloor` = '".$maxFloor."' WHERE `id` = '".$id."'",$con_w);
if($update)
{
	if($equipStr=="")
	{
		//計算執行時間
		$time_end = microtime(true);
		$executeTime = $time_end - $time_start;
		die("Success:".$gold."/".$emerald."/".$curFloor."/".$maxFloor.": \nExecuteTime=".$executeTime);
	}
	else
	{

		$equips=explode("/",$equipStr);
		$arrayCount=count($equips);
		$result=true;
		$uidStr="";
		for($i=0;$i<$arrayCount;$i++)
		{
			if($i!=0)
				$uidStr.=",";
			$equipData=explode(",",$equips[$i]);
			$jid=$equipData[0];
			$equipType=$equipData[1];
			$equipSlot=$equipData[2];
			$lv=$equipData[3];
			$quality=$equipData[4];
			$property=$equipData[5];
			$ownUserID=$equipData[6];
			//寫入資料庫
			$insert = mysql_query("INSERT INTO  ".$db_name.".equipment (  `jid` ,`equipType` ,`equipSlot`,`lv`,`quality`,`property`,`ownUserID`,`acquireTime`) VALUES ( '".$jid."','".$equipType."','".$equipSlot."','".$lv."','".$quality."','".$property."','".$ownUserID."','".$now."') ; ",$con_w);
			if($insert)
			{
				$uid=mysql_insert_id();
				$uidStr.=$uid;
			}
			else
			{
				$result=false;
			}
		}

		if($result)
		{			
			//計算執行時間
			$time_end = microtime(true);
			$executeTime = $time_end - $time_start;
			die("Success:".$gold."/".$emerald."/".$curFloor."/".$maxFloor."/".$uidStr.": \nExecuteTime=".$executeTime);
		}
		else
		{
			WriteLastMysqlError($id,"結算時新增裝備資料");
			die("Fail:5");
		}
	}
}
else
{
	WriteLastMysqlError($id,"結算時更新玩家資料");
	die("Fail:5");
}





?>
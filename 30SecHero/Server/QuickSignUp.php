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
//如果server端判斷資料庫中沒此kg帳戶的玩家就新增kg帳戶並把本地送來的資料(以下)新增到DB
	$gold=$_POST['gold'];
	$emerald=$_POST['emerald'];
	$freeEmerald=$_POST['freeEmerald'];
	$payEmerald=$_POST['payEmerald'];
	$curFloor=$_POST['curFloor'];
	$maxFloor=$_POST['maxFloor'];
	$killBoss=$_POST['killBoss'];
	$equipStr=$_POST['equipStr'];
	$strengthenStr=$_POST['strengthenStr'];
	$enchantStr=$_POST['enchantStr'];
	//echo "gold=".$gold." emerald=".$emerald." freeEmerald=".$freeEmerald." payEmerald=".$payEmerald." curFloor=".$curFloor." maxFloor=".$maxFloor." killBoss=".$killBoss." equipStr=".$equipStr." strengthenStr=".$strengthenStr." enchantStr=".$enchantStr;




//找尋Kongergate UserID
$con_l = mysql_connect($db_host_load,$db_user,$db_pass) or ("Fail:1:"  . mysql_error());
if (!$con_l)
	die('Fail:1:' . mysql_error());
mysql_select_db($db_name , $con_l) or die ("Fail:1:" . mysql_error());

$check = mysql_query("SELECT * FROM ".$db_name.".playeraccount WHERE `userID_K`=".$userID_K,$con_l);
$numrows = mysql_num_rows($check);
//新增寫入DB連線
$con_w = mysql_connect($db_host_write,$db_user,$db_pass,true) or ("Fail:1:"  . mysql_error());
	
if ($numrows == 0)//找不到已經存在的Kongregate帳號就創新帳號並把本地送來的資料(以下)新增到DB
{

    if (!$con_w)
        die('Fail:1:' . mysql_error());
	$trueEmerald=0;
	$payKredsLog="";
	//寫入資料庫
    $signUpResult = mysql_query("INSERT INTO  ".$db_name.".playeraccount (  `ac_K` ,`userID_K` ,`gold` ,`emerald`,`trueEmerald`,`freeEmerald`,`payEmerald`,`payKredsLog` ,`curFloor`,`maxFloor`,`killBoss`,`signUpTime`,`signInTime`) VALUES ( '".$ac_K."','".$userID_K."','".$gold."','".$emerald."','".$trueEmerald."','".$freeEmerald."','".$payEmerald."','".$payKredsLog."','".$curFloor."','".$maxFloor."','".$killBoss."' ,'".$signUpTime."','".$signUpTime."') ; ",$con_w);
	
	if ($signUpResult)
	{
		//流水號
		$id=mysql_insert_id();
		$newConn = mysqli_connect($db_host_write, $db_user, $db_pass, $db_name);
		//裝備資料
		if($equipStr!="")
		{
			$equipData=explode('/',$equipStr);
			$query="";
			for($i=0; $i< count($equipData);$i++)
			{
				$data= explode(',',$equipData[$i]);
				//如果有8筆代表此裝備有附魔資料
				if(count($data)==8)
					$query.="INSERT INTO  ".$db_name.".equipment (  `jid` ,`equipType` ,`equipSlot` ,`lv`,`quality`,`property`,`enchant`,`ownUserID` ) VALUES ( '".$data[1]."','".$data[2]."','".$data[3]."','".$data[4]."','".$data[5]."','".$data[6]."','".$data[7]."','".$id."') ; ";
				else if (count($data)==7)
					$query.="INSERT INTO  ".$db_name.".equipment (  `jid` ,`equipType` ,`equipSlot` ,`lv`,`quality`,`property`,`ownUserID` ) VALUES ( '".$data[1]."','".$data[2]."','".$data[3]."','".$data[4]."','".$data[5]."','".$data[6]."','".$id."') ; ";

			}
			// Check connection
			if (!$newConn) {
				die ("Fail:5: b\nExecuteTime=".$executeTime."");
			}
			else
			{
				if (mysqli_multi_query($newConn,$query))
				{
					do
					{
						if ($equipUpdate=mysqli_store_result($newConn))
						{
							if(!$equipUpdate)
							{
								WriteLastMysqlError($userID_K,"使用Kongregate帳號登入時，上傳裝備資料發生錯誤:".$equipUpdate."<br>".mysqli_error($newConn));
								//die ("Fail:5:"."Error " . $equipUpdate . "<br>" . mysqli_error($newConn)." \nExecuteTime=".$executeTime."");
							}
							mysqli_free_result($equipUpdate);
						}
					}
					while (mysqli_next_result($newConn));
				}
				//$equipUpdate=mysqli_multi_query($newConn,$query);				
			}
		}
		//強化資料
		if($strengthenStr!="")
		{
			$strengthenData=explode('/',$strengthenStr);
			$query="";
			for($i=0; $i< count($strengthenData);$i++)
			{
				$data= explode(',',$strengthenData[$i]);
				$query.="INSERT INTO  ".$db_name.".strengthen (  `jid` ,`lv` ,`ownUserID` ) VALUES ( '".$data[0]."','".$data[1]."','".$id."') ; ";

			}
			// Check connection
			if (!$newConn) {
				die ("Fail:5: a\nExecuteTime=".$executeTime."");
			}
			else
			{
				if (mysqli_multi_query($newConn,$query))
				{
					do
					{
						if ($strengthenUpdate=mysqli_store_result($newConn))
						{
							if(!$strengthenUpdate)
							{
								WriteLastMysqlError($userID_K,"使用Kongregate帳號登入時，上傳強化資料發生錯誤:".$strengthenUpdate."<br>".mysqli_error($newConn));
								//die ("Fail:5:"."Error " . $strengthenUpdate . "<br>" . mysqli_error($newConn)." \nExecuteTime=".$executeTime."");
							}
							mysqli_free_result($strengthenUpdate);
						}
					}
					while (mysqli_next_result($newConn));
				}
			}
		}
		//附魔資料
		if($enchantStr!="")
		{
			$enchantData=explode('/',$enchantStr);
			$query="";
			for($i=0; $i< count($enchantData);$i++)
			{
				$data= explode(',',$enchantData[$i]);
				$query.="INSERT INTO  ".$db_name.".enchant (  `jid` ,`lv` ,`ownUserID` ) VALUES ( '".$data[0]."','".$data[1]."','".$id."') ; ";
			}
			// Check connection
			if (!$newConn) {
				die ("Fail:5: a\nExecuteTime=".$executeTime."");
			}
			else
			{
				if (mysqli_multi_query($newConn,$query))
				{
					do
					{
						if ($enchantUpdate=mysqli_store_result($newConn))
						{
							if(!$enchantUpdate)
							{
								WriteLastMysqlError($userID_K,"使用Kongregate帳號登入時，上傳附魔資料發生錯誤:".$enchantUpdate."<br>".mysqli_error($newConn));
								//die ("Fail:5:"."Error " . $enchantUpdate . "<br>" . mysqli_error($newConn)." \nExecuteTime=".$executeTime."");
							}
							mysqli_free_result($enchantUpdate);
						}
					}
					while (mysqli_next_result($newConn));
				}
			}
		}


		
        //計算執行時間
        $time_end = microtime(true);
        $executeTime = $time_end - $time_start;
		//送回client
		die("Success:".$id.",".$gold.",".$emerald.",".$trueEmerald.",".$freeEmerald.",".$payEmerald.",".$payKredsLog.",".$curFloor.",".$maxFloor.",".$killBoss.": \nExecuteTime=".$executeTime);
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
		$trueEmerald=$row['trueEmerald'];
		$freeEmerald=$row['freeEmerald'];
		$payEmerald=$row['payEmerald'];
		$payKredsLog=$row['payKredsLog'];
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
		die("Success:".$id.",".$gold.",".$emerald.",".$trueEmerald.",".$freeEmerald.",".$payEmerald.",".$payKredsLog.",".$curFloor.",".$maxFloor.",".$killBoss.": \nExecuteTime=".$executeTime);
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
<?php
/**
 * DEVOPS预上线镜像更新脚本
 * Created by PhpStorm.
 * User: laijingming
 * Date: 2023/8/31
 * Time: 15:07
 */
define('UNIQID', 1514362957);
define('BASE_URL', 'https://devops-gateway.om.dianhun.cn/DEVOPSEDGE/api/task/');
define('HISTORY_PAGE_URL', BASE_URL . 'getHistoryPage');//历史作业信息地址
define('WORK_URL', BASE_URL . 'perform/');//执行作业地址
define('WORK_INFO_URL', BASE_URL . 'getAll/713');//获取要执行作业信息
header( "Content-type: text/html; charset=utf-8" );
if (!isset($argv[1],$argv[2])) {
    exit('需要参数1');
}
define('API_KEY', $argv[1]);

$skins  = explode(',', trim($argv[2]));
$devops = new Devops($skins);
$devops->run();

class Devops
{
    public $headers = [];
    public $params = [];
    public $sev_cfg = [
        1 => '【游动都市预上线服】业务环境初始化',
        2 => '【游动都市安锋繁体预上线】业务环境初始化',
        3 => '【游动都市BBG韩国预上线服】业务环境初始化',
        4 => '【都市绿洲日本预上线-new】业务环境初始化',
        9 => '【都市十九岁测试服】业务环境初始化',

        5  => '【游动都市霸道总裁镜像服】业务环境初始化',
        6  => '【游动都市安锋繁体镜像服】业务环境初始化',
        7  => '【游动都市BBG韩国镜像服】业务环境初始化',
        8  => '【游动都市绿洲日本镜像服】业务环境初始化',
        10 => '【都市十九岁镜像服】业务环境初始化',
    ];
    public $skins = [];
    public $info;

    public function __construct($_skins)
    {
        $this->headers[] = 'Authorization: Bearer ' . API_KEY;
        $this->params['uniqId'] = UNIQID;
        foreach ($_skins as $v) {
            $this->skins[] = "【".$v."】业务环境初始化";
        }
    }

    public function run()
    {
        $this->check();
        foreach ($this->info as $v) {
            $res = $this->request(WORK_URL . $v['id'], $this->params, 'POST', $this->headers);
            echo $v['name'], $res, PHP_EOL;
            $num = 30;
            while ($num > 0) {
                if ($this->history($v['id'], $this->params, $this->headers) != 1) {
                    break;//返回执行作业结果
                }
                $num--;
                sleep(5);
            }
        }
    }

    public function history($work_id, $params, $headers)
    {
        $stateArr         = [0 => '未知', 1 => '正在执行', 2 => '成功', 3 => '失败',];
        $params['page']   = 1;
        $params['limit']  = 1;
        $params['gameId'] = 713;
        $params['taskId'] = $work_id;
        $res              = $this->request(HISTORY_PAGE_URL, $params, 'GET', $headers);
        $data             = json_decode($res, true);
        $state            = 0;
        if ($data && $data['code'] == 0) {
            $data  = $data['data']['content'][0];
            $state = $data['workHistory']['state'];
            echo $data['taskName'] . ' ' . $data['createdAt'], '-', $stateArr[$state], PHP_EOL;
        }
        return $state;
    }

    public function check()
    {
        if (empty($this->skins)) {
            die('作业不存在' . __LINE__ . PHP_EOL);
        }
        $this->info = $this->getWorkInfo($this->skins, $this->params, $this->headers);
        if (empty($this->info)) {
            die('作业不存在' . __LINE__ . PHP_EOL);
        }
    }

    public function getWorkInfo($skin, $params, $headers)
    {
        $work_info = [];
        if (!empty($skin)) {
            $res  = $this->request(WORK_INFO_URL, $params, 'GET', $headers);
            $data = json_decode($res, true);
            if (isset($data['code']) && $data['code'] == 0) {
                foreach ($data['data'] as $v) {
                    if (in_array(trim($v['name']), $skin)) {
                        $work_info[] = $v;
                    }
                }
            } else {
                echo '获取作业信息失败', PHP_EOL;
            }

        }
        return $work_info;
    }


    public function request($url, $params = '', $mode = 'POST', $header = array(), $timeout = 5)
    {
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, $timeout);
        curl_setopt($ch, CURLOPT_TIMEOUT, $timeout);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        if (!empty($header)) {
            curl_setopt($ch, CURLOPT_HTTPHEADER, $header);
        }
        if (strtolower($mode) == 'post') {
            curl_setopt($ch, CURLOPT_POST, true);
            curl_setopt($ch, CURLOPT_POSTFIELDS, (is_array($params) ? http_build_query($params) : $params));
        } else {
            $url .= (strpos($url, '?') === false ? '?' : '&') . (is_array($params) ? http_build_query($params) : $params);
        }
        // 线下环境不用开启curl证书验证, 未调通情况可尝试添加该代码
        curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
        curl_setopt($ch, CURLOPT_URL, $url);

        $result = curl_exec($ch);
        $errno  = curl_errno($ch);
        $errno && $result = $errno;

        curl_close($ch);
        return $result;
    }
}
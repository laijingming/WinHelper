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
define('LIST_AUTH_GAMES_URL', "https://devops-gateway.om.dianhun.cn/DEVOPSEDGE/api/me/list-auth-games");//获取要执行作业项目信息
header("Content-type: text/html; charset=utf-8");
if (!isset($argv[1], $argv[2])) {
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

    public $info;

    public function __construct($_skins)
    {
        $this->headers[]        = 'Authorization: Bearer ' . API_KEY;
        $this->params['uniqId'] = UNIQID;
        foreach ($_skins as $v) {
            $this->skins[] = trim($v);
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
                if ($this->history($v['id'],$v['gameId'], $this->params, $this->headers) != 1) {
                    break;//返回执行作业结果
                }
                $num--;
                sleep(5);
            }
        }
    }

    public function history($work_id, $game_id, $params, $headers)
    {
        $stateArr         = [0 => '未知', 1 => '正在执行', 2 => '成功', 3 => '失败',];
        $params['page']   = 1;
        $params['limit']  = 1;
        $params['gameId'] = $game_id;
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
            $games = $this->request(LIST_AUTH_GAMES_URL, $params, 'GET', $headers);
            $games = json_decode($games, true);
            if (isset($games['code']) && $games['code'] == 0) {
                foreach ($games['data'] as $v) {
                    $url = BASE_URL . 'getAll/' . $v['id'];
                    $res = $this->request($url, $params, 'GET', $headers);
                    $res = json_decode($res, true);
                    if (isset($res['code']) && $res['code'] == 0) {
                        foreach ($res['data'] as $vv) {
                            if (in_array(trim($vv['name']), $skin)) {
                                $work_info[] = $vv;
                            }
                        }
                    }
                }
            }
        }
        if (empty($work_info)) {
            die('获取作业信息失败' . __LINE__ . PHP_EOL);
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
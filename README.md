# MyRoomServer
![lines](https://img.shields.io/tokei/lines/github/MyRoomCampus/MyRoomServer?style=flat-square)
![GitHub repo size](https://img.shields.io/github/repo-size/MyRoomCampus/MyRoomServer?style=flat-square)
![GitHub issues](https://img.shields.io/github/issues/MyRoomCampus/MyRoomServer?style=flat-square)
![GitHub closed issues](https://img.shields.io/github/issues-closed/MyRoomCampus/MyRoomServer?style=flat-square)
[![Deploy](https://github.com/MyRoomCampus/MyRoomServer/actions/workflows/deploy.yml/badge.svg?branch=master)](https://github.com/MyRoomCampus/MyRoomServer/actions/workflows/deploy.yml)

此项目为**麦荣MyRoom**的服务端，项目使用 ASP.NET 框架。

## 接口说明

### 鉴权部分

鉴权部分使用[JWT](https://jwt.io/)，使用 AccessToken 和 RefreshToken。

AccessToken 有效时间较短，用户在获取资源的时候需要携带 AccessToken。
当 AccessToken 过期后，用户需要获取一个新的 AccessToken。

RefreshToken 用于获取新的 AccessToken。
这样可以缩短 AccessToken 的过期时间保证安全，同时又不会因为频繁过期重新要求用户登录。

#### 如何使用

- 携带 header `Authorization: "Bearer {AccessToken}"` 访问需要鉴权的资源

- 携带 header `Authorization: "Bearer {RefreshToken}"` 刷新 Token

JWT分为三个部分。HEADER、PAYLOAD、VERIFY SIGNATURE，以'.'分割，使用 base64 编码。
那么获取过期时间就可以..

```javascript
let parts = accessToken.split('.')        // 分割 token
let payloadCoded = parts[1]               // 获取 base64 编码过的 payload
let payload = JSON.parse(atob(payload))   // base64 解码 读取 JSON
let exp = payload['exp']                  // 获取过期时间
```

#### 用户注册

目前未对用户注册做绑定要求，注册接口要求用户名及密码均为6位长度及以上，用户名不重复则注册成功。返回码说明详见Apifox。

#### 用户登录

用户登录验证账号及密码成功后，会返回 token，样例如下。

```json
{
    "accessToken": "eyJhbGciOiJ...",
    "refreshToken": "eyJhbGciOiJIUzI1..."
}
```

accessToken 为用户访问接口使用，当 accessToken 过期时，可携带 refreshToken 访问刷新 token 接口，依据时间 refreshToken 会返回 accessToken，或同时返回 accessToken 和新的 refreshToken。

#### 刷新Token

携带 refreshToken 访问。刷新AccessToken和RefreshToken。

当RefreshToken未临期时，只返回新的AccessToken；当RefreshToken临期时，同时返回新的AccessToken和RefreshToken。

AccessToken有效期为60分钟，RefreshToken有效期为432000分钟，当RefreshToken剩余有效期不足10080分钟时进入临期状态。

#### 登录流程

![登录流程](docs/images/登录流程.drawio.svg)

### SignalR

#### 方法使用流程
![方法使用流程](docs/images/项目管理员与用户交互.drawio.svg)

## 代码提交

Commit message 使用 [Angular 规范](https://www.ruanyifeng.com/blog/2016/01/commit_message_change_log.html)

向主分支的提交需提交 Pull Request，并使用 squash merge

## 项目部署

### 自动部署

当 master 分支发生 push 动作是进行部署。

### 手动部署

使用 docker 部署项目，在项目根目录下运行 `docker compose up` 即可。

注意需要配置相应的环境变量，详见 [docker-compose.yml](docker-compose.yml)

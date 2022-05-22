# MyRoomServer

此项目为**麦荣MyRoom**的服务端，项目使用 ASP.NET 框架。

## 接口说明

### 鉴权部分

鉴权部分使用[JWT](https://jwt.io/)，

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

### SignalR

待写...

## 项目部署

...

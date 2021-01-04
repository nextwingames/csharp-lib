# C# Library
[![Download](https://img.shields.io/badge/download-v2.1.0-blue)](https://github.com/nextwingames/csharp-lib/releases/download/2.1.0/Nextwin.dll)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/nextwingames/csharp-lib/blob/main/LICENSE)

C#으로 게임 클라이언트 및 서버를 개발할 수 있도록 도와주는 라이브러리로 기타 다른 C# 어플리케이션에서도 사용 가능합니다.
다음 프로젝트에서 사용하였으며 사용 예시를 확인할 수 있습니다.

[Unity Client Framework](https://github.com/nextwingames/unity-client), [C# Game Server Framework](https://github.com/nextwingames/csharp-server)

## Contents
- [Nextwin.Net](#nextwinnet)
- [Nextwin.Protocol](#nextwinprotocol)
- [Nextwin.Util](#nextwinutil)

## Nextwin.Net
통신 및 세션 관련
### NetworkManager
통신을 담당하는 클래스로 특정 서버에 연결하거나 데이터를 전송, 수신하는 역할을 수행합니다.

생성자는 다음과 같이 클라이언트용과 서버용으로 나누어져 있습니다.
```C#
// 클라이언트에서 생성할 때
NetworkManager networkManager = new NetworkManager(serializer);

// 서버에서 생성할 때
NetworkManager networkManager = new NetworkManager(socket, serializer);
```
이때 생성자 인자로 [ISerializer 인터페이스](#iserializer)를 상속받아 구현된 클래스를 전달해주어야 합니다. 서버측에는 서버 Listener가 Accept한 소켓도 함께 인자로 전달합니다.

클라이언트에서 서버에 접속할 때에는 Connect() 함수를 호출합니다. 또한 연결을 해제하기 위해 Disconnect() 함수를 사용합니다.
```C#
/// IP:127.0.0.1, PORT:9000 인 서버에 접속
networkManager.Connect(127.0.0.1, 9000);

/// 연결 해제
networkManager.Disconnect();
```

전송 및 수신은 다음과 같이 실행할 수 있습니다.
```C#
/// 전송
networkManager.Send(data);

/// 수신
byte[] buffer = networkManager.Receive();
```
Send 함수의 인자로는 모든 클래스가 전달될 수 있습니다.

## Nextwin.Protocol
통신을 위해 필요한 프로토콜을 정의합니다. DataBundle, Header, IDto는 더이상 사용되지 않습니다.
### ISerializer
통신을 위해 직렬화 및 역직렬화를 구현할 인터페이스입니다.
```C#
byte[] Serialize<T>(T data);
T Deserialize<T>(byte[] bytes);
```

## Nextwin.Util
기타 유틸입니다. JsonManager는 더이상 사용되지 않습니다.
### ArrayStringConverter
배열을 string으로 변환합니다.
### EnumConverter
string을 enum으로 변환합니다.
### Print
콘솔 출력을 위한 유틸입니다. 유니티와 콘솔 앱에서 모두 사용할 수 있습니다.
### ProcessManager
다른 프로세스를 실행시켜주기 위한 유틸입니다. 현재 개발중입니다.

cmd창에서 dir 명령어(window)를 수행할 때 다음과 같이 사용합니다.
```C#
// dir 명령어 실행 결과를 result에 저장
string result = ProcessManager.SyncCmd("dir");
// dir 실행 결과 출력
Print.Log(result);
```
